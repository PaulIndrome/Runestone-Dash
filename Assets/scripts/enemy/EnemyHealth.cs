using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float maxHealth;
	public float currentHealth;
	public bool hasShield = false;
	public ParticleSystem killedPS;
	public GameObject healthBarPrefab;
	public Transform healthBarPosition;
	private Enemy enemy;

	private HealthBar healthBar;


	public void Start(){
		enemy = GetComponent<Enemy>();
	}

	public void SetupHealth(EnemyType enemyType){
		maxHealth = enemyType.health;
		currentHealth = maxHealth;
	}
	public void TakeDamage(float damage){
		if(currentHealth <= 0) return;

		currentHealth -= damage;

		if(currentHealth <= 0){
			enemy.StopMoving();
			StartCoroutine(HasBeenKilled());
			healthBar.SetRedBarTo(0f);
			return;
		}

		healthBar.SetRedBarTo(currentHealth / maxHealth);
	}

	public void HealToFullHealth(){
		currentHealth = maxHealth;
		healthBar.SetRedBarTo(currentHealth / maxHealth);
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
		while(killedPS.isPlaying){
			transform.position = Vector3.MoveTowards(transform.position, transform.position - Vector3.up, Time.deltaTime*2f);
			yield return null;
		}
		Destroy(healthBar.gameObject);
		Destroy(gameObject);
		yield return null;
	}

}
