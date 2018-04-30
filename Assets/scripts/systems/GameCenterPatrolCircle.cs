using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script governs the visuals and logic for the patrol radius around the Runestone
public class GameCenterPatrolCircle : MonoBehaviour {

	public delegate void PlayerRadiusChanged(float newRadius);
	public static event PlayerRadiusChanged pRadChange;
	private int resolution = 32;
	[SerializeField] float radius, linePosY = 0.1f;
	float currentRadius;
	private LineRenderer line;
	Coroutine radiusChangeCoroutine;
	Vector3 lineRendererPosition;

	void Start () {
		currentRadius = radius;
		pRadChange += SetRadius;
		line = GetComponent<LineRenderer>();
	}
	
	void Update () {
		if(radius != currentRadius && radiusChangeCoroutine == null){
			pRadChange(radius);
		}
		line.positionCount = resolution + 1;
		for (var i = 0; i < line.positionCount; i++){
			var angle = (360/line.positionCount+1) * i;
			lineRendererPosition = transform.position + currentRadius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0.1f, Mathf.Sin(Mathf.Deg2Rad * angle));
			lineRendererPosition.y = linePosY;
			line.SetPosition(i, lineRendererPosition);
		}
	}

	public void SetCenterRadius(float newRadius){
		if(pRadChange != null)
			pRadChange(newRadius);
	}

	private void SetRadius(float newRadius){
		if(radiusChangeCoroutine != null)
			StopCoroutine(radiusChangeCoroutine);

		if(newRadius >= 1)
			radiusChangeCoroutine = StartCoroutine(MoveLineToNewRadius(newRadius));
		else
			radiusChangeCoroutine = StartCoroutine(MoveLineToNewRadius(5f));
	}

	IEnumerator MoveLineToNewRadius(float newRadius){
		float oldRadius = currentRadius;
		while(currentRadius != newRadius){
			currentRadius = Mathf.MoveTowards(currentRadius, newRadius, Mathf.Abs(newRadius - oldRadius) * Time.deltaTime);
			yield return null;
		}
		radiusChangeCoroutine = null;
	}

}
