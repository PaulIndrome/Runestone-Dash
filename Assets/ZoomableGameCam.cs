using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ZoomableGameCam : MonoBehaviour {

	CinemachineVirtualCamera zoomableCam;
	CinemachineGroupComposer cgc;
	[SerializeField] Slider slider;
	[SerializeField] float initialFOV = 35f, minMaxRange = 10f;
	float minFOV = 25f, maxFOV = 45f;

	void Start(){
		zoomableCam = GetComponent<CinemachineVirtualCamera>();
		cgc = zoomableCam.GetCinemachineComponent<CinemachineGroupComposer>();
		
		minFOV = initialFOV - minMaxRange;
		maxFOV = initialFOV + minMaxRange;

		slider.value = (initialFOV - minFOV) / (maxFOV - minFOV);
		cgc.m_MinimumFOV = initialFOV;
	}

	public void AdjustFOV(float newValue){
		newValue = Mathf.Clamp01(newValue);
		cgc.m_MinimumFOV = Mathf.Lerp(minFOV, maxFOV, newValue);
	}
}
