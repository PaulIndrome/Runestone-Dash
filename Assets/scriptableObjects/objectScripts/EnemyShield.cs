using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/effects/EnemyShield")]
public class EnemyShield : EnemyEffect {

	public override void Apply(Enemy enemy){
		enemy.GetEnemyAnimation().SetBool("hasShield", true);
		enemy.GetEnemyShieldCollision().Activate();
		enemy.GetEnemyHealth().hasShield = true;
	}

}
