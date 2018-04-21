using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttribute : ScriptableObject {
	//public int testIntVal = 123;
	//public float testFloatVal = 6.41f;

	public EnemyAttribute swapWith;

	public List<IEnemyValue> values = new List<IEnemyValue>();
	[SerializeField] public IEnemyValue[] values2;

	public void SwapAllAttributes(EnemyAttribute otherAttributes){
		if(otherAttributes.GetType() != this.GetType()) return;
		else {
			for (int i = 0; i < values.Count; i++){
				values[i].Swap(otherAttributes.values[i]);
			}
		}
	}

}