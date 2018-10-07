using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the main driver of the scripted victory events
public class EnemyChainKill : MonoBehaviour {

	Player player;
	PlayerDashChaining playerDashChaining;
	EnemySpawn enemySpawn;
	RuneStone enemyDestroysRunestone;
	[SerializeField] Cinemachine.CinemachineVirtualCamera chainKillCam;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		playerDashChaining = player.GetComponent<PlayerDashChaining>();

		enemySpawn = GetComponent<EnemySpawn>();

		enemyDestroysRunestone = GameObject.FindGameObjectWithTag("RuneStone").GetComponent<RuneStone>();
	}

	public void ChainKillAllEnemies(List<Enemy> enemyList){
		if(!player.playerState.canMove) return;

		//stop time completely while we set this thing up, just to be save
		Time.timeScale = 0;
		
		//clear all targets
		playerDashChaining.ClearTargetStash();

		//make the player into a badass
		player.playerState.isLegendary = true;
		player.playerState.currentDamage = 1000f;
		playerDashChaining.SetChainKillParticlesEnabled(true);

		//stop all enemies from moving
		foreach(Enemy e in enemyList){
			e.canMove = false;
		}

		//set all enemies up for pain and sudden death
		for(int i = 0; i < enemyList.Count; i++){
			Enemy currentEnemy = enemyList[i];
			
			//we need to make sure the player isn't knocked out of his legendary status... that would be
			//very embarrassing
			if(currentEnemy.GetEnemyHealth().hasShield){
				currentEnemy.GetEnemyShieldCollision().SetDestructible(true);
			}
			//we reuse the player dash chaining script to great effect here, which saves us 
			//having to write a specialized chain kill dash script
			playerDashChaining.StashTarget(currentEnemy.transform.position);
		}
		chainKillCam.Priority = 12;
		Time.timeScale = 0.9f;
		this.StartCoroutine(ResetPlayerAfterChainKill());
	}

	IEnumerator ResetPlayerAfterChainKill(){
		//we wait until the player has killed all enemies and then trigger a game victory in 
		//the usual way
		yield return new WaitUntil(() => playerDashChaining.isTargetStashEmpty());

		playerDashChaining.SetChainKillParticlesEnabled(false);
		
		yield return new WaitForSecondsRealtime(1f);
		
		chainKillCam.Priority = 8;
		enemyDestroysRunestone.WinOrLoose(true);
		
		yield break;
	}

	void OnDestroy(){
		StopAllCoroutines();
	}


}
