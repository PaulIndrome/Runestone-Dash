using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickToDash : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
	private PlayerDashChaining playerDashChaining;
	[SerializeField] [Range(0.1f, 1.5f)] private float delayBetweenDashes;
	[HideInInspector] public bool recentlyDashed = false;

	public void Start(){
		playerDashChaining = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDashChaining>();
	}

	public void OnPointerDown(PointerEventData ped){
	}

	public void OnPointerClick(PointerEventData ped){
		
	}

	public void OnPointerUp(PointerEventData ped){
		//if(recentlyDashed) return;
		playerDashChaining.StashTarget(ped.pointerCurrentRaycast.worldPosition);
		StartCoroutine(ResetRecentlyDashed());
	}

	IEnumerator ResetRecentlyDashed(){
		recentlyDashed = true;
		yield return new WaitForSeconds(delayBetweenDashes);
		recentlyDashed = false;
	}

}
