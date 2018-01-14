using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Animator animator;
	BoxCollider playerCollider;
	SphereCollider playerColliderSphere;
	public PlayerState playerState;
	Vector3 playerColliderStartSize;
	float playerColliderStartRadius;

	public void Start(){
		animator = GetComponent<PlayerDash>().playerAnimator;
		playerCollider = GetComponent<BoxCollider>();
		playerColliderStartSize = playerCollider.size;
		playerColliderSphere = GetComponent<SphereCollider>();
		playerColliderStartRadius = playerColliderSphere.radius;
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
		playerColliderSphere.radius = playerColliderStartRadius;
	}
	public void SetColliderWidth(float width){
		Vector3 newSize = playerCollider.size;
		newSize.x = width;
        playerCollider.size = newSize;
		playerColliderSphere.radius = width;
	}

}
