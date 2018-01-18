using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCountButton : MonoBehaviour {

	public EnemySpawn enemySpawn;
	public Text enemyCount;
	
	void Start(){
		enemyCount = GetComponentInChildren<Text>();
		enemyCount.text = "enemies: " + enemySpawn.numEnemyMax;
	}

	public void IncreaseEnemies(){
		enemySpawn.numEnemyMax += 1;
		enemyCount.text = "enemies: " + enemySpawn.numEnemyMax;
	}

	public void DecreaseEnemies(){
		enemySpawn.numEnemyMax -= 1;
		enemyCount.text = "enemies: " + enemySpawn.numEnemyMax;
	}

}
