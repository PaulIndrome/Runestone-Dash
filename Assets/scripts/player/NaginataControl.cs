using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaginataControl : MonoBehaviour {

	[SerializeField] Material bladeTrailMaterial;
	[SerializeField] Color lowComboColor = Color.cyan, highComboColor = Color.red;
	ParticleSystem bladeTrailPS;
	Player player;

	int maxPlayerDamage, damageIncreaseRatio, comboForMaxDamage;

	void Start(){
		bladeTrailPS = GetComponent<ParticleSystem>();
		player = GetComponentInParent<Player>();

		bladeTrailMaterial.SetColor("_EmisColor", lowComboColor);

		maxPlayerDamage = player.playerState.maxDamage;
		damageIncreaseRatio = player.playerState.damageIncreaseRatio;
		comboForMaxDamage = maxPlayerDamage * damageIncreaseRatio;

		PlayerState.comboCountChangeEvent += LerpTrailColor;
	}

	public void StartBladeTrail(){
		bladeTrailPS.Play();
	}

	public void StopBladeTrail(){
		bladeTrailPS.Stop();
	}

	void LerpTrailColor(int comboCount){
		Color nextColor = Color.Lerp(lowComboColor, highComboColor, (float)comboCount / (float)comboForMaxDamage);
		bladeTrailMaterial.SetColor("_EmisColor", nextColor);
	}

}
