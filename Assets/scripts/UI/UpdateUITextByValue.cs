using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateUITextByValue : MonoBehaviour {

	Text text;
	void Start(){
		if(text == null) text = GetComponent<Text>();
	}

	public void ChangeTextToSliderValue(float value){
		if(text == null) text = GetComponent<Text>();
		text.text = value.ToString("F1");
	}

}
