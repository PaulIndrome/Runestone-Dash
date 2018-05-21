using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLerper : MonoBehaviour {

	Coroutine timeLerp;

	public void LerpTime(float from, float to, float time){
		if(timeLerp != null) StopCoroutine(timeLerp);
		timeLerp = StartCoroutine(LerpTimeScale(from, to, time));
	}

	public void LerpToZero(float time){
		if(timeLerp != null) StopCoroutine(timeLerp);
		timeLerp = StartCoroutine(LerpTimeScale(Time.timeScale, 0, time));
	}
	
	IEnumerator LerpTimeScale(float from, float to, float time){
		if(from < 0f || from > 1f || to < 0f || to > 1f || time < 0f) {
			Time.timeScale = 1f;
			yield break;
		}
		float t = 0;
		while(t <= time/2){
			Time.timeScale = Mathf.SmoothStep(from, to, t / time);
			t += Time.unscaledDeltaTime;
			yield return null;
		}
		Time.timeScale = to;
		timeLerp = null;
	}
}
