using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDashChaining : MonoBehaviour {
	public Animator playerAnimator;
	Player player;
	AudioSource playerAudioSource;
	NaginataControl naginataControl;
	
	public float dashRadius, dashSpeed;
	Queue<Vector3> targetStash;
	bool targetStashEmpty = true;

	[Header("Particle spawners / systems")]
	public ParticlePooler playerDashKickoffPooled;
	[SerializeField] ParticleSystem[] chainKillParticles;
	
	[Header("Audio Events")]
	public AudioEvent audioPlayerDash;
	public AudioEvent audioPlayerKickoff;

	Coroutine playerDash, shovedBack;

	public void Start(){
		targetStash = new Queue<Vector3>();
		playerAudioSource = GetComponent<AudioSource>();
		player = GetComponent<Player>();
		naginataControl = GetComponentInChildren<NaginataControl>();

		foreach(ParticleSystem p in chainKillParticles)
			p.Pause();
	}

	public void Update(){
		if(targetStash.Count > 0 && player.playerState.canDash && player.playerState.canMove){
			if(!player.playerState.isDashing) playerAnimator.SetTrigger("startDashChain");
			else playerAnimator.SetTrigger("additionalDash");
			player.playerState.isDashing = true;
			if(playerDash != null) StopCoroutine(playerDash);
			playerDash = StartCoroutine(DashInDirection(targetStash.Dequeue()));
		} 
	}
	
	public void StashTarget(Vector3 targetWorldPos){
		targetStashEmpty = false;
		if(player.playerState.canMove)
			targetStash.Enqueue(targetWorldPos);
	}

	public void ClearTargetStash(){
		targetStash.Clear();
		targetStashEmpty = true;
	}

	public bool isTargetStashEmpty(){
		return targetStashEmpty;
	}

	public void SetChainKillParticlesEnabled(bool enabled){
		foreach(ParticleSystem p in chainKillParticles){
			if(enabled) p.Play();
			else p.Pause();
		}
	}

	public void EndDashChain(){
		if(playerDash != null) {
			StopCoroutine(playerDash);
			playerDash = null;
		}
		ClearTargetStash();
		playerAnimator.SetBool("isDashing", false);
		player.playerState.isDashing = false;
		player.playerState.canDash = true;
		player.ResetColliderWidth();
		naginataControl.StopBladeTrail();
	}

	public void PlayerShovedBack(){
		player.playerState.hitEnemyShield = true;
		if(playerDash != null) {
			StopCoroutine(playerDash);
			playerDash = null;
		}
		shovedBack = StartCoroutine(ShovedBack());
	}

	public void StoodBackUp(){
		ClearTargetStash();
		playerAnimator.SetBool("grounded", false);
		player.playerState.hitEnemyShield = false;
		player.playerState.canMove = true;
		player.playerState.canDash = true;
	}

	public IEnumerator DashInDirection(Vector3 target){
		playerAnimator.SetBool("isDashing", true);
		player.playerState.canDash = false;
		player.playerState.isDashing = true;
		
		transform.LookAt(target);
		Vector3 direction;
		if(!player.playerState.isLegendary){
			direction = (transform.position + (target - transform.position).normalized * dashRadius) * 1.01f;
		} else {
			direction = transform.position + (target - transform.position);
		}
		direction.y = 0;
		
		audioPlayerKickoff.PlayOneShot(playerAudioSource);
		audioPlayerDash.PlayOneShot(playerAudioSource);
		
		playerDashKickoffPooled.SpawnFromQueueAndPlay(null, transform.position, direction);
		naginataControl.StartBladeTrail();

		while(Vector3.Distance(transform.position, direction) > 0.05f && !player.playerState.hitEnemyShield){
			transform.position = Vector3.MoveTowards(transform.position, direction, Time.deltaTime * dashSpeed);
			yield return null;
		}
		if(player.playerState.hitEnemyShield){
			yield break;
		}
		yield return new WaitForSeconds(0.15f);
		player.playerState.canDash = true;
		if(targetStash.Count > 0){
			targetStashEmpty = false;
			yield break;
		} else {
			targetStashEmpty = true;
		}
		yield return new WaitForSeconds(0.6f);
		EndDashChain();
	}

	IEnumerator ShovedBack(){
		naginataControl.ClearBladeTrail();
		player.playerState.canMove = false;
		playerAnimator.SetTrigger("hitEnemyShield");
		playerAnimator.SetBool("grounded", true);
		for(float t = 0; t < 1f; t = t + Time.deltaTime * 2){
			transform.position -= (transform.forward * (1-t)) * 0.5f;
			yield return null;
		}
		EndDashChain();
		shovedBack = null;
	}


	

}
