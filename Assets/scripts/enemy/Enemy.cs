using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public EnemyCurvePath enemyCurvePath;
	public EnemyType enemyType;
	private EnemyHealth enemyHealth;
	[SerializeField] Animator enemyAnimator	;
	private EnemyShieldCollision enemyShieldCollision;
	public GameObject meshAndCollider;

	public bool canMove = true;
	public bool CanMove{
		get{ return canMove; }
	}


	void Awake(){
		enemyHealth = GetComponent<EnemyHealth>();
	}

	public void Start(){
		transform.localScale *= enemyType.scaleOfEnemy;
		transform.LookAt(Vector3.zero, Vector3.up);
	}

	public void StopMoving(){
		StopCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

	public void SetupEnemy(RectTransform healthBarCanvas){
		SetupEnemy(enemyType, enemyCurvePath);
		SetupBars(healthBarCanvas);
	}

	public void SetupEnemy(EnemyType et, EnemyCurvePath ecp){
		enemyType = et;
		enemyCurvePath = ecp;

		enemyHealth = GetComponent<EnemyHealth>();
		enemyHealth.SetupHealth(enemyType);
		enemyShieldCollision = GetComponentInChildren<EnemyShieldCollision>();

		foreach(EnemyEffect e in enemyType.enemyEffects){
			e.Apply(this);
		}

		canMove = true;
		StartCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

	public HealthBar SetupBars(RectTransform healthBarCanvas){
		HealthBar newHealthBar = enemyHealth.SetupHealthBar(healthBarCanvas);
		if(enemyHealth.hasShield) enemyShieldCollision.SetupDurabilityBar(healthBarCanvas);
		return newHealthBar;
	}

	public EnemyHealth GetEnemyHealth(){
		return enemyHealth;
	}

	public Animator GetEnemyAnimator(){
		return enemyAnimator;
	}

	public EnemyShieldCollision GetEnemyShieldCollision(){
		return enemyShieldCollision;
	}

	void OnDestroy(){
		PoolableParticle[] poolables = GetComponentsInChildren<PoolableParticle>();
		if(poolables.Length > 0){ 
			foreach(PoolableParticle p in poolables){
				p.ReturnParticle();
			}
		}
	}



}
