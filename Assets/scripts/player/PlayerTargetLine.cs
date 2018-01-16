using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTargetLine : MonoBehaviour { 

	private LineRenderer targetLine;
	private PlayerDashRadius playerDashRadius;
	private Camera mainCam;
	public LayerMask layerMask;
	private RaycastHit raycastHit;
	[HideInInspector] public bool isPointerDown;
	[SerializeField] private float targetLineStartWidth, targetLineEndWidth;

	private Color targetLineColor;

	public void Start(){
		playerDashRadius = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerDashRadius>();
		if(playerDashRadius == null) Debug.LogError("no playerDAshRadius component found");
		mainCam = Camera.main;
		targetLine = GetComponent<LineRenderer>();
		targetLineColor = targetLine.material.color;
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
		while(isPointerDown){
			targetLine.SetPosition(0, playerDashRadius.transform.position);
			Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, layerMask.value);
			endPos = (raycastHit.point - targetLine.GetPosition(0));
			endPos.y = 0;
			endPos = targetLine.GetPosition(0) + endPos.normalized * playerDashRadius.dashRadius;
			targetLine.SetPosition(1, endPos);
			yield return null;
		}
		StartCoroutine(FadeOutTargetLine());
		yield return null;
	}

	IEnumerator FadeOutTargetLine(){
		float timer = 0;
		targetLine.startWidth = targetLineEndWidth;
		targetLine.endWidth = targetLineStartWidth * 2f;
		while(timer <= 1f && !isPointerDown){
			targetLine.material.color = Color.Lerp(targetLineColor, Color.clear, timer);
			targetLine.startWidth = Mathf.Lerp(targetLineEndWidth, 0f, timer);
			targetLine.endWidth = Mathf.Lerp(targetLineStartWidth*2f, 0f, timer);
			timer += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

}
