using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/EnemyCurvePath")]
public class EnemyCurvePath : ScriptableObject {

	public AnimationCurve zigZag;

	public IEnumerator MoveTowardsTarget(MonoBehaviour runner){
		//setup scripts and transforms
		Enemy enemy = runner.GetComponent<Enemy>();
		Animator enemyAnimator = enemy.GetEnemyAnimator();
		EnemyHealth enemyHealth = runner.GetComponent<EnemyHealth>();
		EnemyType enemyType = enemy.enemyType;
		Transform meshAndCollider = enemy.meshAndCollider.transform;
		Transform carrierEmpty = runner.gameObject.transform;

		//gather startposition and instantiate controlling floats
		Vector3 startPosition = carrierEmpty.position;
		float timeWalked = 0;
		float t = 0;
		float nextSpeed = 0;

		Vector3 step = Vector3.zero;

		carrierEmpty.LookAt(Vector3.zero);

		while(enemyHealth.currentHealth > 0){
			if(enemy.CanMove){
				t = timeWalked / enemyType.approachTime;

				step.z = Mathf.Lerp(startPosition.z, 0, t);
				step.x = Mathf.Lerp(startPosition.x, 0, t);
				step = step + (carrierEmpty.right * zigZag.Evaluate(t) * enemyType.leftRightFluct);

				meshAndCollider.LookAt(new Vector3(step.x, meshAndCollider.transform.position.y, step.z));
				nextSpeed = (step - carrierEmpty.position).sqrMagnitude * 100000 * Time.deltaTime;
				//Debug.Log(nextSpeed);
				enemyAnimator.SetFloat("walkSpeed", Mathf.Clamp(nextSpeed, 0.5f, 1.5f));
				carrierEmpty.position = step;
				
				timeWalked += Time.deltaTime;
			} else {
				nextSpeed = 0.1f;
			}
			yield return null;
		}
		yield return null;
	}
}