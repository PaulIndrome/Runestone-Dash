using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathTester : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] int steps = 3;
	[SerializeField] float debugRayDuration = 5f, minRadius = 15f, maxRadius = 20f;
	[SerializeField] LayerMask environmentCheck = 0;

	RandomPathFollower randomPathFollower;

	public RandomPath currentRandomPath;

	 public Color startColor = Color.magenta;

	// Use this for initialization
	void Start () {
		randomPathFollower = GetComponent<RandomPathFollower>();
	}
	
	public void CreateRandomPath(){
		Vector2 randomOnCircle = Random.insideUnitCircle.normalized;
		Vector3 position = target.position + new Vector3(randomOnCircle.x, 0, randomOnCircle.y) * (Random.Range(minRadius, maxRadius));

		GameObject computePos = GameObject.CreatePrimitive(PrimitiveType.Cube);
		computePos.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
		computePos.transform.position = position;

		currentRandomPath = RandomPath.CreateInstance(typeof(RandomPath)) as RandomPath;
		currentRandomPath.CreatePath(computePos.transform, target, steps, startColor, environmentCheck, debugRayDuration);

		randomPathFollower.SlotPath(currentRandomPath.Path);
		randomPathFollower.StartFollowingPath();
	}

	public void StartRandomPathCreationCoroutine(){
		Vector2 randomOnCircle = Random.insideUnitCircle.normalized;
		Vector3 position = target.position + new Vector3(randomOnCircle.x, 0, randomOnCircle.y) * (Random.Range(minRadius, maxRadius));

		GameObject computePos = GameObject.CreatePrimitive(PrimitiveType.Cube);
		computePos.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
		computePos.transform.position = position;

		currentRandomPath = RandomPath.CreateInstance(typeof(RandomPath)) as RandomPath;
		StartCoroutine(currentRandomPath.CreatePathCoroutine(this, computePos.transform, target, steps, startColor, environmentCheck, debugRayDuration));
	}

}
