using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	Rigidbody rb;
	RuneStone runeStone;
	EnemyHealRadius healRadius;
	public float moveSpeed, attackDistance, findHealerDistance;
	public AnimationCurve curvePath;
	public EnemyType enemyType;

	// Use this for initialization
	void Start () {
		SetupEnemyMovement(FindObjectOfType<RuneStone>());
		rb = GetComponent<Rigidbody>();
	}

	public void SetupEnemyMovement(RuneStone runeStone){
		this.runeStone = runeStone;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 directionOfRunestone = runeStone.transform.position - transform.position;
		float distance = directionOfRunestone.magnitude;
		if(distance <= attackDistance) // stop moving, start attacking


		
		rb.MovePosition(transform.position + (directionOfRunestone * Time.deltaTime * moveSpeed));
	}

	public bool SetHealer(EnemyHealRadius enemyHealRadius, float distance){
		if(enemyHealRadius == null ^ distance < Vector3.Distance(transform.position, enemyHealRadius.transform.position)){ // XOR ?
			healRadius = enemyHealRadius;
			return true;
		} else {
			return false;
		}
	}

	/*
	state machine hierarchy:
	
	0. if you're near the runestone, attack it
	
	1. move along curve path towards runestone

	2.1 if you can heal someone, try to stay near them
	2.2 if someone can heal you, try to stay near them

	3. if you have a shield, try to keep yourself turned towards the player 

	4. if you're an aggressor and the player is in range, charge them
	 */
}
