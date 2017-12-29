using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterPatrolCircle : MonoBehaviour {

	private int resolution = 32;
	[SerializeField] private float radius;
	private LineRenderer line;

	void Start () {
		GameCenterRotation.pRadChange += SetRadius;

		line = GetComponent<LineRenderer>();
		radius = Vector3.Distance(transform.position, transform.GetChild(0).position);
	}
	
	void Update () {
		line.positionCount = resolution + 1;
		for (var i = 0; i < line.positionCount; i++){
			var angle = (360/line.positionCount+1) * i;
			line.SetPosition(i, transform.position + radius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0.1f, Mathf.Sin(Mathf.Deg2Rad * angle)));
		}
	}

	public void SetRadius(float newRadius){
		StartCoroutine(MoveLineToNewRadius(newRadius));
	}

	IEnumerator MoveLineToNewRadius(float newRadius){
		while(radius != newRadius){
			radius = Mathf.MoveTowards(radius, newRadius, Time.deltaTime);
			yield return null;
		}
		yield return null;
	}

}
