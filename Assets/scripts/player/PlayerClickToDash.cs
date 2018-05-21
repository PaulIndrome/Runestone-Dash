using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this script sits on the object that intercepts the player input
//hence, the big collision box under the ground
public class PlayerClickToDash : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
	PlayerDashChaining playerDashChaining;
	PlayerTargetLine playerTargetLine;
	Player player;
	[SerializeField] [Range(0.1f, 1.5f)] private float delayBetweenDashes;
	[HideInInspector] public bool recentlyDashed = false, pinchStarted = false;
	Coroutine inputClearCheck;

	public void Start(){
		playerDashChaining = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDashChaining>();
		player = playerDashChaining.gameObject.GetComponent<Player>();
		playerTargetLine = playerDashChaining.gameObject.GetComponentInChildren<PlayerTargetLine>();
	}

	public void OnPointerDown(PointerEventData ped){
		if(Input.touchCount == 1 || Input.GetMouseButtonDown(0)){
			playerTargetLine.isPointerDown = true;
			playerTargetLine.StartLineDrawing();
		} else if(Input.touches.Length == 2) {
			Debug.Log("Pinch Started in PlayerClickToDash");
			pinchStarted = true;
			playerTargetLine.isPointerDown = false;
		}
	}

	public void OnPointerClick(PointerEventData ped){
		
	}

	public void OnPointerUp(PointerEventData ped){
		if(!player.playerState.isLegendary && !pinchStarted){
			playerDashChaining.StashTarget(ped.pointerCurrentRaycast.worldPosition);
			StartCoroutine(ResetRecentlyDashed());
		}

		if(inputClearCheck != null) StopCoroutine(inputClearCheck);
		inputClearCheck = StartCoroutine(CheckForAllInputClear());

	}

	void Reset(){
		if(Input.touches.Length < 1){
			Debug.Log("All touches finished");
			pinchStarted = false;
			playerTargetLine.isPointerDown = false;
		}
	}

	IEnumerator ResetRecentlyDashed(){
		recentlyDashed = true;
		yield return new WaitForSeconds(delayBetweenDashes);
		recentlyDashed = false;
	}

	IEnumerator CheckForAllInputClear(){
		while(pinchStarted || playerTargetLine.isPointerDown){
			if(Input.touches.Length < 1 && !Input.anyKeyDown){
				Debug.Log("All touches finished at " + Time.time);
				pinchStarted = false;
				playerTargetLine.isPointerDown = false;
				inputClearCheck = null;
				Time.timeScale = 1f;
				yield break;
			}
			yield return new WaitForFixedUpdate();
		}
	}

}
