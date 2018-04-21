using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
	[HideInInspector] public List<Enemy> enemies, bosses;
	public bool enemySpawnActive = true;
	[Header("Spawncontrol numbers")]
	[Tooltip("enemy spawn radius (min, max) from origin")]
	[SerializeField] float minEnemySpawnRadius;
	[SerializeField] float maxEnemySpawnRadius = 20;

	[Tooltip("maximum amount of enemies and bosses simultaneously active")]
	public int numEnemyMax;
	[Tooltip("hold bosses until at least X enemies have been killed")]
	public int minBossWave; 
	[Tooltip("spawn a boss after X enemies")]
	public int countDownToBoss;
	[Tooltip("increase maximum amount of enemies after every Xth boss")]
	public int maxEnemyIncreaseRate = 4;
	int numEnemyToSpawn = 1;
	int bossWave = 0;
	int numEnemyTypes, numBossTypes, numEnemyCurvePaths;
	int numEnemiesKilled;
	int nextBossSpawnAt;
	bool bossesSpawning = false;
	bool bossesAlive = false;
	EnemyChainKill enemyChainKill;


	[Header("Direct references")]
	[SerializeField] RectTransform healthBarCanvas;
	[SerializeField] GameObject standardEnemyPrefab;
	[Header("Normal Enemy Types")]
	[SerializeField] List<EnemyType> enemyTypes;
	[Header("Boss Enemy Types")]
	[SerializeField] List<EnemyType> bossTypes;
	[Header("Enemy Paths")]
	[SerializeField] List<EnemyCurvePath> enemyCurvePaths;
	
	public void Start(){
		enemies = new List<Enemy>();
		bosses = new List<Enemy>();

		enemyChainKill = GetComponent<EnemyChainKill>();

		//in case the spawn already has some enemies
		foreach(Transform child in transform){
			Enemy enemyChild = child.GetComponent<Enemy>();
			if(enemyChild == null) continue;
			else{
				if(!enemyChild.enemyType.isBossType){
					enemies.Add(enemyChild);
					enemyChild.SetupEnemy(healthBarCanvas);
				} else {
					bosses.Add(enemyChild);
					enemyChild.SetupEnemy(healthBarCanvas);
				}
			}
		}

		ClearEnemyTypeLists();

		numEnemyToSpawn = 1;
		numEnemiesKilled = 0;

		numEnemyTypes = enemyTypes.Count;
		numBossTypes = bossTypes.Count;
		numEnemyCurvePaths = enemyCurvePaths.Count;
		nextBossSpawnAt = minBossWave;

		PlayerState.comboCountChangeEvent += ListenForCombo;

		Invoke("StartEnemySpawn", 2f);
	}

	void ClearEnemyTypeLists(){
		//remove non-BossTypes from the bossType list
		for(int i = bossTypes.Count-1; i>=0; i--){
			if(!bossTypes[i].isBossType) bossTypes.RemoveAt(i);
		}
		//remove BossTypes from the enemyType list
		for(int i = enemyTypes.Count-1; i>=0; i--){
			if(enemyTypes[i].isBossType) enemyTypes.RemoveAt(i);
		}
	}

	public void StartEnemySpawn(){
		StartCoroutine(EnemySpawnCycle());
	}

	public void RemoveEnemy(Enemy enemyToRemove){
		if(!enemyToRemove.enemyType.isBossType){
			enemies.Remove(enemyToRemove);
			numEnemiesKilled++;
			return;
		} else {
			bosses.Remove(enemyToRemove);
			numEnemiesKilled++;
			if(bosses.Count <= 0){
				bossesAlive = false;
				nextBossSpawnAt = numEnemiesKilled + countDownToBoss;
				return;
			}
			return;
		}
	}

	void ListenForCombo(int comboCount, int maxCombo){
		if(comboCount >= maxCombo){
			enemySpawnActive = false;
			List<Enemy> allEnemies = new List<Enemy>();
			allEnemies.AddRange(enemies);
			allEnemies.AddRange(bosses);
			enemyChainKill.ChainKillAllEnemies(allEnemies);
		}
	}

	void OnDestroy(){
		PlayerState.comboCountChangeEvent -= ListenForCombo;
	}
	IEnumerator EnemySpawnCycle(){
		while(enemySpawnActive){
			if(enemies.Count + bosses.Count < numEnemyMax && !bossesSpawning){
				if(!bossesAlive && nextBossSpawnAt == numEnemiesKilled){
					StartCoroutine(GenerateBossWave());
				} else {
					GenerateRandomEnemy();
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
	}

	public void GenerateRandomEnemy(){
		//generate a new spawn position
		Vector2 randomOnCircle = Random.insideUnitCircle.normalized;
		Vector3 position = new Vector3(randomOnCircle.x, 0, randomOnCircle.y) * (Random.Range(minEnemySpawnRadius, maxEnemySpawnRadius));

		//instantiate a new enemy, parent it to the spawn
		Enemy newEnemy = Instantiate(standardEnemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
		newEnemy.gameObject.transform.SetParent(transform);

		//choose a new random type and path
		EnemyType randomType = enemyTypes[Random.Range(0,numEnemyTypes)];
		EnemyCurvePath randomPath = enemyCurvePaths[Random.Range(0, numEnemyCurvePaths)];

		//set up the enemy with the type and path
		newEnemy.SetupEnemy(randomType, randomPath);
		//create a new healthBar and associate it with the new enemy
		newEnemy.SetupBars(healthBarCanvas);

		//name the enemy gameobject using his properties' shorthand designations
		newEnemy.gameObject.name = numEnemyToSpawn + " - " + randomType.GetShortHand();

		//add the enemy to the list
		enemies.Add(newEnemy);

		//increment the enemy number
		numEnemyToSpawn++;
	}

	IEnumerator GenerateBossWave(){
		bossWave++;
		bossesSpawning = true;
		bossesAlive = true;
		int bossesToSpawn = (bossWave + 1) / 2;
		for(int i = 0; i < bossesToSpawn; i++){
			Vector2 randomOnCircle = Random.insideUnitCircle.normalized;
			Vector3 position = new Vector3(randomOnCircle.x, 0, randomOnCircle.y) * (Random.Range(maxEnemySpawnRadius-1f, maxEnemySpawnRadius));

			Enemy newBoss = Instantiate(standardEnemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
			newBoss.gameObject.transform.SetParent(transform);

			EnemyType randomBossType = bossTypes[Random.Range(0,numBossTypes)];
			EnemyCurvePath randomPath = enemyCurvePaths[Random.Range(0, numEnemyCurvePaths)];

			newBoss.SetupEnemy(randomBossType, randomPath);
			HealthBar bossBar = newBoss.SetupBars(healthBarCanvas);

			Color bossBarColor = Color.magenta;
			bossBarColor.a = 0.8f;
			bossBar.ChangeBarColorTo(bossBarColor);
			bossBarColor.a = 0.95f;
			bossBar.ChangeBarEndColorTo(bossBarColor);

			newBoss.gameObject.name = "B" + numEnemyToSpawn + " - " + randomBossType.GetShortHand();

			bosses.Add(newBoss);

			numEnemyToSpawn++;
			yield return new WaitForSeconds(1.0f);
		}
		numEnemyMax += (bossWave % maxEnemyIncreaseRate == 0) ? 1 : 0;
		bossesSpawning = false;
	}
}