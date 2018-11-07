using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Transform healthBarPosition;

	public RectTransform Bar;

	Image barImage;
	Image barEndImage;

	EnemyHealthBarsHandler handler;

	public void Awake(){
		barImage = Bar.GetComponent<Image>();
		barEndImage = Bar.GetChild(0).GetComponent<Image>();
	}

	//which object should this bar follow during repositioning
	public void SetTarget(Transform position){
		healthBarPosition = position;
		barImage = Bar.GetComponent<Image>();
	}

	// called from handler upon registering
	public void SetHandler(EnemyHealthBarsHandler barHandler){
		handler = barHandler;
	}

	public void Unregister(){
		handler.UnRegisterBar(this);
		Destroy(gameObject);
	}

	//all healthBars and durabilityBars get repositioned to their designated
	//target's position by the EnemyHealthBarsHandler 
	public void RepositionToTarget(Vector3 screenPosition){
		transform.position = screenPosition;
	}

	//reduce the size of the bar according to the percentage of health or durability left
	public void SetBarTo(float healthPercent){
		if(healthPercent <= 0) barEndImage.color = Color.clear;
		float newWidth = healthPercent * 100;
		//cleverly, the starting sizeDelta.x of any health or durability bar is 100
		Bar.sizeDelta = new Vector2(newWidth, Bar.sizeDelta.y);
	}
	
	//utility function to change the bar color 
	public void ChangeBarColorTo(Color color){
		barImage.color = color;
	}
	
	//utility function to change the bar's status line's color
	public void ChangeBarEndColorTo(Color color){
		//sometimes the reference isn't set quick enough during Start(), so this does it prior to referencing anything
		if(barEndImage == null) barEndImage = Bar.GetChild(0).GetComponent<Image>();
		barEndImage.color = color;
	}

	public void ApplyBossColors(){
		Color bossBarColor = Color.magenta;
		bossBarColor.a = 0.8f;
		ChangeBarColorTo(bossBarColor);
		bossBarColor.a = 0.95f;
		ChangeBarEndColorTo(bossBarColor);
	}

}
