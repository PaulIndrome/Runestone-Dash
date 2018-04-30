using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealRadius : MonoBehaviour {

	EnemySpawn enemySpawn;
	Animator enemyAnimation;
	LineRenderer line;
	float maxRadius;
	float currentRadius;
	public float pulseOutTime, pulseInTime;
	int resolution = 16;
	bool healsToMax = false;
	int amountToHeal;
	Vector3 lineRendererBaseHeight = new Vector3(0, 0.2f, 0);

	//setup for the EnemyHealRadius via the variables of the EnemyHealRadiusEffect ScriptableObject
	public void Activate(int healAmount, bool fullHeal, float r, float pulseOut, float pulseIn, Material healRadiusMaterial){
		this.enabled = true;
		amountToHeal = healAmount;
		healsToMax = fullHeal;
		pulseOutTime = pulseOut;
		pulseInTime = pulseIn;
		maxRadius = currentRadius = r;
		SetupLineRenderer(healRadiusMaterial);
		enemyAnimation = GetComponent<Enemy>().GetEnemyAnimator();
		//we actually get the entire EnemySpawn script as a reference so we can easily
		//iterate over all active enemies later when casting the heal spell
		enemySpawn = GetComponentInParent<EnemySpawn>();		
		StartCoroutine(HealAllEnemiesInRange());
	}

	//the LineRenderer component gets added at runtime which is something I should do more often...
	public void SetupLineRenderer(Material healRadiusMaterial) {
		line = gameObject.AddComponent<LineRenderer>();
		line.widthCurve = AnimationCurve.Linear(0,0.1f,1,0.1f);
		line.material = healRadiusMaterial;
		Color lineColor = healsToMax ? Color.yellow : Color.red;
		lineColor.a = 0.5f;
		line.endColor = lineColor;
		line.startColor = line.endColor;
		line.loop = true;
	}

	void Update () {
		line.positionCount = resolution + 1;
		for (var i = 0; i < line.positionCount; i++){
			var angle = (360/line.positionCount+1) * i;
			line.SetPosition(i, transform.position + currentRadius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)) + lineRendererBaseHeight);
		}
	}

	//TODO: add ParticlePooler for the healing spell
	//(With the existing system takes about 30 minutes)
	IEnumerator HealAllEnemiesInRange(){
		float t, lerp;
		bool animationTriggered = false;
		while(gameObject.activeSelf){
			t = 0;
			currentRadius = 0;
			lerp = 0;
			while(t <= pulseOutTime){ //lerp out
				lerp = t / pulseOutTime;
				currentRadius = Mathf.Lerp(0, maxRadius, lerp);
				if(lerp > 0.75f && !animationTriggered) { //start the heal animation ONCE
					enemyAnimation.SetTrigger("fireHealBurst");
					animationTriggered = true;
				}
				t += Time.deltaTime;
				yield return null;
			}
			CheckAllEnemiesInRange(); //cast the healing spell
			t = 0;
			while(t <= pulseInTime){ //lerp back in
				currentRadius = Mathf.Lerp(maxRadius, 0, t / pulseInTime);
				t += Time.deltaTime;
				yield return null;
			}
			animationTriggered = false;
			yield return null;
		}
	}

	//because we can reference the entire list of active enemies, we can just use
	//Vector3.Distance() to check if anything in range can be healed
	public void CheckAllEnemiesInRange(){
		foreach(Enemy e in enemySpawn.enemies){
			if(Vector3.Distance(transform.position, e.transform.position) <= maxRadius){
				//the currentHealth of any Enemy is clamped to its maximum amount so... heal by 1000f for full recovery.
				e.GetEnemyHealth().HealByAmount(healsToMax ? 1000f : amountToHeal);
			}
		}
		foreach(Enemy bE in enemySpawn.bosses){
			if(Vector3.Distance(transform.position, bE.transform.position) <= maxRadius){
				bE.GetEnemyHealth().HealByAmount(healsToMax ? 1000f : amountToHeal);
			}
		}
	}

	
}
