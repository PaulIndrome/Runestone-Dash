using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterPatrolCircle : MonoBehaviour {

	public delegate void PlayerRadiusChanged(float newRadius);
	public static event PlayerRadiusChanged pRadChange;
	private int resolution = 32;
	[SerializeField] private float radius;
	private LineRenderer line;

	void Start () {
		pRadChange += SetRadius;
		line = GetComponent<LineRenderer>();
	}
	
	void Update () {
		line.positionCount = resolution + 1;
		for (var i = 0; i < line.positionCount; i++){
			var angle = (360/line.positionCount+1) * i;
			line.SetPosition(i, transform.position + radius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0.1f, Mathf.Sin(Mathf.Deg2Rad * angle)));
		}
	}

	public void SetCenterRadius(float newRadius){
		if(pRadChange != null)
			pRadChange(newRadius);
	}

	private void SetRadius(float newRadius){
		if(newRadius >= 1)
			StartCoroutine(MoveLineToNewRadius(newRadius));
		else
			StartCoroutine(MoveLineToNewRadius(5f));
	}

	IEnumerator MoveLineToNewRadius(float newRadius){
		while(radius != newRadius){
			radius = Mathf.MoveTowards(radius, newRadius, Time.deltaTime);
			yield return null;
		}
		yield return null;
	}

}
