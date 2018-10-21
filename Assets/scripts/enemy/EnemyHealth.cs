using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float MaxHealth{
		get {
			return maxHealth;
		}
		private set{
			maxHealth = value;
		}
	}
	public float CurrentHealth {
		get {
			return currentHealth;
		}
		private set{
			currentHealth = value;
		}
	}

	public bool FullHealth{
		get {
			return currentHealth >= maxHealth;
		}
	}

	public bool hasShield = false;
	public bool iFramesActive = false;
	public HealthBar healthBarPrefab;
	public Transform healthBarPosition;
	private Enemy enemy;


	private float currentHealth, maxHealth;
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
		MaxHealth = enemyType.health;
		CurrentHealth = MaxHealth;
	}

	public void TakeDamage(float damage){
		if(iFramesActive || CurrentHealth <= 0) return;

		CurrentHealth -= damage;
		audioTakeDamage.PlayOneShot(enemyAudioSource);
		healthBar.SetBarTo(CurrentHealth / MaxHealth);

		if(CurrentHealth <= 0){
			//the enemy has been killed, CUE DEATH
			enemy.StopMoving();
			StartCoroutine(HasBeenKilled());
			healthBar.SetBarTo(0f);
			return;
		}
	}

	public void HealByAmount(float amount){
		if(CurrentHealth == MaxHealth) return;
		CurrentHealth += amount;
		//the currentHealth should never exceed the maximum health amount set by the EnemyType
		CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
		healthBar.SetBarTo(CurrentHealth / MaxHealth);
	}

	//unused method that might be important later
	public void HealToFull(){
		if(CurrentHealth == MaxHealth) return;
		CurrentHealth = MaxHealth;
		healthBar.SetBarTo(CurrentHealth / MaxHealth);
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
	public HealthBar SetupHealthBar(EnemyHealthBarsHandler healthBarHandler){
		healthBar = Instantiate(healthBarPrefab);

		if(enemy.enemyType.isBossType) healthBar.ApplyBossColors();

		healthBar.SetTarget(healthBarPosition);
		healthBar.transform.SetParent(healthBarHandler.transform);
		healthBarHandler.RegisterBar(healthBar);
		return healthBar;
	}

	//currently, killed enemies sink into the ground. This should definitely change some time
	public IEnumerator HasBeenKilled(){
		if(enemy.enemyType.ContainsType(typeof(EnemyHealRadiusEffect))){
			EnemyHealRadius healRadius = enemy.GetComponent<EnemyHealRadius>();
			foreach(EnemyMovement em in healRadius.beingHealedByThis){
				em.RemoveHealer();
				Debug.Log("Removed healer of " + em.gameObject.name);
			}
			healRadius.Deactivate();
		}

		killedPS.Play();
		GetComponentInParent<EnemySpawn>().RemoveEnemy(enemy);
		enemy.StopAllPoolableParticles();
		
		if(hasShield) enemyShieldCollision.DestroyShieldObject();
		
		foreach(Collider bc in GetComponentsInChildren<Collider>()){
			bc.enabled = false;
		}

		healthBar.Unregister();
		audioEnemyDeath.PlayOneShot(enemyAudioSource);
		enemy.GetEnemyAnimator().SetBool("isDead", true);
		yield return null;
	}

	public void EraseEnemyObject(){
		Destroy(healthBar.gameObject);
		StartCoroutine(DespawnEnemy());
	}

	IEnumerator DespawnEnemy(){
		while(killedPS.isPlaying){
			transform.position = Vector3.MoveTowards(transform.position, transform.position - Vector3.up, Time.deltaTime*2f);
			yield return null;
		}
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
