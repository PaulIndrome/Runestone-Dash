using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterRotation : MonoBehaviour {

	public delegate void PlayerRadiusChanged(float newRadius);
	public static event PlayerRadiusChanged pRadChange;
	public float centerRadius;
	private GameCenterPatrolCircle gameCenterPatrolCircle;
	private CarrotStickRadiusControl carrotStickRadiusControl;
	
	public void Start(){
		gameCenterPatrolCircle = GetComponent<GameCenterPatrolCircle>();
		carrotStickRadiusControl = GetComponentInChildren<CarrotStickRadiusControl>();
	}

	public void SetCenterRadius(float newRadius){
		if(pRadChange != null)
			pRadChange(newRadius);
	}

}
