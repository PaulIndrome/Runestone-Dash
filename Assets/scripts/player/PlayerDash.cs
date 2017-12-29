﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDash : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	[SerializeField] private float deadZoneRadius;
	public float classDashTime;
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
			StartCoroutine(DashInDirection(ped.pointerCurrentRaycast.worldPosition, classDashTime + 0.1f));
		}
	}

	public void OnPointerUp(PointerEventData ped){
		StopCoroutine(HoldPointer(Vector3.zero, 0));
		Time.timeScale = 1f;
		isPointerCloseToCharacter = false;
	}

	public void ClickToDash(PointerEventData ped){
		StartCoroutine(DashInDirection(ped.pointerCurrentRaycast.worldPosition, classDashTime));
	}

	IEnumerator HoldPointer(Vector3 pointerDownWorldPos, float time){
		bool outOfReach = false;
		Time.timeScale = 0.33f;
		while(!outOfReach && isPointerCloseToCharacter){
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

	IEnumerator DashInDirection(Vector3 target, float dashTime){
		//GameObject startObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//startObject.transform.position = transform.position;
		playerState.canDash = false;
		target.y = 0;
		transform.LookAt(target);
		Vector3 direction = (target - transform.position).normalized;
		direction.y = 0;
		float timer = 0;
		while(timer < dashTime && !playerState.hitEnemyShield){
			transform.position = transform.position + (direction * Time.deltaTime * dashSpeed);
			timer += Time.deltaTime;
			yield return null;
		}
		Time.timeScale = 1f;
		//GameObject endObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//endObject.transform.position = transform.position;
		//Debug.Log(Vector3.Distance(startObject.transform.position, endObject.transform.position));
		yield return new WaitForSeconds(0.5f);
		playerState.hitEnemyShield = false;
		playerState.canDash = true;
		yield return null;
	}

}
