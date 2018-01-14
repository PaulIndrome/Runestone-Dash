using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldCollision : MonoBehaviour {

	public PlayerState playerState;
	public EnemyHealth enemyHealth;
	public Animator shieldAnimator;
	private RaycastHit raycastHit;

	private Vector3 firstContactPointWorld;
	private Vector3 playerPosAtColLocal;

	public RandomParticleSystemSpawner playerHitShield;
	public RandomParticleSystemSpawner playerDestroyShield;

	public EnemyShieldPiece[] shieldPieces;

	
	private AudioSource enemyAudioSource;
	[Header("Audio Source and Events")]
	public AudioEvent audioShieldDestroyed;
	public AudioEvent audioShieldHit;




	public void Activate(){
		enemyAudioSource = GetComponentInParent<AudioSource>();

		GetComponent<BoxCollider>().enabled = true;
		foreach(EnemyShieldPiece esp in shieldPieces){
			esp.gameObject.SetActive(true);
		}
		shieldAnimator = GetComponent<Animator>();
	}

	public void OnCollisionEnter(Collision col){
		Player player = col.gameObject.GetComponent<Player>();

		if(player != null){
			enemyHealth.TriggerIFrames(0.5f);
			if(!playerState.isDashing){
				playerHitShield.SpawnRandomAndPlay(transform, col.contacts[0].point, player.transform.position);
				ShovePlayerBack(player);
				return;
			}
			//if we've made it this far, the player is dashing ... get it?
			playerPosAtColLocal = transform.InverseTransformPoint(player.transform.position);
			switch(playerPosAtColLocal.z < -0.25f){
				case true:
					PlayerDestroysShield();
					break;
				case false: 
					playerHitShield.SpawnRandomAndPlay(transform, col.contacts[0].point, player.transform.position);
					ShovePlayerBack(player);
					break;
			}
			
		} 
	}

	public void PlayerDestroysShield(){
		//Debug.Log("Shield hit from back");
		shieldAnimator.SetTrigger("shieldDestroyed");
		playerDestroyShield.SpawnRandomAndPlay(transform, transform.position, transform.forward);
		audioShieldDestroyed.PlayOneShot(enemyAudioSource);
		DestroyShield();
	}

	public void ShovePlayerBack(Player player){
		//Debug.Log("Shield hit from front or side");
		shieldAnimator.SetTrigger("playerHitShield");
		playerState.hitEnemyShield = true;
		player.transform.position -= player.transform.forward * 3;
		player.SetAnimatorTrigger("hitEnemyShield");
		audioShieldHit.PlayOneShot(enemyAudioSource);
		player.SetAnimatorBool("isDashing", false);
		//Invoke("DestroyShield", 0.1f);
	}

	public void DestroyShield(){
		GetComponentInParent<EnemyHealth>().hasShield = false;
		foreach(EnemyShieldPiece esp in shieldPieces){
			esp.StartFadeOut(5f);
		}
		GetComponent<BoxCollider>().enabled = false;
	}




}
