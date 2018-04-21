using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerState))]
public class PlayerStateCustomEditor : Editor {

	PlayerState playerState;
	public override void OnInspectorGUI(){
		playerState = serializedObject.targetObject as PlayerState;
		if(GUILayout.Button("Chain Kill All Enemies")){
			playerState.CurrentCombo = playerState.maxCombo;
		}
		DrawDefaultInspector();
	}
	
}
