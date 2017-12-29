using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public float health;
	public bool hasShield = false;
	public ParticleSystem killedPS;
	public EnemyCurvePath enemyCurvePath;
	public EnemyType enemyType;
	public GameObject meshAndCollider;

	public void Start(){
		transform.localScale *= enemyType.scaleOfEnemy;
		transform.LookAt(Vector3.zero, Vector3.up);
		health = health==0 ? 1 : health;
	}

	public void TakeDamage(float damage){
		if(health < 0 || hasShield) return;
		health -= damage;
		if(health <= 0){
			StopCoroutine(enemyCurvePath.MoveTowardsTarget(this));
			StartCoroutine(HasBeenKilled());
		}
	}

	public void HealToFullHealth(){
		health = enemyType.health;
	}

	public IEnumerator HasBeenKilled(){
		killedPS.Play();
		GetComponentInParent<EnemySpawn>().RemoveEnemy(this);
		foreach(Collider bc in GetComponentsInChildren<Collider>()){
			bc.enabled = false;
		}
		while(killedPS.isPlaying){
			transform.position = Vector3.MoveTowards(transform.position, transform.position - Vector3.up, Time.deltaTime*2f);
			yield return null;
		}
		Destroy(gameObject);
		yield return null;
	}

	public void SetupEnemy(){
		SetupEnemy(enemyType, enemyCurvePath);
	}

	public void SetupEnemy(EnemyType et, EnemyCurvePath ecp){
		enemyType = et;
		enemyCurvePath = ecp;

		health = enemyType.health;

		foreach(EnemyEffect e in enemyType.enemyEffects){
			e.Apply(this);
		}

		StartCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

}
