using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class optionsMenu : MonoBehaviour {

	[SerializeField] AudioMixer mainMixer;

	Dictionary<string, bool> currentParameterBools;

	void Start(){
		currentParameterBools = new Dictionary<string, bool>();
	}

	public void toggleVolume(string parameterName){
		float currentVolume;
		if(!mainMixer.GetFloat(parameterName, out currentVolume)) return;

		if(currentParameterBools.ContainsKey(parameterName)){
			if(currentParameterBools[parameterName]){
				mainMixer.SetFloat(parameterName, -80f);
			} else {
				mainMixer.SetFloat(parameterName, 0f);
			}
			currentParameterBools[parameterName] = !currentParameterBools[parameterName];
		} else {
			if(currentVolume < -10f){
				currentParameterBools.Add(parameterName, true);
				mainMixer.SetFloat(parameterName, 0f);
			} else {
				currentParameterBools.Add(parameterName, false);
				mainMixer.SetFloat(parameterName, -80f);
			}

		}
	}

	void OnDestroy(){
		foreach (string key in currentParameterBools.Keys)
		{
			mainMixer.ClearFloat(key);
		}
	}
}
