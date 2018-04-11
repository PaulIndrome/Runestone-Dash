using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

	public EnemyHealth enemyHealth;
	public SkinnedMeshRenderer enemyBody;

	void Start(){
		GetComponent<ParticleSystem>().Play();
	}

	public void OnTriggerEnter(Collider collider){
		Player player = collider.gameObject.GetComponent<Player>();
		if(player != null){
			StartCoroutine(DelayedDamage(collider, player));
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

	IEnumerator DelayedDamage(Collider collider, Player player){
		Vector3 closestPoint = collider.ClosestPoint(transform.position);
		yield return new WaitForSeconds(0.1f);
		if(enemyHealth.iFramesActive) yield return null;
		else {
			enemyHealth.TakeDamage(player.GetCurrentDamage());
			enemyHealth.takeDamagePooler.SpawnFromQueueAndPlay(transform, closestPoint, player.transform.position);
			StartCoroutine(FlashDamage(0.33f));
		}
	}
}
