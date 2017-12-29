using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour {

	public float followSpeed;
	public Transform carrotStick;
	public PlayerState playerState;
	
	void Start () {
		playerState.canDash = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerState.canDash){
			transform.LookAt(carrotStick);
			transform.position = Vector3.MoveTowards(transform.position, carrotStick.position, Time.deltaTime*followSpeed);
		}
	}
}
