using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyChainKill : MonoBehaviour {

	Player player;
	PlayerDashChaining playerDashChaining;
	EnemySpawn enemySpawn;
	EnemyDestroysRunestone enemyDestroysRunestone;
	[SerializeField] Cinemachine.CinemachineVirtualCamera chainKillCam;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		playerDashChaining = player.GetComponent<PlayerDashChaining>();

		enemySpawn = GetComponent<EnemySpawn>();

		enemyDestroysRunestone = GameObject.FindGameObjectWithTag("RuneStone").GetComponent<EnemyDestroysRunestone>();
	}

	public void ChainKillAllEnemies(List<Enemy> enemyList){
		if(!player.playerState.canMove) return;

		Time.timeScale = 0;
		
		playerDashChaining.ClearTargetStash();

		player.playerState.isLegendary = true;
		player.playerState.currentDamage = 1000f;

		playerDashChaining.SetChainKillParticlesEnabled(true);

		foreach(Enemy e in enemyList){
			e.canMove = false;
		}

		for(int i = 0; i < enemyList.Count; i++){
			Enemy currentEnemy = enemyList[i];
			
			if(currentEnemy.GetEnemyHealth().hasShield){
				currentEnemy.GetEnemyShieldCollision().SetDestructible(true);
			}

			playerDashChaining.StashTarget(currentEnemy.transform.position);
		}
		chainKillCam.Priority = 12;
		Time.timeScale = 0.9f;
		this.StartCoroutine(ResetPlayerAfterChainKill());
	}

	IEnumerator ResetPlayerAfterChainKill(){
		yield return new WaitUntil(() => playerDashChaining.isTargetStashEmpty());

		playerDashChaining.SetChainKillParticlesEnabled(false);
		
		yield return new WaitForSecondsRealtime(1f);
		//player.playerState = PlayerState.CreateInstance(typeof(PlayerState)) as PlayerState;
		//enemySpawn.enemySpawnActive = true;
		chainKillCam.Priority = 8;
		enemyDestroysRunestone.WinOrLoose(true);
		
		yield break;
	}

	void OnDestroy(){
		StopAllCoroutines();
	}


}
