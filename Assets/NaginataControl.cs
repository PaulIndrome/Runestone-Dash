using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaginataControl : MonoBehaviour {

	ParticleSystem bladeTrailPS;

	void Start(){
		bladeTrailPS = GetComponent<ParticleSystem>();
	}

	public void StartBladeTrail(){
		bladeTrailPS.Play();
	}

	public void StopBladeTrail(){
		bladeTrailPS.Stop();
	}

}
