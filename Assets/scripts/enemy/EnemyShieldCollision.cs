using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldCollision : MonoBehaviour {

	public void OnTriggerEnter(Collider col){
		Player player = col.GetComponent<Player>();
		if(player != null){
			player.GetComponent<PlayerFollow>().allowedToFollow = false;
			player.TriggerAnimator("hitEnemyShield");
			gameObject.SetActive(false);
		}
	}
}
