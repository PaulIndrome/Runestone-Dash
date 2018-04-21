using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomPathTester))]
public class RandomPathTesterEditor : Editor {

	RandomPathTester randomPathTester;

	public void OnEnable(){
		randomPathTester = serializedObject.targetObject as RandomPathTester;
	}
	
	public override void OnInspectorGUI(){
		if(GUILayout.Button("Create Path")){
			randomPathTester.CreateRandomPath();
			float H, S, V;
			Color.RGBToHSV(randomPathTester.startColor, out H, out S, out V);
			H = Mathf.Repeat(H + Random.Range(0.165f, 0.223f), 1);
			randomPathTester.startColor = Color.HSVToRGB(H, 1, 1);
		}
		if(GUILayout.Button("Create Path Coroutine")){
			randomPathTester.StartRandomPathCreationCoroutine();
			float H, S, V;
			Color.RGBToHSV(randomPathTester.startColor, out H, out S, out V);
			H = Mathf.Repeat(H + Random.Range(0.165f, 0.223f), 1);
			randomPathTester.startColor = Color.HSVToRGB(H, 1, 1);
		}
		DrawDefaultInspector();
	}
}
