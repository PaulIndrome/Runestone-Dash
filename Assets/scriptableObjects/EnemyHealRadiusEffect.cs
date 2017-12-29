using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/effects/EnemyHealRadius")]
public class EnemyHealRadiusEffect : EnemyEffect {

	[Range(2f,10f)] public float healRadius;
	[Range(1f,5f)] public float pulseInwardTime;
	[Range(1f,2f)] public float pulseOutwardTime;
	public override void Apply(Enemy enemy){
		enemy.gameObject.AddComponent<EnemyHealRadius>().Activate(healRadius, pulseOutwardTime, pulseInwardTime);
	}

}
