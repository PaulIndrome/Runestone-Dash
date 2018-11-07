using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour {

	protected Coroutine drawRoutine;

	protected virtual void Start(){
		PlayerInput.pointerDownEvent += StartDrawing;
		PlayerInput.pointerUpEvent += EndDrawing;
	}

	public virtual void StartDrawing(){
		ConditionalStopDrawRoutine();
		drawRoutine = StartCoroutine(DrawVisuals());
	}
	public virtual void EndDrawing(){
		ConditionalStopDrawRoutine();
	}

	protected void ConditionalStopDrawRoutine(){
		if(drawRoutine != null){
			StopCoroutine(drawRoutine);
			drawRoutine = null;
		}
	}

	public abstract IEnumerator DrawVisuals();

	protected virtual void OnDestroy(){
		PlayerInput.pointerDownEvent -= StartDrawing;
		PlayerInput.pointerUpEvent -= EndDrawing;
	}

}
