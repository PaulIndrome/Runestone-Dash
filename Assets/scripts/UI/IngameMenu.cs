using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour {

	[SerializeField] Cinemachine.CinemachineVirtualCamera menuCam;

	void OnEnable(){
		menuCam.Priority = 1000;
		Time.timeScale = 0.01f;
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
