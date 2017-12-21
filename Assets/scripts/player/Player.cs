using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Animator animator;
	PlayerDash playerDash;
	PlayerFollow playerFollow;
	public PlayerState playerState;


	public void Start(){
		animator = GetComponent<Animator>();
		playerDash = GetComponent<PlayerDash>();
		playerFollow = GetComponent<PlayerFollow>();
	}
	public float GetCurrentDamage(){
		return playerState.currentDamage;
	}

	public void SetCurrentDamage(float damage){
		playerState.currentDamage += damage;
	}

	public void TriggerAnimator(string trigger){
		animator.SetTrigger(trigger);
	}

	public void SetFreeAgain(){
		playerFollow.allowedToFollow = true;
		playerState.canDash = true;
		playerState.hitEnemyShield = false;
	}

}
