using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;



public class MainMenuFunctions : MonoBehaviour {

	public delegate void ExampleDelegate();

	public void StartGame(float delay){
		if(delay > 0.01f)
			StartCoroutine(WaitThenCallBack(delay, StartGame));
		else
			StartGame();
	}

	private void StartGame(){
		SceneManager.LoadScene("ingame_01");
	}

	private IEnumerator WaitThenCallBack(float delay, ExampleDelegate exDel){
		yield return new WaitForSeconds(delay);
		exDel();
	}

	public void PlayAudio(){
		GetComponent<AudioSource>().Play();
	}
}
