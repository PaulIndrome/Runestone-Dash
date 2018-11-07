using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif

public class TargetAreaCircle : MonoBehaviour {

	public string areaDenomination = "areaCircle";
	public bool visible = false;
	public bool useMinRange = false;
	[Range(3, 64)] public int resolution = 32;
	[Range(0f, 2f)] public float lineWidth = 0.5f;
	public float minRadius = 2f, maxRadius = 4f, radiusOffset = 0f;
	public bool usePulse = false;
	public float pulse = 3f;
	public bool usePingPong = false;
	public float pingPong = 0f;

	public Material[] circleLineMaterials = new Material[3];

	private bool isSetup = false;
	private float actualOffset = 0f;
	CircleLineRenderer[] lineRenderers;
	GameObject lineRendererParent;

	// Use this for initialization
	void Start () {
		SetupLineRenderers(true);
	}

	public void SetupLineRenderers(bool checkIfSetup){
		if(!checkIfSetup || !isSetup){
			if(lineRendererParent == null){
				lineRendererParent = new GameObject();
				lineRendererParent.transform.SetParent(transform);
				lineRendererParent.transform.SetPositionAndRotation(transform.position, transform.rotation);
				lineRendererParent.name = areaDenomination;
			} else if(lineRendererParent.name != areaDenomination){
				lineRendererParent.name = areaDenomination;
			}
			lineRenderers = lineRendererParent.GetComponentsInChildren<CircleLineRenderer>();
			if(lineRendererParent.transform.childCount != 3 || lineRenderers.Length != 3){
				foreach(Transform t in lineRendererParent.transform){
					Destroy(t.gameObject);
				}
				for(int i = 0; i < 3 ; i++){
					GameObject newChild = new GameObject();
					CircleLineRenderer clr = newChild.AddComponent<CircleLineRenderer>();
					newChild.transform.SetParent(lineRendererParent.transform);
					clr.SetType((TargetAreaCircleType) i, this, circleLineMaterials[i]);
				}
			} else {
				for(int i = 0; i < lineRenderers.Length; i++){
					if(i >= 3){
						Destroy(lineRenderers[i].gameObject);
					} else {
						lineRenderers[i].SetType((TargetAreaCircleType) i, this, circleLineMaterials[i]);
					}
				}
			}

			if(lineRenderers.Length <= 0)
				lineRenderers = lineRendererParent.GetComponentsInChildren<CircleLineRenderer>();

			actualOffset = 0f;

			if(usePulse && pulse > 0) actualOffset = radiusOffset % pulse;
			else if(usePingPong && pingPong > 0)
				actualOffset = Mathf.PingPong(radiusOffset, pingPong);
			else 
				actualOffset = radiusOffset;

			foreach(CircleLineRenderer circle in lineRenderers){
				switch(circle.thisCircleType){
					case TargetAreaCircleType.minCircle:
						if(useMinRange){
							if(!circle.drawLine) circle.drawLine = true;
							circle.UpdateAttributes(resolution, minRadius + actualOffset <= 0 ? 0f : lineWidth, minRadius + actualOffset);
						} else {
							circle.drawLine = false;
						}
						break;
					case TargetAreaCircleType.innerCircle:
						if(useMinRange){
							circle.UpdateAttributes(Mathf.Clamp(resolution, 3, 32), Mathf.Abs(maxRadius - minRadius), ((minRadius + maxRadius) / 2f) + actualOffset);
						} else {
							circle.UpdateAttributes(Mathf.Clamp(resolution, 3, 32), Mathf.Abs(maxRadius + actualOffset), ((maxRadius + actualOffset) / 2f));
						}
						break;
					case TargetAreaCircleType.maxCircle:
						circle.UpdateAttributes(resolution, lineWidth, maxRadius + actualOffset);
						break;
				}
			}
			isSetup = true;
			OnValidate();
		} else {
			return;
		}
		
	}

	public void OnValidate(){
		minRadius = Mathf.Clamp(minRadius, 0f, maxRadius - 1f);
		maxRadius = Mathf.Clamp(maxRadius, minRadius + 1f, 100f);

		if(isSetup){
			if(lineRenderers == null || lineRenderers.Length <= 0)
				lineRenderers = GetComponentsInChildren<CircleLineRenderer>();

			actualOffset = 0f;

			if(usePulse && pulse > 0) actualOffset = radiusOffset % pulse;
			else if(usePingPong && pingPong > 0)
				actualOffset = Mathf.PingPong(radiusOffset, pingPong);
			else 
				actualOffset = radiusOffset;

			foreach(CircleLineRenderer circle in lineRenderers){
				switch(circle.thisCircleType){
					case TargetAreaCircleType.minCircle:
						if(useMinRange){
							if(!circle.drawLine) circle.drawLine = true;
							circle.UpdateAttributes(resolution, minRadius + actualOffset <= 0 ? 0f : lineWidth, minRadius + actualOffset);
						} else {
							circle.drawLine = false;
						}
						break;
					case TargetAreaCircleType.innerCircle:
						if(useMinRange){
							circle.UpdateAttributes(Mathf.Clamp(resolution, 3, 32), Mathf.Abs(maxRadius - minRadius), ((minRadius + maxRadius) / 2f) + actualOffset);
						} else {
							circle.UpdateAttributes(Mathf.Clamp(resolution, 3, 32), Mathf.Abs(maxRadius + actualOffset), ((maxRadius + actualOffset) / 2f));
						}
						break;
					case TargetAreaCircleType.maxCircle:
						circle.UpdateAttributes(resolution, lineWidth, maxRadius + actualOffset);
						break;
				}
			}
		}
	}

	public void RelocateOrigin(Vector3 newPos){
		lineRendererParent.transform.position = newPos;
	}

	public bool Contains(Vector3 position){
		float distance = Vector3.Distance(transform.position, position);
		return Contains(distance);
	}
	public bool Contains(float distance){
		return distance < (maxRadius + actualOffset) && ((!useMinRange) || (useMinRange && distance > (minRadius + actualOffset)));
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(TargetAreaCircle))]
public class TargetAreaCircleInspector : Editor {
	public override void OnInspectorGUI(){
		TargetAreaCircle tac = serializedObject.targetObject as TargetAreaCircle;
		if(GUILayout.Button("Re-Setup"))
			tac.SetupLineRenderers(false);
		DrawDefaultInspector();
	}
}
#endif