using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class PlayerArcLine : MonoBehaviour {

	public bool visible = false;
	public bool useTargetPos = false;
	[SerializeField] int resolution = 32;
	public float distance = 5f, arcDistance = 1f, arcAngle = 90f;
	public Vector3 targetPos;
	public Material lineMaterialValid, lineMaterialInvalid;
	Vector3[] arcArray;
	LineRenderer line;
	Vector3 arcApex = Vector3.zero, arcTargetPos = Vector3.zero, targetVector = Vector3.zero;

	void Start(){
		line = GetComponent<LineRenderer>();
		line.material = lineMaterialValid;
	}

	public void RenderArc(bool valid){
		if(!line.enabled) line.enabled = true;
		line.material = valid ? lineMaterialValid : lineMaterialInvalid;
		line.positionCount = resolution + 1;
		line.SetPositions(CalculatePoints());
	}

	/// <summary> Toggle on (true), off (false) or opposite state (null) </summary>
	public void Toggle(bool? onOff){
		if(onOff == null){
			line.enabled = !line.enabled;
		} else {
			line.enabled = (bool) onOff;
		}
	}

	Vector3[] CalculatePoints(){
		if(useTargetPos){
			arcTargetPos = targetPos;
			arcTargetPos.y = 0f;
			distance = (arcTargetPos - transform.position).magnitude;
			transform.rotation = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, arcTargetPos - transform.position, Vector3.up), 0f);
		} else {
			arcTargetPos = transform.position + (transform.forward * distance);
		}

		float y = Mathf.Sin(Mathf.Deg2Rad * arcAngle) * arcDistance;
		float x = Mathf.Cos(Mathf.Deg2Rad * arcAngle) * arcDistance;
		arcApex = transform.position + (transform.forward * (distance / 2f)) + (transform.up * y) + (transform.right * x);

		arcArray = new Vector3[resolution + 1];
		
		for(int i = 0; i <= resolution; i++){
			float t = (float) i / (float) resolution;
			arcArray[i] = GetQuadraticCoordinates(t);
		}

		return arcArray;
	}

	Vector3 GetQuadraticCoordinates(float t){	
		return Mathf.Pow(1f - t, 2f) * transform.position + 2 * t * (1 - t) * arcApex + Mathf.Pow(t, 2) * arcTargetPos;
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(arcApex, 0.25f);
	}

}
