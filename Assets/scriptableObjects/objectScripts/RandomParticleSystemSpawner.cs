using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="serializableObjects/ParticleSystem"), System.Serializable]
public class RandomParticleSystemSpawner : ScriptableObject {

	[Tooltip("particle systems must not be indefinitely looping")]
	public GameObject[] particleSystems;

	public void SpawnRandomAndPlay(Transform carrier, Vector3 contactLocalPos, Vector3 targetWorldPos){
		GameObject tempObject = Instantiate(particleSystems[Random.Range(0, particleSystems.Length)]);
		tempObject.transform.parent = carrier.parent;
		contactLocalPos.z = 0f;
		contactLocalPos.y = Mathf.Lerp(contactLocalPos.y, 0, 0.3f);
		tempObject.transform.localPosition = contactLocalPos;
		targetWorldPos.y = tempObject.transform.position.y;
		tempObject.transform.LookAt(targetWorldPos);
	}
	
}
