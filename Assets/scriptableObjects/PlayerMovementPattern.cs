using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="player/movement")]
public class PlayerMovementPattern : ScriptableObject {

	[Range(-1,1)]
	public int direction;
	
}
