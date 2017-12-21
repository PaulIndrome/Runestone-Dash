using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour {

	public float followSpeed;
	public bool allowedToFollow = true;
	public Transform carrotStick;
	public PlayerState playerState;
	
	void Start () {
		playerState.canDash = true;
		allowedToFollow = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerState.canDash && allowedToFollow){
			transform.LookAt(carrotStick);
			transform.position = Vector3.MoveTowards(transform.position, carrotStick.position, Time.deltaTime*followSpeed);
		}
	}
}
