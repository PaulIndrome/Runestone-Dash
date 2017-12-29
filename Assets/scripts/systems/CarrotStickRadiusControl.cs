﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotStickRadiusControl : MonoBehaviour {

	private float currentRadius;
	private float timeStep;
	private float circleResolution = 60;
	public float timeToCircle = 10f;

	private float angle = 90;

	private bool runningInCircles = false;


	public void Start(){
		GameCenterRotation.pRadChange += SetRadiusTo;

		currentRadius = transform.position.magnitude;

		timeStep = (2 * Mathf.PI) / timeToCircle;

		Invoke("StartCircleMovement", 2f);
	}

	public void SetRadiusTo(float newRadius){
		StartCoroutine(MoveCarrotStickToNewRadius(newRadius));
	}

	public void StartCircleMovement(){
		StartCoroutine(MoveCarrotStickInCircle());
	}

	public void SetTimeToCircle(float newTimeToCircle){
		if(newTimeToCircle == 0){
			timeToCircle = timeStep = 0;
			return;
		} else {
			timeStep = (2 * Mathf.PI) / timeToCircle;
			if(!runningInCircles) StartCoroutine(MoveCarrotStickInCircle());
		}
	}

	IEnumerator MoveCarrotStickToNewRadius(float newRadius){
		while(currentRadius != newRadius){
			currentRadius = Mathf.MoveTowards(currentRadius, newRadius, Time.deltaTime);
			yield return null;
		}
		yield return null;
	}

	IEnumerator MoveCarrotStickInCircle(){
		runningInCircles = true;
		while(timeToCircle != 0){
			angle = (angle + timeStep) % 360;
			transform.position = currentRadius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
			yield return null;
		}
		runningInCircles = false;
		yield return null;
	}

}