using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

	public EnemyHealth enemyHealth;

	public void OnTriggerEnter(Collider collider){
		Player player = collider.gameObject.GetComponent<Player>();
		if(player != null && !enemyHealth.iFramesActive){
			enemyHealth.TakeDamage(player.GetCurrentDamage());
			enemyHealth.takeDamagePS.SpawnRandomAndPlay(enemyHealth.transform, collider.ClosestPoint(transform.position), player.transform.position);
			//Debug.Break();
		}
		else {
			EnemyDestroysRunestone edr = collider.gameObject.GetComponent<EnemyDestroysRunestone>();
			if(edr != null)
				edr.WinOrLoose(false);
			else 
				return;
		}
		
	}
}
