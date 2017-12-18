using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="player/movement")]
public class DistancesFromCenter : ScriptableObject {

	[Range(-10,10)]
	public float[] distancesFromCenter;
	
}
