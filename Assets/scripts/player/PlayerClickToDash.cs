using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickToDash : MonoBehaviour, IPointerClickHandler, IPointerDownHandler {
	private PlayerDash playerDash;
	[SerializeField] [Range(0.1f, 1.5f)] private float delayBetweenDashes;
	private bool recentlyDashed = false;

	public void Start(){
		playerDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>();
	}
	public void OnPointerDown(PointerEventData ped){
	}

	public void OnPointerClick(PointerEventData ped){
		if(recentlyDashed) return;
		playerDash.ClickToDash(ped);
		StartCoroutine(ResetRecentlyDashed());
	}

	IEnumerator ResetRecentlyDashed(){
		recentlyDashed = true;
		yield return new WaitForSeconds(delayBetweenDashes);
		recentlyDashed = false;
	}

}
