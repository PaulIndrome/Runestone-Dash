using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCountUI : MonoBehaviour {

	Text comboText;

	// Use this for initialization
	void Start () {
		comboText = GetComponent<Text>();
		PlayerState.comboCountChangeEvent += UpdateComboText;
	}
	
	void UpdateComboText(int newValue){
		comboText.text = "" + newValue;
	}

	void OnDestroy(){
		PlayerState.comboCountChangeEvent += UpdateComboText;
	}
}
