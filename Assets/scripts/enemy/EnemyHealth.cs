using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float maxHealth;
	public float currentHealth;
	public bool hasShield = false;
	public bool iFramesActive = false;
	public GameObject healthBarPrefab;
	public Transform healthBarPosition;
	private Enemy enemy;

	private HealthBar healthBar;
	private AudioSource enemyAudioSource;

	[Header("enemyHealth Audio")]
	public AudioEvent audioEnemyDeath;
	public AudioEvent audioTakeDamage;

	[Header("enemyHealth ParticleSystems")]	
	public ParticleSystem killedPS;
	public RandomParticleSystemSpawner takeDamagePS;

	public void Start(){
		enemyAudioSource = GetComponent<AudioSource>();
		enemy = GetComponent<Enemy>();
	}

	public void SetupHealth(EnemyType enemyType){
		maxHealth = enemyType.health;
		currentHealth = maxHealth;
	}
	public void TakeDamage(float damage){
		if(iFramesActive || currentHealth <= 0) return;

		currentHealth -= damage;

		if(currentHealth <= 0){
			enemy.StopMoving();
			StartCoroutine(HasBeenKilled());
			healthBar.SetRedBarTo(0f);
			return;
		}

		audioTakeDamage.PlayOneShot(enemyAudioSource);
		healthBar.SetRedBarTo(currentHealth / maxHealth);
	}

	public void HealByAmount(float amount){
		if(currentHealth == maxHealth) return;
		currentHealth += amount;
		healthBar.SetRedBarTo(currentHealth / maxHealth);
	}

	public void TriggerIFrames(float iFrameTime){
		StartCoroutine(TriggerInvulnerabilityFrames(iFrameTime));
	}

	public void SetupHealthBar(RectTransform healthBarCanvas){
		GameObject healthBarObject = Instantiate(healthBarPrefab);
		healthBar = healthBarObject.GetComponent<HealthBar>();
		healthBar.SetTarget(healthBarPosition);
		healthBarObject.transform.SetParent(healthBarCanvas);
	}

	public IEnumerator HasBeenKilled(){
		killedPS.Play();
		GetComponentInParent<EnemySpawn>().RemoveEnemy(enemy);
		foreach(Collider bc in GetComponentsInChildren<Collider>()){
			bc.enabled = false;
		}
		audioEnemyDeath.PlayOneShot(enemyAudioSource);
		while(killedPS.isPlaying){
			transform.position = Vector3.MoveTowards(transform.position, transform.position - Vector3.up, Time.deltaTime*2f);
			yield return null;
		}
		Destroy(healthBar.gameObject);
		Destroy(gameObject);
		yield return null;
	}

	IEnumerator TriggerInvulnerabilityFrames(float iFrameTime){
		iFramesActive = true;
		yield return new WaitForSeconds(iFrameTime);
		iFramesActive = false;
		yield return null;
	}

}
