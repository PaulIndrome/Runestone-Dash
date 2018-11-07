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

	bool holdDecision = false;
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
	Player player;
	
	public float rotationSpeed, runeStoneAttackDistance, moveToHealerRate, findHealerDistance = 10f, animationSpeedOffset = 0f;

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

	public Vector3 DirectionOfPlayer {
		get {
			return player.transform.position - transform.position;
		}
	}

	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody>();
		currentActivity = Activity.MovingToRunestone;
	}

	public void SetupEnemyMovement(RuneStone runeStone, Player player, Enemy enemy, Animator enemyAnimator){
		thisEnemy = enemy;
		this.player = player;
		this.runeStone = runeStone;
		animator = enemyAnimator;
		
		distanceToRuneStone = Vector3.Distance(transform.position, runeStone.transform.position);
		float approachTime = thisEnemy.enemyType.approachTime;
		moveSpeed = distanceToRuneStone / approachTime;

		float playAnimationRatio = 3.333f / (approachTime / ((distanceToRuneStone / ((2f / 3f) * thisEnemy.enemyType.scaleOfEnemy)) / 4f));

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
				rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentHealer.transform.forward, Vector3.up), Time.deltaTime * rotationSpeed));
				rb.MovePosition(transform.position + (currentHealer.transform.forward * Time.deltaTime * healerSpeed));
				break;
			case Activity.MovingToRunestone:
				finalMoveDirection = DirectionOfRunestone.normalized;
				if(thisEnemy.enemyType.ContainsType(typeof(EnemyShieldEffect))){
					Vector3 lookRotation = finalMoveDirection + ShieldMoveRotation().normalized;
					rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookRotation, Vector3.up), Time.deltaTime * rotationSpeed));
				} else {
					rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(finalMoveDirection, Vector3.up), Time.deltaTime * rotationSpeed));
				}
				rb.MovePosition(transform.position + (finalMoveDirection * Time.deltaTime * moveSpeed));
				break;
			case Activity.Dying:
				StopAllCoroutines();
				break;
		}
		Debug.DrawRay(transform.position, finalMoveDirection * 3f, Color.yellow, 1f);
	}

	Vector3 ShieldMoveRotation(){
		Vector3 lookRotation;
		float angle = Vector3.SignedAngle(DirectionOfRunestone.normalized, player.transform.position - transform.position, Vector3.up);
		if(Mathf.Abs(angle) < 90f){
			lookRotation = DirectionOfPlayer;
			Debug.DrawRay(transform.position, lookRotation, Color.cyan, 1f);
		} else {
			lookRotation = transform.right * Mathf.Sign(angle);
			Debug.DrawRay(transform.position, lookRotation.normalized * 3f, Color.red, 1f);
		}
		animator.SetFloat(shieldWalkDirX, Mathf.Clamp(Mathf.Sign(-angle) * (1f - Vector3.Dot(DirectionOfRunestone.normalized, lookRotation.normalized)), -1f, 1f));
		animator.SetFloat(shieldWalkDirZ, Mathf.Clamp01(Vector3.Dot(DirectionOfRunestone.normalized, lookRotation.normalized)));

		return lookRotation;
	}

	public bool CanBeHealed(float distance){
		return distance < findHealerDistance && currentHealer == null && !thisEnemy.enemyHealth.FullHealth;
	}

	public void SetHealer(EnemyHealRadius enemyHealer, float maxRadius, float healerSpeed){
		currentHealer = enemyHealer;
		currentHealerHealRadius = maxRadius;
		this.healerSpeed = healerSpeed;
	}

	public void RemoveHealer(){
		holdDecision = true;
		currentHealer = null;
		healerColliders = null;
		currentHealerHealRadius = 0f;
		holdDecision = false;
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
		while(gameObject.activeSelf){
			if(gameObject.activeInHierarchy){
				if(holdDecision){
					Debug.Log("Holding decisions");
					yield return new WaitUntil( () => !holdDecision);
					Debug.Log("Resuming decisions");
				}
				Activity nextActivity = DetermineNextActivity();
				currentActivity = nextActivity;
				yield return new WaitForSeconds(lag);
			}
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
