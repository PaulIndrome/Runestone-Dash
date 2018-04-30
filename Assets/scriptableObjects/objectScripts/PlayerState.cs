using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the MAIN ScriptableObject that contains all of the player character's dynamic attributes during a play session
//gets instantiated on game start and destroyed on gam end or restart
public class PlayerState : ScriptableObject {

	public delegate void ComboCountChange(int currentComboValue, int maxComboValue);
	//any combo count change is broadcast to a few key scripts on a need-to-know basis (f.e. EnemySpawn)
	public static event ComboCountChange comboCountChangeEvent;

	public float currentDamage = 1;
	public int maxDamage = 4;
	public int damageIncreaseRatio = 20; //increase the player's damage every X combo hits
	public int maxCombo;
	public bool canMove = true;
	public bool canDash = true;
	public bool hitEnemyShield = false;
	public bool isDashing = false;
	public bool isLegendary = false; //set to true when the combocount reaches maxCombo, used for mayhem

	int currentCombo = 0;
	//the current combo property fires the combo count change event whenever it is changed
	public int CurrentCombo {
		get { return currentCombo; }
		set {
			currentCombo = Mathf.Clamp(value, 0, maxCombo);
			if(currentCombo > 0f && !isLegendary) {
				nextComboResetTime = Time.time + comboResetSeconds;
				//the damage the player does gets increased the higher his combo count is
				currentDamage += (currentCombo % damageIncreaseRatio == 0) ? 1f : 0f;
			} else {
				currentDamage = 1f;
			}
			//the combo change event always broadcasts the current combo count and the combo count needed to win the game
			//this means the maximum combo count needed to win can be easily tweaked for development
			if(comboCountChangeEvent != null) comboCountChangeEvent(currentCombo, maxCombo);

			currentDamage = Mathf.Clamp(currentDamage, 1, isLegendary ? 1000 : maxDamage);
		}
	}

	public float comboResetSeconds = 3f;
	public float nextComboResetTime = 0;

	void Awake(){
		maxCombo = maxDamage * damageIncreaseRatio;
		CurrentCombo = 0;
	}

	public void StartComboResetter(MonoBehaviour runner){
		runner.StartCoroutine(ComboResetter());
	}

	IEnumerator ComboResetter(){
		while(!EnemyDestroysRunestone.gameOver){
			if(Time.time >= nextComboResetTime && !isLegendary)
				CurrentCombo = 0;
			
			//for performance and semi-random fairness purposes the reset timer for the 
			//combo count only ticks every 0.5 seconds, that means the actual combo reset time 
			//ranges from X to X+(0.5 - Time.deltatime) 
			yield return new WaitForSecondsRealtime(0.5f);	
		}
	}

}
