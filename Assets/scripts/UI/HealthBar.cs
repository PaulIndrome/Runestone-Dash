using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Transform healthBarPosition;

	public RectTransform Bar;

	Image barImage;
	Image barEndImage;

	public void Start(){
		barImage = Bar.GetComponent<Image>();
		barEndImage = Bar.GetChild(0).GetComponent<Image>();
	}

	public void SetTarget(Transform position){
		healthBarPosition = position;
		barImage = Bar.GetComponent<Image>();
	}

	public void RepositionToTarget(Vector3 screenPosition){
		transform.position = screenPosition;
	}

	public void SetBarTo(float healthPercent){
		if(healthPercent <= 0) barEndImage.color = Color.clear;
		float newWidth = healthPercent * 100;
		Bar.sizeDelta = new Vector2(newWidth, Bar.sizeDelta.y);
	}
	
	public void ChangeBarColorTo(Color color){
		barImage.color = color;
	}

	public void ChangeBarEndColorTo(Color color){
		if(barEndImage == null) barEndImage = Bar.GetChild(0).GetComponent<Image>();
		barEndImage.color = color;
	}

}
