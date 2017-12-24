using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public float health;
	public ParticleSystem killedPS;
	public EnemyCurvePath enemyCurvePath;
	public EnemyType enemyType;
	public GameObject meshAndCollider;

	public void Start(){
		transform.localScale *= enemyType.scaleOfEnemy;
		transform.LookAt(Vector3.zero, Vector3.up);

		health = enemyType.health;

		foreach(EnemyEffect e in enemyType.enemyEffects){
			e.Apply(this);
		}

		StartCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

	public void takeDamage(float damage){
		if(health < 0) return;
		health -= damage;
		if(health <= 0){
			StopCoroutine(enemyCurvePath.MoveTowardsTarget(this));
			StartCoroutine(HasBeenKilled());
		}
	}

	public IEnumerator HasBeenKilled(){
		killedPS.Play();
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

}
