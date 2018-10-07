using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

	public EnemyHealth enemyHealth;
	public SkinnedMeshRenderer enemyBody;

	[SerializeField] ParticlePooler chainKillParticlePooler;

	private Texture originalEmissiveTex, damageEmissiveTex = null;
	private bool flashingDMG = false;
	private Coroutine flashDamageRoutine;

	void Start(){
		GetComponent<ParticleSystem>().Play();
		originalEmissiveTex = enemyBody.material.GetTexture("_EmissionMap");
	}

	public void OnTriggerEnter(Collider collider){
		Player player = collider.gameObject.GetComponent<Player>();
		if(player != null){
			if(player.playerState.isLegendary){
				enemyHealth.TakeDamage(player.GetCurrentDamage());
				//StartCoroutine(FlashDamage(0.33f));
				chainKillParticlePooler.SpawnFromQueueAndPlay(null, transform.position, Vector3.zero);
			}
			else
				StartCoroutine(DelayedDamage(collider, player));
		}
		else {
			RuneStone edr = collider.gameObject.GetComponent<RuneStone>();
			if(edr != null){
				edr.WinOrLoose(false);
			}
			else 
				return;
		}
		
	}

	IEnumerator FlashDamage(float flashTime){
		flashingDMG = true;
		float multiplier = 1f / flashTime;
		enemyBody.material.SetTexture("_EmissionMap", null);
		enemyBody.material.SetColor("_EmissionColor", Color.red);
		while(flashTime > 0f){
			enemyBody.material.SetColor("_EmissionColor", Color.red * Mathf.LinearToGammaSpace(flashTime * multiplier));
			flashTime -= Time.deltaTime;
			yield return null;
		}
		enemyBody.material.SetTexture("_EmissionMap", originalEmissiveTex);
		enemyBody.material.SetColor("_EmissionColor", Color.red);
		flashingDMG = false;
		yield return null;
	}

	//because of unity's collision detection enemies were sometimes hit through their shields
	//delaying the damage calculation for a few milliseconds gives the invulnerability frames
	//time to kick in which solves the problem
	IEnumerator DelayedDamage(Collider collider, Player player){
		Vector3 closestPoint = collider.ClosestPoint(transform.position);
		yield return new WaitForSeconds(0.1f);
		if(enemyHealth.iFramesActive) yield return null;
		else {
			enemyHealth.takeDamagePooler.SpawnFromQueueAndPlay(transform, closestPoint, player.transform.position);
			enemyHealth.TakeDamage(player.GetCurrentDamage());
			player.playerState.CurrentCombo += 1;
			if(!flashingDMG) flashDamageRoutine = StartCoroutine(FlashDamage(0.33f));
			else {
				StopCoroutine(flashDamageRoutine);
				flashDamageRoutine = StartCoroutine(FlashDamage(0.33f));
			}
		}
	}
}
