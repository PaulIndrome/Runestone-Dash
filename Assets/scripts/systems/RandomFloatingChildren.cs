using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script to make the children of any given GameObject float randomly according to set parameters
//used for the floating stones above the Runestone
public class RandomFloatingChildren : MonoBehaviour {

	Transform[] childObjects;
	Vector3[] startPositions;
	Vector3[] startRotations;
	float time;

	[Header("slow down < 1 > speed up")]
	public float timeScale;
	[Header("maximum distance to travel in axis")]
	public float xMaxD;
	public float yMaxD;
	public float zMaxD;
	[Header("maximum angle to rotate around axis")]
	public float xMaxR;
	public float yMaxR;
	public float zMaxR;

	private Vector3 maxDistanceVector;
	private Vector3 maxRotationVector;

	// Use this for initialization
	void Start () {
		childObjects = new Transform[transform.childCount];
		startPositions = new Vector3[childObjects.Length];
		startRotations = new Vector3[childObjects.Length];

		for(int i = 0; i < childObjects.Length; i++){
			childObjects[i] = transform.GetChild(i);
			startPositions[i] = childObjects[i].position;
			startRotations[i] = childObjects[i].rotation.eulerAngles;
		}
	}
	
	// Update is called once per frame
	void Update () {
		time = Time.time * timeScale;
		maxDistanceVector = new Vector3(xMaxD, yMaxD, zMaxD);
		maxRotationVector = new Vector3(xMaxR, yMaxR, zMaxR);
		for(int i = 0; i < childObjects.Length; i++){
			childObjects[i].position = startPositions[i] + (Mathf.PerlinNoise(time, i * 3918.12f) * 2 - 1) * maxDistanceVector;
			childObjects[i].rotation = Quaternion.Euler(startRotations[i] + (Mathf.PerlinNoise(time, i * 3918.12f) * 2 - 1) * maxRotationVector);
		}
	}
}
