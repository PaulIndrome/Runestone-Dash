using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldCollision : MonoBehaviour {

	public PlayerState playerState;
	public void Activate(){
		GetComponent<BoxCollider>().enabled = true;
		foreach(Transform g in transform){
			g.gameObject.SetActive(true);
		}
	}
	public void OnTriggerEnter(Collider col){
		Player player = col.GetComponent<Player>();
		if(player != null){
			playerState.hitEnemyShield = true;
			player.transform.position -= player.transform.forward;
			player.TriggerAnimator("hitEnemyShield");
			Invoke("DestroyShield", 0.1f);
		}
	}

	public void DestroyShield(){
		GetComponentInParent<EnemyHealth>().hasShield = false;
		gameObject.SetActive(false);
	}
}
