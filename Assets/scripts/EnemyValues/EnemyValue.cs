using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyValue {
	void Swap(IEnemyValue enemyValue);
}


public class EnemyValue<T> : IEnemyValue {
    public T Value;

	public EnemyValue(T initvalue){
		Value = initvalue;
	}

	public void Swap(IEnemyValue enemyValue){
		//type check for generic types seems to be a bit weird
		EnemyValue<T> eVal = enemyValue as EnemyValue<T>;
		if(enemyValue == null || enemyValue.GetType() != Value.GetType()){
			Debug.LogWarning("Value mismatch");
			return;
		}
		
		T temp = Value;
		Value = eVal.Value;
		eVal.Value = temp;
	}

}

[System.Serializable]
public class EInt : EnemyValue<int>
{
    public EInt(int initvalue) : base(initvalue)
    {
    }
}
[System.Serializable]
public class EFloat : EnemyValue<float>
{
    public EFloat(float initvalue) : base(initvalue)
    {
    }
}
[System.Serializable]
public class EString : EnemyValue<string>
{
    public EString(string initvalue) : base(initvalue)
    {
    }
}
