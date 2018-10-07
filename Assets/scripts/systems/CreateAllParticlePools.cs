using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAllParticlePools : MonoBehaviour {

	[Tooltip("Instantiate all PoolableParticles within these pools on start")]
	[SerializeField] ParticlePooler[] particlePoolers;

	void Start(){
		//ParticlePooler[] allPoolers = Resources.FindObjectsOfTypeAll(typeof(ParticlePooler)) as ParticlePooler[];
		foreach(ParticlePooler p in particlePoolers){
			p.CreatePool();
		}
	}
}
