using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour {

	public EnemyDestroysRunestone enemyDestroysRunestone;
	void Update () {
		if(transform.childCount <= 0){
			enemyDestroysRunestone.RuneStoneDestroyed(true);
		}		
	}
}
