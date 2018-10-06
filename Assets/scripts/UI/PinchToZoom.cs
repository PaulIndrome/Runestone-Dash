using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PinchToZoom : MonoBehaviour {

	 Touch one, two;
	 [SerializeField] float zoomSpeed = 0.01f;
	 [SerializeField] Slider slider;
	 PlayerClickToDash playerClickToDash;
	 PlayerTargetLine playerTargetLine;

	void Start(){
		playerClickToDash = GetComponent<PlayerClickToDash>();
		playerTargetLine = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerTargetLine>();
	}

	public void Update() {
		if(Input.touches.Length == 2) {
			
			playerClickToDash.pinchStarted = true;
			playerTargetLine.isPointerDown = false;

			// Store both touches.
            one = Input.GetTouch(0);
            two = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = one.position - one.deltaPosition;
            Vector2 touchOnePrevPos = two.position - two.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (one.position - two.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			slider.value += deltaMagnitudeDiff * zoomSpeed * Time.unscaledDeltaTime;
			
			
			//if(!pinchStarted){
			//	one = Input.GetTouch(0);
			//	two = Input.GetTouch(1);
			//	originalDistance = Vector3.Distance(one.position, two.position);
			//	pinchStarted = true;
			//}
			//
			//float distance = Vector3.Distance(one.position, two.position);
			//if(distance > originalDistance){
			//	slider.value += zoomSpeed;
			//} else if (distance < originalDistance) {
			//	slider.value -= zoomSpeed;
			//}
		}

	}
}
