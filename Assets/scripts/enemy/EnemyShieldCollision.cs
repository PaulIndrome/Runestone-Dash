using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldCollision : MonoBehaviour {

	public PlayerState playerState;
	public EnemyHealth enemyHealth;
	public Animator shieldAnimator;
	private RaycastHit raycastHit;


	public void Activate(){
		GetComponent<BoxCollider>().enabled = true;
		foreach(Transform g in transform){
			g.gameObject.SetActive(true);
		}
		shieldAnimator = GetComponent<Animator>();
	}

	public void OnCollisionEnter(Collision col){
		Player player = col.gameObject.GetComponent<Player>();
		
		Vector3 firstContactPointLocal = transform.InverseTransformPoint(col.contacts[0].point);

		if(player != null){
			switch(firstContactPointLocal.z >= 0){
				case true: 
					Debug.Log("Shield hit from front or side");
					shieldAnimator.SetTrigger("playerHitShield");
					playerState.hitEnemyShield = true;
					player.transform.position -= player.transform.forward * 3;
					player.SetAnimatorTrigger("hitEnemyShield");
					player.SetAnimatorBool("isDashing", false);
					//Invoke("DestroyShield", 0.1f);
					break;
				case false:
					Debug.Log("Shield hit from back");
					DestroyShield();
					break;
			}
			
		} 
	}

	public void DestroyShield(){
		GetComponentInParent<EnemyHealth>().hasShield = false;
		gameObject.SetActive(false);
	}



}
