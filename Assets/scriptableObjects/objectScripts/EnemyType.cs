using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/EnemyType")]
public class EnemyType : ScriptableObject {

	public bool isBossType = false;

	[Range(1f, 10f)]
	public float health;
	
	[Range(0.5f, 5f)]
	public float scaleOfEnemy; 

	[Range(1f, 20f)]
	public float leftRightFluct; //multiplier for the EnemyCurvePath's Y-values

	[Range(5f, 50f)]
	public float approachTime; //how long does it take this EnemyType to reach the Runestone

	public List<EnemyEffect> enemyEffects;

	//if needed the EnemyType can be checked whether or not it contains a certain EnemyEffect type
	//however, this is unused and largely untested
	public bool ContainsType(System.Type effectType){
		foreach(EnemyEffect e in enemyEffects){
			if(e.GetType() == effectType){
				return true;
			}
		}
		return false;
	}

	//this method is used to create clear designations for the spawned enemies
	//which makes checking for errors during enemy spawning much easier
	//(it's a quality of life thing)
	public string GetShortHand(){
		int effectsAmount = enemyEffects.Count;
		string s = "h" + health + "_s" + scaleOfEnemy + "_lrf" + leftRightFluct + "_t" + approachTime + "-";
		for(int i = 0; i < effectsAmount; i++){
			s += enemyEffects[i].GetShortHand() + ((i == effectsAmount-1) ? "" : "-");
		}

		return s;
	}

}
