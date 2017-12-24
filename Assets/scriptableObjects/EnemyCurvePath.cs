using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/EnemyCurvePath")]
public class EnemyCurvePath : ScriptableObject {

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

		Vector3 step = Vector3.zero;

		carrierEmpty.LookAt(Vector3.zero);

		while(enemyScript.health > 0){
			t = timeWalked / enemyType.approachTime;

			step.z = Mathf.Lerp(startPosition.z, 0, t);
			step.x = Mathf.Lerp(startPosition.x, 0, t);
			step = step + (carrierEmpty.right * zigZag.Evaluate(t) * enemyType.leftRightFluct);

			meshAndCollider.LookAt(new Vector3(step.x, meshAndCollider.transform.position.y, step.z));
			carrierEmpty.position = step;
			
			timeWalked += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}
}