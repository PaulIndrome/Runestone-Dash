using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathFollower : MonoBehaviour {

	bool guideLerping = false;
	int moveToward;
	Vector3 currentVelocity;
	Coroutine lerpGuideRoutine, followPathRoutine;
	[SerializeField] float smoothTime = 1f, posRadius = 1f, speed = 1f, guideLerpTime = 1f;
	List<Vector3> currentPath;

	Transform guideTransform;

	public void SlotPath(List<Vector3> path){
		currentPath = path;
	}

	public void StartFollowingPath(){
		if(followPathRoutine != null) return;
		followPathRoutine = StartCoroutine(FollowPath());
	}

	IEnumerator FollowPath(){

		transform.position = currentPath[0];

		//Vector3 guide = new Vector3(currentPath[1].x, currentPath[1].y, currentPath[1].z);

		guideTransform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		guideTransform.localScale = new Vector3(0.5f, 3f, 0.5f);
		guideTransform.position = currentPath[1];

		moveToward = 1;

		while(Vector3.Distance(transform.position, currentPath[currentPath.Count - 1]) > 0.01f){
			transform.position = Vector3.MoveTowards(transform.position, guideTransform.position, Time.deltaTime * speed);
			if(Vector3.Distance(transform.position, guideTransform.position) < posRadius && moveToward < currentPath.Count-1 && !guideLerping){
				if(lerpGuideRoutine != null) {
					StopCoroutine(lerpGuideRoutine);
				}
				lerpGuideRoutine = StartCoroutine(LerpGuide(currentPath[moveToward+1], guideLerpTime));
			}
			//transform.position = Vector3.SmoothDamp(transform.position, guideTransform.position, ref currentVelocity, smoothTime, speed);
			yield return null;
		}

		followPathRoutine = null;
	}

	IEnumerator LerpGuide(Vector3 target, float time){
		Debug.Log("Moving towards target " + (moveToward+1));
		moveToward++;
		guideLerping = true;
		for(float t = 0; t < time; t = t + Time.deltaTime){
			guideTransform.position = Vector3.Lerp(guideTransform.position, target, t / time);
			yield return null;
		}
		guideTransform.position = target;
		guideLerping = false;
		lerpGuideRoutine = null;
	}
}
