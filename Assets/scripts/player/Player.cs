using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Animator animator;
	SphereCollider playerColliderSphere;
	public PlayerState playerState;
	Vector3 playerColliderStartSize;
	float playerColliderStartRadius;
	[HideInInspector] public PlayerDashChaining playerDashChaining;

	void Awake(){
		if(playerState == null) 
			playerState = PlayerState.CreateInstance(typeof(PlayerState)) as PlayerState;
	}

	public void Start(){
		animator = GetComponent<Animator>();
		playerDashChaining = GetComponent<PlayerDashChaining>();
		playerColliderSphere = GetComponent<SphereCollider>();
		playerColliderStartRadius = playerColliderSphere.radius;

		playerState.StartComboResetter(this);

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
