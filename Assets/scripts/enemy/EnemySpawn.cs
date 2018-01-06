using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
	public List<Enemy> enemies;
	public bool enemySpawnActive = true;
	public float enemySpawnRadius;
	public int numEnemyMax;

	[SerializeField] private GameObject standardEnemyPrefab;
	[SerializeField] private EnemyType[] enemyTypes;
	[SerializeField] private EnemyCurvePath[] enemyCurvePaths;
	
	public void Start(){
		enemies = new List<Enemy>();

		//in case the spawn already has some enemies
		foreach(Transform child in transform){
			Enemy enemyChild = child.GetComponent<Enemy>();
			if(enemyChild == null) continue;
			else{
				enemies.Add(enemyChild);
				enemyChild.SetupEnemy();
			}
		}

		Invoke("StartEnemySpawn", 2f);
	}

	public void StartEnemySpawn(){
		StartCoroutine(EnemySpawnCycle());
	}

	public void RemoveEnemy(Enemy enemyToRemove){
		enemies.Remove(enemyToRemove);
	}

	IEnumerator EnemySpawnCycle(){
		Vector2 randomOnCircle;
		Vector3 position = Vector3.zero;
		int numEnemyTypes = enemyTypes.Length;
		int numEnemyCurvePaths = enemyCurvePaths.Length;
		while(enemySpawnActive){
			if(GetComponentsInChildren<Enemy>().Length < numEnemyMax){
				randomOnCircle = Random.insideUnitCircle.normalized;
				position = new Vector3(randomOnCircle.x, 0, randomOnCircle.y) * (enemySpawnRadius+Random.Range(-5,1));
				Enemy newEnemy = Instantiate(standardEnemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
				newEnemy.gameObject.transform.SetParent(transform);
				newEnemy.SetupEnemy(enemyTypes[Random.Range(0,numEnemyTypes)], enemyCurvePaths[Random.Range(0, numEnemyCurvePaths)]);
				enemies.Add(newEnemy);
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
	}
}
