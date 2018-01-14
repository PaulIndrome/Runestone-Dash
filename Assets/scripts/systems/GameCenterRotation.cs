using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterRotation : MonoBehaviour {

	public delegate void PlayerRadiusChanged(float newRadius);
	public static event PlayerRadiusChanged pRadChange;
	public float centerRadius;
	
	public void SetCenterRadius(float newRadius){
		if(pRadChange != null)
			pRadChange(newRadius);
	}

}
