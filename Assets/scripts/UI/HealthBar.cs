using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public Transform targetEnemy;

	public RectTransform redBar;

	public void SetTarget(Transform targetTransform){
		targetEnemy = targetTransform;
	}

	public void RepositionToTarget(Vector3 screenPosition){
		transform.position = screenPosition;
	}

	public void SetRedBarTo(float healthPercent){
		redBar.localScale = new Vector3(healthPercent, 1, 1);
	}
}
