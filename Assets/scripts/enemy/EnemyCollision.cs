using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

	public EnemyHealth enemyHealth;
	public SkinnedMeshRenderer enemyBody;

	[SerializeField] ParticlePooler chainKillParticlePooler;

	void Start(){
		GetComponent<ParticleSystem>().Play();
	}

	public void OnTriggerEnter(Collider collider){
		Player player = collider.gameObject.GetComponent<Player>();
		if(player != null){
			if(player.playerState.isLegendary){
				enemyHealth.TakeDamage(player.GetCurrentDamage());
				StartCoroutine(FlashDamage(0.33f));
				chainKillParticlePooler.SpawnFromQueueAndPlay(null, transform.position, Vector3.zero);
			}
			else
				StartCoroutine(DelayedDamage(collider, player));
		}
		else {
			EnemyDestroysRunestone edr = collider.gameObject.GetComponent<EnemyDestroysRunestone>();
			if(edr != null){
				edr.WinOrLoose(false);
			}
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
			enemyHealth.takeDamagePooler.SpawnFromQueueAndPlay(transform, closestPoint, player.transform.position);
			enemyHealth.TakeDamage(player.GetCurrentDamage());
			player.playerState.CurrentCombo += 1;
			StartCoroutine(FlashDamage(0.33f));
		}
	}
}
