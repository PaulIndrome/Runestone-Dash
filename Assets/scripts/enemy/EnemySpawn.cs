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

	[Tooltip("maximum amount of enemies ")]
	public int numEnemyMax;
	public int minBossWave; 
	public int bossModulo;
	int enemyNo = 1;
	int bossWave = 0;
	int numEnemyTypes, numBossTypes, numEnemyCurvePaths;
	bool bossesSpawning = false;
	bool bossesAlive = false;

	[Header("Direct references")]
	[SerializeField] RectTransform healthBarCanvas;
	[SerializeField] private GameObject standardEnemyPrefab;
	[Header("Normal Enemy Types")]
	[SerializeField] List<EnemyType> enemyTypes;
	[Header("Boss Enemy Types")]
	[SerializeField] List<EnemyType> bossTypes;
	[Header("Enemy Paths")]
	[SerializeField] List<EnemyCurvePath> enemyCurvePaths;
	
	public void Start(){
		enemies = new List<Enemy>();
		bosses = new List<Enemy>();

		//in case the spawn already has some enemies
		foreach(Transform child in transform){
			Enemy enemyChild = child.GetComponent<Enemy>();
			if(enemyChild == null) continue;
			else{
				enemies.Add(enemyChild);
				enemyChild.SetupEnemy(healthBarCanvas);
			}
		}

		ClearEnemyTypeLists();

		enemyNo = 1;
		numEnemyTypes = enemyTypes.Count;
		numBossTypes = bossTypes.Count;
		numEnemyCurvePaths = enemyCurvePaths.Count;

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
		if(!enemyToRemove.enemyType.isBossType)
			enemies.Remove(enemyToRemove);
		else {
			bosses.Remove(enemyToRemove);
			if(bosses.Count == 0)
				bossesAlive = false;
		}
	}

	IEnumerator EnemySpawnCycle(){
		while(enemySpawnActive){
			if(enemies.Count + bosses.Count < numEnemyMax && !bossesSpawning){
				if(!bossesAlive && enemyNo >= minBossWave && enemyNo % bossModulo == 0){
					StartCoroutine(GenerateBoss());
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
		newEnemy.gameObject.name = enemyNo + " - " + randomType.GetShortHand();

		//add the enemy to the list
		enemies.Add(newEnemy);

		//increment the enemy number
		enemyNo++;
	}

	IEnumerator GenerateBoss(){
		bossWave++;
		bossesSpawning = true;
		bossesAlive = true;

		for(int i = 0; i < bossWave; i++){
			Vector2 randomOnCircle = Random.insideUnitCircle.normalized;
			Vector3 position = new Vector3(randomOnCircle.x, 0, randomOnCircle.y) * (Random.Range(maxEnemySpawnRadius-1f, maxEnemySpawnRadius));

			Enemy newBoss = Instantiate(standardEnemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
			newBoss.gameObject.transform.SetParent(transform);

			EnemyType randomBossType = bossTypes[Random.Range(0,numBossTypes)];
			EnemyCurvePath randomPath = enemyCurvePaths[Random.Range(0, numEnemyCurvePaths)];

			newBoss.SetupEnemy(randomBossType, randomPath);
			HealthBar bossBar = newBoss.SetupBars(healthBarCanvas);
			bossBar.ChangeBarColorTo(Color.magenta);

			newBoss.gameObject.name = "B" + enemyNo + " - " + randomBossType.GetShortHand();

			bosses.Add(newBoss);

			enemyNo++;
			yield return new WaitForSeconds(1.0f);
		}

		numEnemyMax += (bossWave % 3 == 0) ? 1 : 0;

		bossesSpawning = false;
	}

}
