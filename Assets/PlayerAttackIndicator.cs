using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackIndicator : MonoBehaviour {

	[SerializeField] PlayerAttack currentAttack;
	[HideInInspector] public Camera mainCam;

	public Coroutine visualsRoutine;

	void Start(){
		mainCam = Camera.main;
		PlayerInput.pointerDownEvent += StartTargeting;
		PlayerInput.pointerUpEvent += EndTargeting;
	}

	void StartTargeting(){
		EndTargeting();
		//currentAttack.ShowVisuals(this);
	}

	void EndTargeting(){
		if(visualsRoutine != null){
			StopCoroutine(visualsRoutine);
			visualsRoutine = null;
		}
	}



}
