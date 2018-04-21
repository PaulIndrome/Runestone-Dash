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
			Debug.Log(transform.root.name + " has collided with " + collision.collider.transform.name);
			Debug.Break();
			StartCoroutine(GameOverAndRestart(false));
		} 
	}

	public void WinOrLoose(bool winLoose){
		StartCoroutine(GameOverAndRestart(winLoose));
	}

	IEnumerator GameOverAndRestart(bool winLoose){
		menuCam.Priority = 100;
		int secondsToRestart = 5;
		Time.timeScale = 0f;

		playerClickToDash.enabled = playerTargetLineControl.enabled = false;

		gameOver = true;

		if(winLoose){
			winLooseText.text = "The Runestone has been defended!\nYou have become legend!";
		}
		gameOverTextsParent.gameObject.SetActive(true);
		yield return new WaitForSecondsRealtime(2.0f);
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
