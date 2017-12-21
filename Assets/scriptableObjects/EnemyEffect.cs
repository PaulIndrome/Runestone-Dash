using System.Collections;
using UnityEngine;

public abstract class EnemyEffect : ScriptableObject {
	public abstract void Apply(GameObject enemy);
}
