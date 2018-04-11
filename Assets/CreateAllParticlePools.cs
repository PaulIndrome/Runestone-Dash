using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAllParticlePools : MonoBehaviour {

	[Tooltip("Instantiate all PoolableParticles within these pools on start")]
	[SerializeField] ParticlePooler[] particlePoolers;

	void Start(){
		foreach(ParticlePooler p in particlePoolers){
			p.CreatePool(transform);
		}
	}
}
