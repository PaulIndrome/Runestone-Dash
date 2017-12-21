using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDash : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	[SerializeField] private float deadZoneRadius;
	public float dashTime;
	public float dashSpeed;

	private bool isPointerCloseToCharacter = false;
	public Vector3 pointerStart;
	public Vector3 pointerInWorld;
	public PlayerState playerState;
	
	public void OnPointerDown(PointerEventData ped){
		isPointerCloseToCharacter = true;
		if(playerState.canDash){
			pointerStart = ped.position;
			StartCoroutine(HoldPointer(ped.pointerCurrentRaycast.worldPosition, 2f));
		}
	}

	public void OnDrag(PointerEventData ped){
		if(Vector3.Distance(ped.pointerPressRaycast.worldPosition, ped.pointerCurrentRaycast.worldPosition) >= deadZoneRadius && playerState.canDash && isPointerCloseToCharacter){
			StartCoroutine(DashInDirection(ped.pointerCurrentRaycast.worldPosition));
		}
	}

	public void OnPointerUp(PointerEventData ped){
		StopCoroutine(HoldPointer(Vector3.zero, 0));
		Time.timeScale = 1f;
		isPointerCloseToCharacter = false;
	}

	IEnumerator HoldPointer(Vector3 pointerDownWorldPos, float time){
		bool outOfReach = false;
		Time.timeScale = 0.33f;
		while(!outOfReach && isPointerCloseToCharacter){
			//Debug.Log(Vector3.Distance(pointerDownWorldPos, transform.position));
			if(Vector3.Distance(pointerDownWorldPos, transform.position) > deadZoneRadius){
				outOfReach = true;
				break;
			}
			yield return null;
		}
		Time.timeScale = 1f;
		pointerStart = Vector3.zero;
		isPointerCloseToCharacter = false;
		yield return null;
	}

	IEnumerator DashInDirection(Vector3 target){
		playerState.canDash = false;
		target.y = 0;
		transform.LookAt(target);
		Vector3 direction = (target - transform.position).normalized;
		direction.y = 0;
		float startTime = Time.time;
		while(Time.time < startTime + dashTime){
			transform.position = transform.position + (direction * Time.deltaTime * dashSpeed);
			yield return null;
		}
		Time.timeScale = 1f;
		yield return new WaitForSeconds(0.5f);
		playerState.canDash = true;
		yield return null;
	}

}
