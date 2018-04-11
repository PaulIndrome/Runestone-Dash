using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDashChaining : MonoBehaviour {
	public PlayerState playerState;
	public Animator playerAnimator;
	private Player player;
	private AudioSource playerAudioSource;
	private NaginataControl naginataControl;
	
	public float dashRadius, dashSpeed;
	private Queue<Vector3> targetStash;
	[Header("Particle spawners")]
	public ParticlePooler playerDashKickoffPooled;
	
	[Header("Audio Events")]
	public AudioEvent audioPlayerDash;
	public AudioEvent audioPlayerKickoff;

	public void Start(){
		targetStash = new Queue<Vector3>();
		playerAudioSource = GetComponent<AudioSource>();
		player = GetComponent<Player>();
		naginataControl = GetComponentInChildren<NaginataControl>();
	}

	public void Update(){
		if(targetStash.Count > 0 && playerState.canDash){
			playerState.isDashing = true;
			StartCoroutine(DashInDirection(targetStash.Dequeue()));
		}
	}
	
	public void StashTarget(Vector3 targetWorldPos){
		if(targetStash.Count < 4)
			targetStash.Enqueue(targetWorldPos);
	}

	public void StartNaginataParticles(bool startStop){
		if(startStop)
			naginataControl.StartBladeTrail();
		else
			naginataControl.StopBladeTrail();
	}

	IEnumerator DashInDirection(Vector3 target){
		playerAnimator.SetBool("isDashing", true);
		StartNaginataParticles(true);
		playerState.canDash = false;
		playerState.isDashing = true;
		transform.LookAt(target);
		Vector3 direction = (target - transform.position).normalized;
		direction.y = 0;
		direction = transform.position + direction.normalized * dashRadius;
		audioPlayerKickoff.PlayOneShot(playerAudioSource);
		audioPlayerDash.PlayOneShot(playerAudioSource);
		//playerDashKickoff.SpawnAndPlay(null, transform.position, direction);
		playerDashKickoffPooled.SpawnFromQueueAndPlay(null, transform.position, direction);
		//Time.timeScale = 0.75f;
		while(Vector3.Distance(transform.position, direction) > 0.1f && !playerState.hitEnemyShield){
			transform.position = Vector3.MoveTowards(transform.position, direction, Time.deltaTime * dashSpeed);
			yield return null;
		}
		//Time.timeScale = 1f;
		StartNaginataParticles(false);
		yield return new WaitForSeconds(0.15f);
		playerState.canDash = true;
		playerState.hitEnemyShield = false;
		yield return new WaitForSeconds(0.45f);
		playerAnimator.SetBool("isDashing", false);
		playerState.isDashing = false;
		player.ResetColliderWidth();
		yield return null;
	}

}
