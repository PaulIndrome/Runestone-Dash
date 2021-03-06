﻿using System.Collections;
using UnityEngine;

public abstract class EnemyEffect : ScriptableObject {
	public abstract void Apply(Enemy enemy);

	public abstract string GetShortHand();
}
