using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterRotation : MonoBehaviour {

	public float turningSpeed;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, Time.deltaTime*turningSpeed);
	}
}
