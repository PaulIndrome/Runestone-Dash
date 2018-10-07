using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
