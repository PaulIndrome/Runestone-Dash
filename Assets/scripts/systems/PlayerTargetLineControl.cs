using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this script controls when to draw a target line and when to stop
public class PlayerTargetLineControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	 PlayerTargetLine playerTargetLine;



	 public void Start(){
		 playerTargetLine = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerTargetLine>();
	 }

	 public void OnPointerDown(PointerEventData ped){
		if(Input.touchCount == 1){
			playerTargetLine.isPointerDown = true;
			playerTargetLine.StartLineDrawing();
		}
	 }

	 public void OnPointerUp(PointerEventData ped){
		 playerTargetLine.isPointerDown = false;
		
	 }
}
