using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Transform healthBarPosition;

	public RectTransform Bar;

	Image barImage;

	public void Start(){
		barImage = Bar.GetComponent<Image>();
	}

	public void SetTarget(Transform position){
		healthBarPosition = position;
		barImage = Bar.GetComponent<Image>();
	}

	public void RepositionToTarget(Vector3 screenPosition){
		transform.position = screenPosition;
	}

	public void SetBarTo(float healthPercent){
		Bar.localScale = new Vector3(healthPercent, 1, 1);
	}
	
	public void ChangeBarColorTo(Color color){
		barImage.color = color;
	}

}
