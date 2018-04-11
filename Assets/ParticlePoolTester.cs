using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolTester : MonoBehaviour {

	public ParticlePooler particlePoolerToTest;

	public Transform lookAtPos;
	public float spawnDelay;

	public Transform parent;

	float nextSpawnTime;

	// Use this for initialization
	void Start () {
		particlePoolerToTest.CreatePool(parent);
		nextSpawnTime = Time.time + spawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= nextSpawnTime){
			//particlePoolerToTest.SpawnFromQueueAndPlay(parent, parent.position, lookAtPos.position);
			particlePoolerToTest.SpawnFromListAndPlay(parent, parent.position, lookAtPos.position);
			nextSpawnTime = Time.time + spawnDelay;
		}
	}
}
