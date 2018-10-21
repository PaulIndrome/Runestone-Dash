using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public enum Activity {
		MovingToRunestone,
		MovingToHealer,
		MovingAlongHealer,
		ChargingPlayer,
		AttackingRuneStone,
		Dying
	}

	bool jumping = false;
	float moveSpeed, healerSpeed, distanceToRuneStone, distanceToHealer, currentHealerHealRadius, currentHealerMoveSpeed;
	int shieldWalkDirX = Animator.StringToHash("walkDirectionX"), shieldWalkDirZ = Animator.StringToHash("walkDirectionZ"), animSpeedID = Animator.StringToHash("animSpeed");
	[SerializeField]
	Activity currentActivity;
	Rigidbody rb;
	RuneStone runeStone;
	EnemyHealRadius currentHealer;
	Vector3 finalMoveDirection, directionOfRunestone, directionOfHealer;
	GameObject healerColliders;
	Coroutine decisionRoutine;
	Animator animator;

	Enemy thisEnemy;
	
	public float rotationSpeed, runeStoneAttackDistance, moveToHealerRate, jumpForce, findHealerDistance = 10f, animationSpeedOffset = 0f;
	public AnimationCurve curvePath;
	public EnemyType enemyType;

	public float MoveSpeed {
		get { return moveSpeed; }
	}

	public Vector3 DirectionOfHealer{
		get {
			if(currentHealer != null){
				return currentHealer.transform.position - transform.position;
			} else {
				return Vector3.zero;
			}
		}
	}

	public Vector3 DirectionOfRunestone {
		get {
			return runeStone.transform.position - transform.position;
		}
	}

	// Use this for initialization
	void Start () {
		thisEnemy = GetComponent<Enemy>();
		rb = GetComponent<Rigidbody>();
		currentActivity = Activity.MovingToRunestone;
	}

	public void SetupEnemyMovement(RuneStone runeStone, float approachTime, Animator enemyAnimator){
		this.runeStone = runeStone;
		animator = enemyAnimator;
		distanceToRuneStone = Vector3.Distance(transform.position, runeStone.transform.position);
		moveSpeed = distanceToRuneStone / approachTime;

		/*
		float stepDistanceAtSize1 = 2f / 3f;
		Debug.Log("step distance at size 1: " + stepDistanceAtSize1);
		float enemySize = thisEnemy.enemyType.scaleOfEnemy;
		Debug.Log("enemySize: " + enemySize);
		float actualStepDistance = stepDistanceAtSize1 * enemySize;
		Debug.Log("actual step distance: " + actualStepDistance);
		float stepsNeeded = distanceToRuneStone / actualStepDistance;
		Debug.Log("steps needed: " + stepsNeeded);
		float animationCyclesNeeded = stepsNeeded / 4f;
		Debug.Log("animation cycles needed (4 steps/cycle): " + animationCyclesNeeded);
		float timePerAnimationCycle = approachTime / animationCyclesNeeded;
		Debug.Log("time per animation cycle: " + timePerAnimationCycle);
		float playAnimationRatio = 3.333f / timePerAnimationCycle;
		Debug.Log("animation play ratio: " + playAnimationRatio);
		*/

		float playAnimationRatio = 3.333f / (approachTime / ((distanceToRuneStone / ((2f / 3f) * thisEnemy.enemyType.scaleOfEnemy)) / 4f));

		/*
			If enemy size is 0.9f, the moveSpeed fits pretty well
			If enemy size is larger than 0.9f, moveSpeed needs to decrease and vice versa
		 */
		//float difference = 0.9f - enemySize; // if enemySize > 0.9f this is negative
		//Debug.Log(gameObject.name + "animSpeed = " + moveSpeed + " - (0.9 - " + enemySize + ")");
		animator.SetFloat(animSpeedID, playAnimationRatio + animationSpeedOffset);

		decisionRoutine = StartCoroutine(DecisionLag(0.5f));
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentActivity){
			case Activity.AttackingRuneStone:
				break;
			case Activity.ChargingPlayer:
				break;
			case Activity.MovingToHealer:
				finalMoveDirection = ((DirectionOfHealer * moveToHealerRate) + (DirectionOfRunestone * (1f - moveToHealerRate))).normalized;
				rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(finalMoveDirection, Vector3.up), Time.deltaTime * rotationSpeed));
				rb.MovePosition(transform.position + (finalMoveDirection * Time.deltaTime * moveSpeed));
				break;
			case Activity.MovingAlongHealer:
				rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(healerColliders.transform.forward, Vector3.up), Time.deltaTime * rotationSpeed));
				rb.MovePosition(transform.position + (healerColliders.transform.forward * Time.deltaTime * healerSpeed));
				break;
			case Activity.MovingToRunestone:
				finalMoveDirection = DirectionOfRunestone.normalized;
				rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(finalMoveDirection, Vector3.up), Time.deltaTime * rotationSpeed));
				rb.MovePosition(transform.position + (finalMoveDirection * Time.deltaTime * moveSpeed));
				break;
			case Activity.Dying:
				StopAllCoroutines();
				break;
		}

	}

	Vector3 MoveToRunestone(){
		directionOfRunestone = runeStone.transform.position - transform.position;
		return directionOfRunestone.normalized;
	}

	public bool CanBeHealed(float distance){
		return distance < findHealerDistance && currentHealer == null && !thisEnemy.enemyHealth.FullHealth;
	}

	public void SetHealer(EnemyHealRadius enemyHealer, float maxRadius, float healerSpeed){
		currentHealer = enemyHealer;
		currentHealerHealRadius = maxRadius;
		this.healerSpeed = healerSpeed;

		healerColliders = enemyHealer.GetComponent<Enemy>().meshAndCollider;
	}

	public void RemoveHealer(){
		currentHealer = null;
		healerColliders = null;
		currentHealerHealRadius = 0f;
	}

	Activity DetermineNextActivity(){
		if(thisEnemy.enemyHealth.CurrentHealth <= 0){
			return Activity.Dying;
		} else if(	(DirectionOfRunestone.magnitude < runeStoneAttackDistance && currentActivity != Activity.AttackingRuneStone) || 
					(currentActivity == Activity.AttackingRuneStone && DirectionOfRunestone.magnitude > runeStoneAttackDistance)) {
				return Activity.AttackingRuneStone;
		} else if(currentHealer != null){
			if(thisEnemy.enemyHealth.FullHealth){
				RemoveHealer();
				animator.SetFloat(animSpeedID, moveSpeed + animationSpeedOffset);
				return Activity.MovingToRunestone;
			} else {
				if(DirectionOfHealer.magnitude < currentHealerHealRadius * 0.9f){
					animator.SetFloat(animSpeedID, healerSpeed + animationSpeedOffset);
					return Activity.MovingAlongHealer;
				}
				else {
					animator.SetFloat(animSpeedID, moveSpeed + animationSpeedOffset);
					return Activity.MovingToHealer;
				}
			}
		} else {
			return currentActivity;
		}
	}

	IEnumerator DecisionLag(float lag){
		Debug.Log("Decision routine started at " + Time.time);
		while(gameObject.activeSelf){
			//string s = " a ";
			if(gameObject.activeInHierarchy){
				//s += " b ";
				Activity nextActivity = DetermineNextActivity();
				if(currentActivity != nextActivity)
					Debug.Log("next activity is " + nextActivity);
				currentActivity = nextActivity;
				yield return new WaitForSeconds(lag);
			}
			//Debug.Log(s);
		}
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.white;
		UnityEditor.Handles.DrawWireDisc(transform.position + Vector3.up * 0.5f, Vector3.up, findHealerDistance);
	}

	/*
	state machine hierarchy:
	
	0. if you're near the runestone, attack it
	
	1. move along curve path towards runestone

	2.1 if you can heal someone, try to stay near them
	2.2 if someone can heal you, try to stay near them

	3. if you have a shield, try to keep yourself turned towards the player 

	4. if you're an aggressor and the player is in range, charge them
	 */
}
