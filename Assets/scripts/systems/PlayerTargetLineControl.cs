using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTargetLineControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	 PlayerTargetLine playerTargetLine;

	 public void Start(){
		 playerTargetLine = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerTargetLine>();
	 }

	 public void OnPointerDown(PointerEventData ped){
		playerTargetLine.isPointerDown = true;
		playerTargetLine.StartLineDrawing();
	 }

	 public void OnPointerUp(PointerEventData ped){
		 playerTargetLine.isPointerDown = false;
	 }
}
