using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathTester : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] int steps = 3;
	[SerializeField] float debugRayDuration = 5f;
	[SerializeField] LayerMask environmentCheck = 0;

	public RandomPath currentRandomPath;

	 public Color startColor = Color.magenta;

	// Use this for initialization
	void Start () {
		
	}
	
	public void CreateRandomPath(){
		currentRandomPath = RandomPath.CreateInstance(typeof(RandomPath)) as RandomPath;
		currentRandomPath.CreatePath(transform, target, steps, startColor, environmentCheck, debugRayDuration);
	}

	public void StartRandomPathCreationCoroutine(){
		currentRandomPath = RandomPath.CreateInstance(typeof(RandomPath)) as RandomPath;
		StartCoroutine(currentRandomPath.CreatePathCoroutine(this, transform, target, steps, startColor, environmentCheck, debugRayDuration));
	}

}
