using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName="enemy/EnemyCurvePath")]
public class EnemyCurvePath : ScriptableObject {

	public AnimationCurve zigZag;

	public IEnumerator MoveTowardsTarget(MonoBehaviour runner){
		//setup scripts and transforms
		Enemy enemy = runner.GetComponent<Enemy>();
		if(enemy == null) yield break;
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

				//the next step to take, defined by the animationcurve's y-values and the enemytype's fluctuation amount
				step.z = Mathf.Lerp(startPosition.z, 0, t);
				step.x = Mathf.Lerp(startPosition.x, 0, t);
				step = step + (carrierEmpty.right * zigZag.Evaluate(t) * enemyType.leftRightFluct);

				//the animationspeed of the walking animation is governed by how far the empty carrier will move next
				nextSpeed = (step - carrierEmpty.position).sqrMagnitude * 100000 * Time.deltaTime;
				enemyAnimator.SetFloat("walkSpeed", Mathf.Clamp(nextSpeed, 0.5f, 1.5f));

				//the mesh and collider of the enemy are rotated, the empty carrier keeps its orientation
				meshAndCollider.LookAt(new Vector3(step.x, meshAndCollider.transform.position.y, step.z));
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