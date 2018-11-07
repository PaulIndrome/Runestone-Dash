using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public delegate void PointerDownDelegate();
	public static PointerDownDelegate pointerDownEvent, pointerUpEvent;

	public static bool pointerDown = false;

	[SerializeField] PlayerAttack currentAttack;

	public void OnPointerDown(PointerEventData eventData) {
		pointerDown = true;
		if(pointerDownEvent != null)
			pointerDownEvent();
	}

	public void OnPointerUp(PointerEventData eventData) {
		pointerDown = false;
		if(pointerUpEvent != null)
			pointerUpEvent();
	}

	
}
