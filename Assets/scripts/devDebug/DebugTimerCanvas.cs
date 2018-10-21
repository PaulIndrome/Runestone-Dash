using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTimerCanvas : MonoBehaviour {

	float timeElapsed;

	Text text;
	void Start () {
		timeElapsed = 0;
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "" + timeElapsed.ToString("0.00");
		timeElapsed += Time.deltaTime;
	}
}
