﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="serializableObjects/ParticleSystem"), System.Serializable]
public class RandomParticleSystemSpawner : ScriptableObject {

	[Tooltip("particle systems must not be indefinitely looping")]
	public GameObject[] particleSystems;

	public void SpawnRandomAndPlay(Transform parent, Vector3 spawnAtPosWorld, Vector3 lookAtPosWorld){
		GameObject tempObject = Instantiate(particleSystems[Random.Range(0, particleSystems.Length)]);
		//spawnAtPosWorld.z = 0f;
		spawnAtPosWorld.y = Mathf.Lerp(spawnAtPosWorld.y, 0, 0.3f);
		tempObject.transform.position = spawnAtPosWorld;
		tempObject.transform.parent = parent;
		lookAtPosWorld.y = tempObject.transform.position.y;
		tempObject.transform.LookAt(lookAtPosWorld);
	}
	
}
