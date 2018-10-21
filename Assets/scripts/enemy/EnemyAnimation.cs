using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

	private Animator animator;
	void OnEnable () {
		animator = GetComponentInChildren<Animator>();
	}
	
	public void SetBool(string boolToSet, bool b){
		animator.SetBool(boolToSet, b);
	}

	public void SetTrigger(string triggerToSet){
		animator.SetTrigger(triggerToSet);
	}

}
