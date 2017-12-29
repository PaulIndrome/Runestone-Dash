using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

	public Enemy enemy;
	public void OnTriggerEnter(Collider collision){
		Player player = collision.gameObject.GetComponent<Player>();
		if(player != null && !enemy.hasShield){
			enemy.TakeDamage(player.GetCurrentDamage());
		}
		else if(collision.gameObject.GetComponent<EnemyDestroysRunestone>() != null)
			collision.gameObject.GetComponent<EnemyDestroysRunestone>().RuneStoneDestroyed(false);
		else
			return;
	}
}
