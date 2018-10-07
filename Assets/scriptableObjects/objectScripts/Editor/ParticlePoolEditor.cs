using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParticlePooler))]
public class ParticlePoolEditor : Editor {

	public override void OnInspectorGUI(){
		ParticlePooler pp = serializedObject.targetObject as ParticlePooler;
		if(GUILayout.Button("Debug Log Queue")){
			pp.DebugLogQueue();
		}
		if(GUILayout.Button("Debug Log List")){
			pp.DebugLogList();
		}

		DrawDefaultInspector();
	}
	
}
