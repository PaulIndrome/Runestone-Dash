using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyDestroysRunestone : MonoBehaviour {

	public static bool gameOver = false;

	public Cinemachine.CinemachineVirtualCamera menuCam;
	public RectTransform gameOverTextsParent;
	public Text restartInText;
	public Text winLooseText;
	
	PlayerClickToDash playerClickToDash;
	PlayerTargetLineControl playerTargetLineControl;

	void Start(){
		playerClickToDash = GetComponentInChildren<PlayerClickToDash>();
		playerTargetLineControl = GetComponentInChildren<PlayerTargetLineControl>();
	}

	public void OnCollisionEnter(Collision collision){
		if(!collision.gameObject.CompareTag("Player")){
			StartCoroutine(GameOverAndRestart(false));
		} 
	}

	public void WinOrLoose(bool winLoose){
		StartCoroutine(GameOverAndRestart(winLoose));
	}

	//coroutine to restart game regardless of win or loose scenario, true means win
	IEnumerator GameOverAndRestart(bool winLoose){ 
		menuCam.Priority = 100;
		int secondsToRestart = 5;
		Time.timeScale = 0.005f;

		playerClickToDash.enabled = playerTargetLineControl.enabled = false;

		gameOver = true;

		if(winLoose){
			winLooseText.text = "The Runestone has been defended!\nYou have become legend!";
		}

		//wait a few seconds before starting the countdown
		yield return new WaitForSecondsRealtime(2.0f);
		gameOverTextsParent.gameObject.SetActive(true);
		restartInText.enabled = true;
		for(int i = secondsToRestart; i>0;i--){
			restartInText.text = "Restarting in: " + i;
			yield return new WaitForSecondsRealtime(1.0f);
		}
		Time.timeScale = 1f;
		menuCam.Priority = 9;
		gameOver = false;
		SceneManager.LoadScene("ingame_01");
	}
}
