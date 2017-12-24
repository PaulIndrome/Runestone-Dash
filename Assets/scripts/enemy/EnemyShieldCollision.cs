using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldCollision : MonoBehaviour {

	public void Activate(){
		GetComponent<BoxCollider>().enabled = true;
		foreach(Transform g in transform){
			g.gameObject.SetActive(true);
		}
	}
	public void OnTriggerEnter(Collider col){
		Debug.Log(col.gameObject.name + " collided with shield");
		Player player = col.GetComponent<Player>();
		if(player != null){
			player.GetComponent<PlayerFollow>().allowedToFollow = false;
			player.transform.position -= player.transform.forward;
			player.TriggerAnimator("hitEnemyShield");
			gameObject.SetActive(false);
		}
	}
}
