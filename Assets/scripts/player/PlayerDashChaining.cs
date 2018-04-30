using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this script governs all dashing
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
		//while there is a reason and ability to dash, keep doing so
		if(targetStash.Count > 0 && player.playerState.canDash && player.playerState.canMove){
			//the entire animation system is rather convoluted... I am happy it works
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

	//this is an AnimationEvent callback function triggered by the standing-back-up
	//animation reaching a certain point at which control is given back to the player
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

		//if the chainkill script events are in effect, the dash is no longer clamped to a maximum range
		if(!player.playerState.isLegendary){
			direction = (transform.position + (target - transform.position).normalized * dashRadius) * 1.01f;
		} else {
			direction = transform.position + (target - transform.position);
		}

		//this is a precaution against certain rare flukes in which the player character learned to fly...
		direction.y = 0;
		
		audioPlayerKickoff.PlayOneShot(playerAudioSource);
		audioPlayerDash.PlayOneShot(playerAudioSource);
		
		playerDashKickoffPooled.SpawnFromQueueAndPlay(null, transform.position, direction);
		naginataControl.StartBladeTrail();

		//move towards the current dash target as long as it has not been reached
		//and no enemy shield has been hit
		while(Vector3.Distance(transform.position, direction) > 0.05f && !player.playerState.hitEnemyShield){
			transform.position = Vector3.MoveTowards(transform.position, direction, Time.deltaTime * dashSpeed);
			yield return null;
		}
		if(player.playerState.hitEnemyShield){
			//if an enemy shield has been hit, everything related to dashing is stopped
			//because the PlayerShovedBack method and ShovedBack coroutine take over
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
		//wreathe control from the player, canDash is currently still false if a shield has been hit
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
