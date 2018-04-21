using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IEnemyValue), true)]
public class EnemyValuePropDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
		SerializedProperty value = property.FindPropertyRelative("Value");

		EditorGUI.PropertyField(position, value, GUIContent.none);
	}
	
}
