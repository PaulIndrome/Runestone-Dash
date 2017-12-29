using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/EnemyType")]
public class EnemyType : ScriptableObject {

	[Range(1f, 10f)]
	public float health;
	
	[Range(0.5f, 5f)]
	public float scaleOfEnemy;

	[Range(1f, 20f)]
	public float leftRightFluct;

	[Range(5f, 50f)]
	public float approachTime;

	public EnemyEffect[] enemyEffects;

}
