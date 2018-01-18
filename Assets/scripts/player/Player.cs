﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Animator animator;
	SphereCollider playerColliderSphere;
	public PlayerState playerState;
	Vector3 playerColliderStartSize;
	float playerColliderStartRadius;
	private PlayerDash playerDash;

	public void Start(){
		animator = GetComponent<Animator>();
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

	public void ResetColliderWidth(){
		playerColliderSphere.radius = playerColliderStartRadius;
	}
	public void SetColliderWidth(float width){
		playerColliderSphere.radius = width;
	}

}
