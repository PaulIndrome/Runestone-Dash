using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/effects/EnemyShield")]
public class EnemyShield : EnemyEffect {

	public bool indestructibleShield = false;
	[Range(1f, 10f)] public int shieldDurability = 1;

	public override void Apply(Enemy enemy){
		enemy.GetEnemyShieldCollision().Activate(indestructibleShield, shieldDurability);
		enemy.GetEnemyHealth().hasShield = true;
		enemy.GetEnemyAnimator().SetBool("isShielded", true);
	}

	public override string GetShortHand(){
		return (indestructibleShield ? "i" : "") + "shield";
	}

}
