using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this script draws the targetting line whenever the pointer is held down
public class PlayerTargetLine : MonoBehaviour { 
	[HideInInspector] public bool isPointerDown;
	[SerializeField] private float targetLineStartWidth, targetLineEndWidth;
	private float dashRadius;
	private LineRenderer targetLine;
	private PlayerDashChaining playerDashChaining;
	private Camera mainCam;
	public LayerMask layerMask;
	private RaycastHit raycastHit;
	private Color targetLineColor;

	public void Start(){
		playerDashChaining = GetComponentInParent<PlayerDashChaining>();
		if(playerDashChaining == null) Debug.LogError("no playerDAshRadius component found");
		mainCam = Camera.main;
		targetLine = GetComponent<LineRenderer>();
		targetLineColor = targetLine.material.color;

		dashRadius = playerDashChaining.dashRadius;
	}

	public void StartLineDrawing(){
		StopCoroutine(FadeOutTargetLine());
		StartCoroutine(RepositionLineStartPos());
	}

	IEnumerator RepositionLineStartPos(){
		targetLine.material.color = targetLineColor;
		targetLine.startWidth = targetLineStartWidth;
		targetLine.endWidth = targetLineEndWidth;
		Vector3 endPos;

		//the diminished timescale can be used to strategical effect... though currently the game
		//fails in making that apparent enough
		Time.timeScale = 0.33f;

		//this coroutine keeps running while the player presses down any type of pointer (touch or mouse)
		while(isPointerDown){
			targetLine.SetPosition(0, transform.position);
			//because a pointer can be both a mouse and a touch, mouse and touch are the same on mobile devices
			//which means we can just raycast through the current mouseposition to find out where to aim our 
			//target line at next
			Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, layerMask.value);
			endPos = (raycastHit.point - transform.position);
			endPos = transform.position + endPos.normalized * dashRadius;
			endPos.y = 0.2f;
			targetLine.SetPosition(1, endPos);
			yield return null;
		}
		Time.timeScale = 1f;
		StartCoroutine(FadeOutTargetLine());
		yield return null;
	}

	//because visuals...
	IEnumerator FadeOutTargetLine(){
		float timer = 0;
		targetLine.startWidth = targetLineEndWidth;
		targetLine.endWidth = targetLineStartWidth * 4f;
		while(timer <= 1f && !isPointerDown){
			targetLine.material.color = Color.Lerp(targetLineColor, Color.clear, timer);
			targetLine.startWidth = Mathf.Lerp(targetLine.startWidth, 0f, timer);
			targetLine.endWidth = Mathf.Lerp(targetLine.endWidth, 0f, timer);
			timer += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

}
