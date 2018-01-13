using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Animator animator;
	PlayerDash playerDash;
	PlayerFollow playerFollow;
	BoxCollider playerCollider;
	public PlayerState playerState;
	Vector3 playerColliderStartSize;

	public void Start(){
		animator = GetComponent<PlayerDash>().playerAnimator;
		playerDash = GetComponent<PlayerDash>();
		playerFollow = GetComponent<PlayerFollow>();
		playerCollider = GetComponent<BoxCollider>();
		playerColliderStartSize = playerCollider.size;
	}
	public float GetCurrentDamage(){
		return playerState.currentDamage;
	}

	public void SetCurrentDamage(float damage){
		playerState.currentDamage = damage;
	}

	public void SetAnimatorTrigger(string trigger){
		animator.SetTrigger(trigger);
	}

	public void SetAnimatorBool(string boolName, bool setTo){
		animator.SetBool(boolName, setTo);
	}

	public void SetFreeAgain(){
		playerState.canDash = true;
		playerState.hitEnemyShield = false;
	}

	public void ResetColliderWidth(){
		playerCollider.size = playerColliderStartSize;
	}
	public void SetColliderWidth(float width){
		Vector3 newSize = playerCollider.size;
		newSize.x = width;
        playerCollider.size = newSize;
	}

}
