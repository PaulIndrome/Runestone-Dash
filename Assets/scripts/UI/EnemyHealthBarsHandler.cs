using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarsHandler : MonoBehaviour {

	private Camera mainCam;
	private List<HealthBar> healthBars = new List<HealthBar>();
	void Start(){
		mainCam = Camera.main;
	}
	
	void Update () {
		foreach(HealthBar h in healthBars){
			//the repositioning of any health oder durability bar is handled centrally for performance purposes
			//also I apparently wrote this while sleeping because it worked so well right out of the box...
			h.RepositionToTarget(mainCam.WorldToScreenPoint(h.healthBarPosition.position));
		}
	}

	public bool RegisterBar(HealthBar h){
		if(!healthBars.Contains(h)){
			healthBars.Add(h);
			h.SetHandler(this);
			return true;
		} else 
			return false;
	}

	public bool UnRegisterBar(HealthBar h){
		if(healthBars.Contains(h)){
			Debug.Log("Unregistered");
			healthBars.Remove(h);
			return true;
		} else
			return false;
	}

}
