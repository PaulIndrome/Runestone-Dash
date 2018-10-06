using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour {

	[SerializeField] float LerpToStopTime = 2f;
	[SerializeField] Cinemachine.CinemachineVirtualCamera menuCam;
	[SerializeField] TimeLerper timeLerper;
	
	Coroutine timeScaleCheck;

	// OnEnable is called on gameObject enable
	void OnEnable(){
		menuCam.Priority = 1000;

		timeLerper.LerpToZero(LerpToStopTime);

		if(timeScaleCheck != null) StopCoroutine(timeScaleCheck);
		timeScaleCheck = StartCoroutine(CheckTimeScale(LerpToStopTime));

		//the use of CineMachine discourages using a timescale of 0 because a couple of 
		//divide-by-zero exceptions aren't properly caught in its current version
	}
	
	public void ResumeGame(){
		if(timeScaleCheck != null) StopCoroutine(timeScaleCheck);
		
		timeLerper.LerpTime(Time.timeScale, 1f, 4f);
		gameObject.SetActive(false);
	}
	
	public void RestartGame(){
		SceneManager.LoadScene("ingame_01");
	}
	
	public void QuitGame(){
		if(timeScaleCheck != null) StopCoroutine(timeScaleCheck);
		Application.Quit();
	}

	void OnDisable(){
		menuCam.Priority = 0;
		if(timeScaleCheck != null) StopCoroutine(timeScaleCheck);
	}

	IEnumerator CheckTimeScale(float startDelay){
		yield return new WaitForSecondsRealtime(startDelay);

		while(gameObject.activeSelf){
			Time.timeScale = 0.000f;
			yield return new WaitForSecondsRealtime(0.5f);
		}
	}

}
