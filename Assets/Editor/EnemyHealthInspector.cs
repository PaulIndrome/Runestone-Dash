using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyHealth))]
public class EnemyHealthInspector : Editor {

	public override void OnInspectorGUI(){
		SerializedProperty maxHealthProperty = serializedObject.FindProperty("MaxHealth");
		SerializedProperty currentHealthProperty = serializedObject.FindProperty("CurrentHealth");

		EnemyHealth enemyHealth = target as EnemyHealth;

		Rect healthBarRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth * 0.6f, 20f);

		EditorGUI.ProgressBar(healthBarRect, enemyHealth.CurrentHealth / enemyHealth.MaxHealth, "health");

		DrawDefaultInspector();
	}

}
