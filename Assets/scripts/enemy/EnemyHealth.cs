using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float maxHealth;
	public float currentHealth;
	public bool hasShield = false;
	public bool iFramesActive = false;
	public HealthBar healthBarPrefab;
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
			//the enemy has been killed, CUE DEATH
			enemy.StopMoving();
			StartCoroutine(HasBeenKilled());
			healthBar.SetBarTo(0f);
			return;
		}
	}

	public void HealByAmount(float amount){
		if(currentHealth == maxHealth) return;
		currentHealth += amount;
		//the currentHealth should never exceed the maximum health amount set by the EnemyType
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		healthBar.SetBarTo(currentHealth / maxHealth);
	}

	//unused method that might be important later
	public void HealToFull(){
		if(currentHealth == maxHealth) return;
		currentHealth = maxHealth;
		healthBar.SetBarTo(currentHealth / maxHealth);
	}

	//if the player hits the shield first we don't want the Enemy to take damage 
	//because we can logically assume the shield was hit from the front
	public void TriggerIFrames(float iFrameTime){
		if(iFrameTimer != null) {
			StopCoroutine(iFrameTimer);
		}
		iFrameTimer = StartCoroutine(TriggerInvulnerabilityFrames(iFrameTime));
	}

	//the Enemy Prefab contains an empty GameObject (healthBarPosition) that acts as an
	//anchor for the healthBar to follow
	public HealthBar SetupHealthBar(RectTransform healthBarCanvas){
		healthBar = Instantiate(healthBarPrefab);
		healthBar.SetTarget(healthBarPosition);
		healthBar.transform.SetParent(healthBarCanvas);
		return healthBar;
	}

	//currently, killed enemies sink into the ground. This should definitely change some time
	public IEnumerator HasBeenKilled(){
		killedPS.Play();
		GetComponentInParent<EnemySpawn>().RemoveEnemy(enemy);
		enemy.StopAllPoolableParticles();
		
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
