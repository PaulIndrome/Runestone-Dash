using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyDestroysRunestone : MonoBehaviour {

	public static bool gameOver = false;

	public Cinemachine.CinemachineVirtualCamera virtualCamera;
	public Canvas gameOverCanvas;
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
		virtualCamera.enabled = false;
		int secondsToRestart = 5;
		Time.timeScale = 0f;

		playerClickToDash.enabled = playerTargetLineControl.enabled = false;

		gameOver = true;

		if(winLoose){
			winLooseText.text = "The Runestone has been defended!";
		}
		gameOverCanvas.gameObject.SetActive(true);
		yield return new WaitForSecondsRealtime(1.0f);
		restartInText.enabled = true;
		for(int i = secondsToRestart; i>0;i--){
			restartInText.text = "Restarting in: " + i;
			yield return new WaitForSecondsRealtime(1.0f);
		}
		Time.timeScale = 1f;
		virtualCamera.enabled = true;
		gameOver = false;
		SceneManager.LoadScene("ingame_01");
	}
}
