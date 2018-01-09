using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

	public EnemyHealth enemyHealth;
	public void OnTriggerEnter(Collider collision){
		Player player = collision.gameObject.GetComponent<Player>();
		if(player != null && !enemyHealth.hasShield){
			enemyHealth.TakeDamage(player.GetCurrentDamage());
		}
		else {
			EnemyDestroysRunestone edr = collision.gameObject.GetComponent<EnemyDestroysRunestone>();
			if(edr != null)
				edr.WinOrLoose(false);
			else 
				return;
		}
		
	}
}
