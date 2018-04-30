using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this script sits on the object that intercepts the player input
//hence, the big collision box under the ground
public class PlayerClickToDash : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
	PlayerDashChaining playerDashChaining;
	Player player;
	[SerializeField] [Range(0.1f, 1.5f)] private float delayBetweenDashes;
	[HideInInspector] public bool recentlyDashed = false;

	public void Start(){
		playerDashChaining = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDashChaining>();
		player = playerDashChaining.gameObject.GetComponent<Player>();
	}

	public void OnPointerDown(PointerEventData ped){
	}

	public void OnPointerClick(PointerEventData ped){
		
	}

	public void OnPointerUp(PointerEventData ped){
		if(!player.playerState.isLegendary){
			playerDashChaining.StashTarget(ped.pointerCurrentRaycast.worldPosition);
			StartCoroutine(ResetRecentlyDashed());
		}
	}

	IEnumerator ResetRecentlyDashed(){
		recentlyDashed = true;
		yield return new WaitForSeconds(delayBetweenDashes);
		recentlyDashed = false;
	}

}
