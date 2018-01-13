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


	public void Activate(){
		GetComponent<BoxCollider>().enabled = true;
		foreach(Transform g in transform){
			g.gameObject.SetActive(true);
		}
		shieldAnimator = GetComponent<Animator>();
	}

	public void OnCollisionEnter(Collision col){
		Player player = col.gameObject.GetComponent<Player>();

		if(player != null){
			playerPosAtColLocal = transform.InverseTransformPoint(player.transform.position);
			//Debug.Log("PlayerPosAtColLocal.z = " + playerPosAtColLocal.z);
			//Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), player.transform.position, Quaternion.identity);
			//Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), transform.position, Quaternion.identity);

			switch(playerPosAtColLocal.z < -0.5f){
				case true:
					//Debug.Log("Shield hit from back");
					playerDestroyShield.SpawnRandomAndPlay(transform, playerPosAtColLocal, player.transform.position);
					DestroyShield();
					break;
				case false: 
					//Debug.Log("Shield hit from front or side");
					shieldAnimator.SetTrigger("playerHitShield");
					playerState.hitEnemyShield = true;
					player.transform.position -= player.transform.forward * 3;
					player.SetAnimatorTrigger("hitEnemyShield");
					player.SetAnimatorBool("isDashing", false);
					playerHitShield.SpawnRandomAndPlay(transform, playerPosAtColLocal, player.transform.position);
					//Invoke("DestroyShield", 0.1f);
					break;
			}
			
		} 
	}

	public void DestroyShield(){
		GetComponentInParent<EnemyHealth>().hasShield = false;
		foreach(Transform g in transform){
			g.gameObject.SetActive(false);
		}
		GetComponent<BoxCollider>().enabled = false;
	}



}
