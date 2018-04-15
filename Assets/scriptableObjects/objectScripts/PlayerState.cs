﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerState : ScriptableObject {

	public delegate void ComboCountChange(int currentComboValue);
	public static event ComboCountChange comboCountChangeEvent;

	public float currentDamage = 1;
	public int maxDamage = 4;
	public int damageIncreaseRatio = 20;
	public bool canDash = true;
	public bool hitEnemyShield = false;
	public bool isDashing = false;
	int currentCombo = 0;
	public int CurrentCombo {
		get { return currentCombo; }
		set {
			currentCombo = value;
			currentCombo = Mathf.Clamp(currentCombo, 0, (maxDamage * damageIncreaseRatio));
			if(currentCombo > 0f) {
				nextComboResetTime = Time.time + comboResetSeconds;
				currentDamage += (currentCombo % damageIncreaseRatio == 0) ? 1f : 0f;
			} else {
				currentDamage = 1f;
			}
			if(comboCountChangeEvent != null) comboCountChangeEvent(currentCombo);

			currentDamage = Mathf.Clamp(currentDamage, 1, maxDamage);
		}
	}

	public float comboResetSeconds = 3f;
	public float nextComboResetTime = 0;
	
	public void StartComboResetter(MonoBehaviour runner){
		runner.StartCoroutine(ComboResetter());
	}

	IEnumerator ComboResetter(){
		while(!EnemyDestroysRunestone.gameOver){
			if(Time.time >= nextComboResetTime)
				CurrentCombo = 0;
			
			yield return new WaitForSecondsRealtime(0.5f);	
		}
	}
	
}
