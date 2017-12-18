using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDash : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	private Camera mainCam;
	public float deadZoneRadius;
	public float dashTime;
	public float dashSpeed;
	public Vector3 pointerStart;
	public Vector3 pointerInWorld;
	public PlayerState playerState;
	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
	}
	
	public void OnBeginDrag(PointerEventData ped){
		if(!playerState.currentlyDashing){
			pointerStart = ped.position;
		}
	}

	public void OnDrag(PointerEventData ped){
		if(Vector2.Distance(ped.pressPosition, ped.position) >= deadZoneRadius && !playerState.currentlyDashing){
			playerState.currentlyDashing = true;
			StartCoroutine(DashInDirection(ped.pointerCurrentRaycast.worldPosition));
		}
	}

	public void OnEndDrag(PointerEventData ped){
		//playerState.currentlyDashing = false;
	}


	IEnumerator DashInDirection(Vector3 target){
		target.y = 0;
		Time.timeScale = 0.5f;
		Debug.Log("Dashing started at " + Time.time);
		transform.LookAt(target);
		Vector3 direction = (target - transform.position).normalized;
		direction.y = 0;
		float distance = Vector3.Distance(transform.position, target);
		float startTime = Time.time;
		float dashEndTime = startTime + dashTime;
		float timeScaleMaxDelta = 0.5f / (dashEndTime - startTime);
		while(Time.time < startTime + dashTime){
			transform.position = transform.position + (direction * Time.deltaTime * dashSpeed);
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, timeScaleMaxDelta);
			yield return null;
		}
		Time.timeScale = 1f;
		yield return new WaitForSeconds(0.5f);
		playerState.currentlyDashing = false;
		yield return null;
	}
}
