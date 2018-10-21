using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public EnemyCurvePath enemyCurvePath;
	public EnemyType enemyType;
	public EnemyMovement enemyMovement;
	public EnemyHealth enemyHealth {
		get;
		private set;
	}
	[SerializeField] private Animator enemyAnimator;
	private EnemyShieldCollision enemyShieldCollision;
	public GameObject meshAndCollider;
	[SerializeField] private bool useCurvePath = true;

	public bool canMove = true;
	public bool CanMove {
		get{ return canMove; }
	}

	void Awake(){
		enemyHealth = GetComponent<EnemyHealth>();
	}

	public void Start(){
		enemyAnimator.transform.localScale *= enemyType.scaleOfEnemy;
		transform.LookAt(Vector3.zero, Vector3.up);

	}

	public void StopMoving(){
		StopCoroutine(enemyCurvePath.MoveTowardsTarget(this));
	}

	// used when an Enemy prefab has been manually placed as a child of the EnemySpawn gameobject
	public void SetupEnemy(EnemyHealthBarsHandler healthBarHandler){
		SetupEnemy(enemyType, enemyCurvePath);
		SetupBars(healthBarHandler);
	}

	//set up all necessary attributes and apply the effects to yourself
	public void SetupEnemy(EnemyType et, EnemyCurvePath ecp){
		enemyType = et;
		enemyCurvePath = ecp;

		enemyHealth = GetComponent<EnemyHealth>();
		enemyMovement = GetComponent<EnemyMovement>();
		enemyHealth.SetupHealth(enemyType);

		enemyShieldCollision = GetComponentInChildren<EnemyShieldCollision>();
		if(!enemyType.ContainsType(typeof(EnemyShieldEffect))){
			enemyShieldCollision.gameObject.SetActive(false);
		}

		foreach(EnemyEffect e in enemyType.enemyEffects){
			e.Apply(this);
		}

		canMove = true;
		//interestingly, ScriptableObjects can contain the definition for coroutines and
		//run them on an external MonoBehaviour, which is what happens here
		if(useCurvePath) {
			if(enemyMovement != null) enemyMovement.enabled = false;
			StartCoroutine(enemyCurvePath.MoveTowardsTarget(this));
		}
		else if(enemyMovement != null){
			enemyMovement.enabled = true;
			enemyMovement.SetupEnemyMovement(GameObject.FindGameObjectWithTag("RuneStone").GetComponent<RuneStone>(), enemyType.approachTime, enemyAnimator);
		}
		else 
			Debug.Log("no movement capabilities set on " + gameObject.name, this.gameObject);

	}

	public HealthBar SetupBars(EnemyHealthBarsHandler healthBarHandler){
		HealthBar newHealthBar = enemyHealth.SetupHealthBar(healthBarHandler);
		if(enemyHealth.hasShield) enemyShieldCollision.SetupDurabilityBar(healthBarHandler);
		return newHealthBar;
	}

	public Animator GetEnemyAnimator(){
		return enemyAnimator;
	}

	public EnemyShieldCollision GetEnemyShieldCollision(){
		return enemyShieldCollision;
	}

	public void StopAllPoolableParticles(){
		PoolableParticle[] poolables = GetComponentsInChildren<PoolableParticle>();
		if(poolables.Length > 0){ 
			foreach(PoolableParticle p in poolables){
				p.Stop();
			}
		}
	}

	//make sure to not take any PoolableParticle within your hierarchy to your grave
	//things get hairy when scripts try to access destroyed gameobjects
	void OnDestroy(){
		StopAllCoroutines();
	}

}
