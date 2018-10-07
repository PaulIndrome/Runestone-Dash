using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/effects/EnemyHealRadius")]
public class EnemyHealRadiusEffect : EnemyEffect {

	public bool healsToMax = false;
	[Range(1f, 10f)] public int amountToHeal = 1;
	[Range(1f,10f)] public float healRadius;
	[Range(0.5f,10f)] public float pulseInwardTime;
	[Range(0.5f,10f)] public float pulseOutwardTime;
	public Material healRadiusMaterial;
	public ParticlePooler radiusHealParticles;
	public override void Apply(Enemy enemy){
		//the EnemyHealRadius gets added to the Enemy gameobject, which then adds the lineRenderer component etc.
		enemy.gameObject.AddComponent<EnemyHealRadius>().Activate(amountToHeal, healsToMax, healRadius, pulseOutwardTime, pulseInwardTime, healRadiusMaterial, radiusHealParticles);
		enemy.GetEnemyAnimator().SetBool("isHealer", true);
	}

	public override string GetShortHand(){
		return "heal";
	}

}
