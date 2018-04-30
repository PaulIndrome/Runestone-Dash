using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarsHandler : MonoBehaviour {

	private Camera mainCam;
	void Start(){
		mainCam = Camera.main;
	}
	
	
	void Update () {
		foreach(HealthBar h in GetComponentsInChildren<HealthBar>()){
			//the repositioning of any health oder durability bar is handled centrally for performance purposes
			//also I apparently wrote this while sleeping because it worked so well right out of the box...
			h.RepositionToTarget(mainCam.WorldToScreenPoint(h.healthBarPosition.position));
		}
	}
}
