using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyDestroysRunestone : MonoBehaviour {

	public Canvas gameOverCanvas;
	public Text restartInText;
	public Text winLooseText;

	public PlayerState playerState;
	public void OnCollisionEnter(Collision collision){
		if(!collision.gameObject.CompareTag("Player")){
			StartCoroutine(GameOverAndRestart(false));
		}
	}

	public void RuneStoneDestroyed(bool winLoose){
		StartCoroutine(GameOverAndRestart(winLoose));
	}

	IEnumerator GameOverAndRestart(bool winLoose){
		playerState.currentlyDashing = true;
		int secondsToRestart = 5;
		Time.timeScale = 0f;
		if(winLoose){
			winLooseText.text = "The Runestone has been defended!";
		}
		gameOverCanvas.gameObject.SetActive(true);
		yield return new WaitForSecondsRealtime(1.0f);
		while(secondsToRestart >= 0){
			restartInText.enabled = true;
			restartInText.text = "Restarting in: " + secondsToRestart;
			secondsToRestart--;
			yield return new WaitForSecondsRealtime(1.0f);
		}
		Time.timeScale = 1f;
		playerState.currentlyDashing = false;
		SceneManager.LoadScene("ingame_01");
	}
}
