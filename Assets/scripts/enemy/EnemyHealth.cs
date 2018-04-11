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
	private EnemyShieldCollision enemyShieldCollision;

	[Header("enemyHealth Audio")]
	public AudioEvent audioEnemyDeath;
	public AudioEvent audioTakeDamage;

	[Header("enemyHealth ParticleSystems")]	
	public ParticleSystem killedPS;
	public ParticlePooler takeDamagePooler;

	Coroutine iFrameTimer;

	public void Start(){
		enemyAudioSource = GetComponent<AudioSource>();
		enemy = GetComponent<Enemy>();
		enemyShieldCollision = GetComponentInChildren<EnemyShieldCollision>();
	}

	public void SetupHealth(EnemyType enemyType){
		maxHealth = enemyType.health;
		currentHealth = maxHealth;
	}
	public void TakeDamage(float damage){
		if(iFramesActive || currentHealth <= 0) return;

		currentHealth -= damage;
		audioTakeDamage.PlayOneShot(enemyAudioSource);
		healthBar.SetBarTo(currentHealth / maxHealth);

		if(currentHealth <= 0){
			enemy.StopMoving();
			StartCoroutine(HasBeenKilled());
			healthBar.SetBarTo(0f);
			return;
		}
	}

	public void HealByAmount(float amount){
		if(currentHealth == maxHealth) return;
		currentHealth += amount;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		healthBar.SetBarTo(currentHealth / maxHealth);
	}

	public void HealToFull(){
		if(currentHealth == maxHealth) return;
		currentHealth = maxHealth;
		healthBar.SetBarTo(currentHealth / maxHealth);
	}

	public void TriggerIFrames(float iFrameTime){
		if(iFrameTimer != null) {
			StopCoroutine(iFrameTimer);
		}
		iFrameTimer = StartCoroutine(TriggerInvulnerabilityFrames(iFrameTime));
	}

	public HealthBar SetupHealthBar(RectTransform healthBarCanvas){
		GameObject healthBarObject = Instantiate(healthBarPrefab);
		healthBar = healthBarObject.GetComponent<HealthBar>();
		healthBar.SetTarget(healthBarPosition);
		healthBarObject.transform.SetParent(healthBarCanvas);
		return healthBar;
	}

	public IEnumerator HasBeenKilled(){
		killedPS.Play();
		GetComponentInParent<EnemySpawn>().RemoveEnemy(enemy);
		
		if(hasShield) enemyShieldCollision.DestroyShieldObject();
		
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
		iFrameTimer = null;
	}

	

}
