using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyAttribute), true)]
public class EnemyAttributeEditor : Editor {

	EnemyAttribute enemyAttribute;

	public void OnEnable(){
	}
	public override void OnInspectorGUI(){
		enemyAttribute = serializedObject.targetObject as EnemyAttribute;

		if(enemyAttribute.values.Count > 0){
			foreach(IEnemyValue iev in enemyAttribute.values){
				EnemyValue<int> intval = iev as EnemyValue<int>;
				if(intval != null){
					EditorGUILayout.SelectableLabel("" + intval.Value);
					EditorGUILayout.IntField(intval.Value);
					continue;
				}
				EnemyValue<float> floatval = iev as EnemyValue<float>;
				if(floatval != null){
					EditorGUILayout.SelectableLabel("" + floatval.Value);
					EditorGUILayout.FloatField(floatval.Value);
					continue;
				}
				EnemyValue<string> stringval = iev as EnemyValue<string>;
				if(stringval != null){
					EditorGUILayout.SelectableLabel("" + stringval.Value);
					EditorGUILayout.TextField(stringval.Value);
					continue;
				}
			}
		} 

		DrawDefaultInspector();
		if(GUILayout.Button("Swap all attributes")){
			enemyAttribute.SwapAllAttributes(enemyAttribute.swapWith);
		}

		serializedObject.ApplyModifiedProperties();
	}

}
