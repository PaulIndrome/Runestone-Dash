using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaginataControl : MonoBehaviour {

	[SerializeField] Material bladeTrailMaterial;
	[SerializeField] Color lowComboColor = Color.cyan, highComboColor = Color.red;
	ParticleSystem bladeTrailPS;
	Player player;

	void Start(){
		bladeTrailPS = GetComponent<ParticleSystem>();
		player = GetComponentInParent<Player>();

		bladeTrailMaterial.SetColor("_EmisColor", lowComboColor);

		PlayerState.comboCountChangeEvent += LerpTrailColor;
	}

	public void StartBladeTrail(){
		bladeTrailPS.Play();
	}

	public void StopBladeTrail(){
		bladeTrailPS.Stop();
	}

	public void ClearBladeTrail(){
		StopBladeTrail();
		bladeTrailPS.Clear(true);
	}

	void LerpTrailColor(int comboCount, int maxCombo){
		Color nextColor = Color.Lerp(lowComboColor, highComboColor, (float)comboCount / (float)maxCombo);
		bladeTrailMaterial.SetColor("_EmisColor", nextColor);
	}

	void OnDestroy(){
		PlayerState.comboCountChangeEvent -= LerpTrailColor;
	}
}
