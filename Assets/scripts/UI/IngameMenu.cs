using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour {

	[SerializeField] Cinemachine.CinemachineVirtualCamera menuCam;

	void OnEnable(){
		menuCam.Priority = 1000;
		//the use of CineMachine discourages using a timescale of 0 because a couple of 
		//divide-by-zero exceptions aren't properly caught in its current version
		Time.timeScale = 0.005f;
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
		menuCam.Priority = 0;
		Time.timeScale = 1f;
	}

}
