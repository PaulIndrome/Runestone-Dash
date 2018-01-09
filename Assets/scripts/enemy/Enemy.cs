using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public EnemyCurvePath enemyCurvePath;
	public EnemyType enemyType;
	private EnemyHealth enemyHealth;
	public GameObject meshAndCollider;

	public void Start(){
		transform.localScale *= enemyType.scaleOfEnemy;
		transform.LookAt(Vector3.zero, Vector3.up);
		enemyHealth = GetComponent<EnemyHealth>();
	}

	public void StopMoving(){
		StopCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

	public void SetupEnemy(){
		SetupEnemy(enemyType, enemyCurvePath);
	}

	public void SetupEnemy(EnemyType et, EnemyCurvePath ecp){
		enemyType = et;
		enemyCurvePath = ecp;

		enemyHealth = GetComponent<EnemyHealth>();
		enemyHealth.SetupHealth(enemyType);

		foreach(EnemyEffect e in enemyType.enemyEffects){
			e.Apply(this);
		}

		StartCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

	public void SetupHealthBar(RectTransform healthBarCanvas){
		enemyHealth.SetupHealthBar(healthBarCanvas);
	}

	public EnemyHealth GetEnemyHealth(){
		return enemyHealth;
	}

}
