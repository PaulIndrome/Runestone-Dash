using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleLineRenderer : MonoBehaviour {

	public TargetAreaCircleType thisCircleType;
	[HideInInspector] public bool drawLine = true;
	[SerializeField] private int resolution = 32;
	[SerializeField] private float lineWidth = 0.5f;
	[SerializeField] private float radius = 5f;
	private TargetAreaCircle father;
	private float heightCorrection;
	
	LineRenderer line;

	void Start(){
		line = GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update () {
		if(father.visible && drawLine){
			if(!line.enabled) line.enabled = true;

			line.positionCount = resolution + 1;

			for (var i = 0; i < line.positionCount; i++){
				var angle = (360/line.positionCount+1) * i;
				line.SetPosition(i, transform.position - (transform.forward * heightCorrection) + radius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)));
			}
		} else {
			if(line.enabled)
				line.enabled = false;
		}
	}

	public void UpdateAttributes(int newResolution, float newWidth, float newRadius){
		resolution = newResolution;
		lineWidth = newWidth;
		line.startWidth = line.endWidth = lineWidth;
		radius = newRadius;
	}

	public void SetType(TargetAreaCircleType type, TargetAreaCircle parent, Material mat){
		line = GetComponent<LineRenderer>();
		line.material = mat;
		line.loop = true;
		line.alignment = LineAlignment.TransformZ;
		line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		line.receiveShadows = false;
		line.allowOcclusionWhenDynamic = false;

        father = parent;
		thisCircleType = type;
		gameObject.name = type.ToString();

		transform.SetPositionAndRotation(parent.transform.position, Quaternion.Euler(90f, 0f, 0f));

		switch(type){
			case TargetAreaCircleType.minCircle:
				heightCorrection = 0.05f;
				break;
			case TargetAreaCircleType.innerCircle:
				heightCorrection = 0.025f;
				break;
			case TargetAreaCircleType.maxCircle:
				heightCorrection = 0.075f;
				break;
		}
	}

}
