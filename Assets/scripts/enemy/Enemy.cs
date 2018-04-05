using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public EnemyCurvePath enemyCurvePath;
	public EnemyType enemyType;
	private EnemyHealth enemyHealth;
	private EnemyAnimation enemyAnimation;
	private EnemyShieldCollision enemyShieldCollision;
	public GameObject meshAndCollider;

	private bool canMove;
	public bool CanMove{
		get{ return canMove; }
	}

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
		enemyAnimation = GetComponent<EnemyAnimation>();
		enemyShieldCollision = GetComponentInChildren<EnemyShieldCollision>();

		foreach(EnemyEffect e in enemyType.enemyEffects){
			e.Apply(this);
		}

		canMove = true;
		StartCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

	public void SetupHealthBar(RectTransform healthBarCanvas){
		enemyHealth.SetupHealthBar(healthBarCanvas);
	}

	public EnemyHealth GetEnemyHealth(){
		return enemyHealth;
	}

	public EnemyAnimation GetEnemyAnimation(){
		return enemyAnimation;
	}

	public EnemyShieldCollision GetEnemyShieldCollision(){
		return enemyShieldCollision;
	}

}
