using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

	public EnemyHealth enemyHealth;
	public MeshRenderer enemyBody;

	void Start(){
		GetComponent<ParticleSystem>().Play();
	}

	public void OnTriggerEnter(Collider collider){
		Player player = collider.gameObject.GetComponent<Player>();
		if(player != null && !enemyHealth.iFramesActive){
			enemyHealth.TakeDamage(player.GetCurrentDamage());
			enemyHealth.takeDamagePS.SpawnRandomAndPlay(enemyHealth.transform, collider.ClosestPoint(transform.position), player.transform.position);
			StartCoroutine(FlashDamage(0.35f));
		}
		else {
			EnemyDestroysRunestone edr = collider.gameObject.GetComponent<EnemyDestroysRunestone>();
			if(edr != null)
				edr.WinOrLoose(false);
			else 
				return;
		}
		
	}

	IEnumerator FlashDamage(float flashTime){
		float multiplier = 0.5f / flashTime;
		enemyBody.material.SetColor("_EmissionColor", Color.red * Mathf.LinearToGammaSpace(0.5f));
		while(flashTime >= 0f){
			enemyBody.material.SetColor("_EmissionColor", Color.red * Mathf.LinearToGammaSpace(flashTime * multiplier));
			flashTime -= Time.deltaTime;
			yield return null;
		}
		enemyBody.material.SetColor("_EmissionColor", Color.black);
		yield return null;
	}
}
