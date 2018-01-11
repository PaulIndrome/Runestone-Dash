using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotStickRadiusControlLookAtPlayer : MonoBehaviour {

	public PlayerState playerState;
	public PlayerFollow playerFollow;
	public float radiusTolerance;
	private float currentRadius;
	private float timeStep;
	private float circleResolution = 60;
	public float timeToCircle = 10f;
	private float angle = 90;
	private bool runningInCircles = false;
	[SerializeField] private bool playerOnRadius = true;


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
		StartCoroutine(MoveCarrotStickAlongCircle());
	}

	public bool IsPlayerOnCircle(){
		return 	playerFollow.transform.position.magnitude >= currentRadius - radiusTolerance && 
				playerFollow.transform.position.magnitude <= currentRadius + radiusTolerance;
	}

	public void SetTimeToCircle(float newTimeToCircle){
		if(newTimeToCircle == 0){
			timeToCircle = timeStep = 0;
			return;
		} else {
			timeStep = (2 * Mathf.PI) / timeToCircle;
			if(!runningInCircles) StartCoroutine(MoveCarrotStickAlongCircle());
		}
	}

	public void MoveCarrotStick(bool rotationNeeded){
		Vector3 nextPos = currentRadius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
		if(rotationNeeded){
			Quaternion lookRotation = Quaternion.LookRotation((nextPos - transform.position), Vector3.up);
			transform.rotation = lookRotation;
		}
		transform.position = nextPos;
	}

	IEnumerator MoveCarrotStickToNewRadius(float newRadius){
		while(currentRadius != newRadius){
			currentRadius = Mathf.MoveTowards(currentRadius, newRadius, Time.deltaTime);
			yield return null;
		}
		yield return null;
	}

	IEnumerator MoveCarrotStickAlongCircle(){
		runningInCircles = true;
		while(timeToCircle != 0){
			playerOnRadius = IsPlayerOnCircle();
			if(playerOnRadius){
				angle = (angle + timeStep) % 360;
			} else {
				angle = Vector3.SignedAngle(Vector3.forward, playerFollow.transform.position, Vector3.up);
			}
			MoveCarrotStick(playerOnRadius);
			yield return null;
		}
		runningInCircles = false;
		yield return null;
	}

}
