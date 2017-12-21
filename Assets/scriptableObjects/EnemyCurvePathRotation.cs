using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/EnemyCurvePathRotation")]
public class EnemyCurvePathRotation : ScriptableObject {

	public AnimationCurve zigZag;

	public IEnumerator MoveTowardsTarget(MonoBehaviour runner){
		//setup scripts and transforms
		Enemy enemyScript = runner.GetComponent<Enemy>();
		EnemyType enemyType = enemyScript.enemyType;
		Transform meshAndCollider = enemyScript.meshAndCollider.transform;
		Transform carrierEmpty = runner.gameObject.transform;

		//gather startposition and instantiate controlling floats
		Vector3 startPosition = carrierEmpty.position;
		float timeWalked = 0;
		float t = 0;

		//
		Vector3 step = Vector3.zero;

		while(enemyScript.health > 0){
			t = timeWalked / enemyType.approachTime;

			step.z = Mathf.Lerp(startPosition.z, 0, t);
			step.x = zigZag.Evaluate(t) * enemyType.leftRightFluct;

			carrierEmpty.localPosition = step;
			
			timeWalked += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}
}
