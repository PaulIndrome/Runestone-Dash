using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarsHandler : MonoBehaviour {

	private Camera mainCam;
	void Start(){
		mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		foreach(HealthBar h in GetComponentsInChildren<HealthBar>()){
			h.RepositionToTarget(mainCam.WorldToScreenPoint(h.healthBarPosition.position));
		}
	}
}
