using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/effects/EnemyShield")]
public class EnemyShield : EnemyEffect {

	public override void Apply(Enemy enemy){
		enemy.gameObject.GetComponentInChildren<EnemyShieldCollision>().Activate();
		enemy.hasShield = true;
	}

}
