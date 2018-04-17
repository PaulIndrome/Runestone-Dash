using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour {

	void OnEnable(){
		Time.timeScale = 0f;
	}
	
	public void ResumeGame(){
		gameObject.SetActive(false);
	}
	
	public void RestartGame(){
		SceneManager.LoadScene("ingame_01");
	}
	
	public void QuitGame(){
		Application.Quit();
	}

	void OnDisable(){
		Time.timeScale = 1f;
	}
}
