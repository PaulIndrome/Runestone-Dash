using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/effects/EnemyShield")]
public class EnemyShieldEffect : EnemyEffect {

	public bool indestructibleShield = false;
	[Range(1f, 10f)] public int shieldDurability = 1;

	public override void Apply(Enemy enemy){
		//for animation reasons, every Enemy owns a dormant shield which is activated and setup when needed
		enemy.GetEnemyShieldCollision().Activate(indestructibleShield, shieldDurability);
		enemy.enemyHealth.hasShield = true;
		enemy.GetEnemyAnimator().SetBool("isShielded", true);
	}

	public override string GetShortHand(){
		return (indestructibleShield ? "i" : "") + "shield";
	}

}
