using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldPiece : MonoBehaviour {

	Material material;
	MeshRenderer meshRenderer;
	public AnimationCurve fadeOutCurve;
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
		material = meshRenderer.material;
	}
	
	public void StartFadeOut(float time){
		StartCoroutine(FadeOutShieldPieces(time));
	}

	IEnumerator FadeOutShieldPieces(float fadeOutTime){
		float timer = 0;
		Color clearColor = material.color;
		clearColor.a = 0f;
		while(timer <= fadeOutTime){
			material.color = Color.Lerp(material.color, clearColor, fadeOutCurve.Evaluate(timer/fadeOutTime));
			timer += Time.deltaTime;
			yield return null;
		}
		material.color = Color.clear;
		meshRenderer.enabled = false;
		gameObject.SetActive(false);
		yield return null;
	}
}
