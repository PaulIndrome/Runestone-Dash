using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealRadius : MonoBehaviour {

	private EnemyAnimation enemyAnimation;
	private LineRenderer line;
	float radius;
	float pulsingRadius;
	public float pulseOutTime, pulseInTime;
	int resolution = 16;

	public void Activate(float r, float pulseOut, float pulseIn, Material healRadiusMaterial){
		this.enabled = true;
		pulseOutTime = pulseOut;
		pulseInTime = pulseIn;
		radius = pulsingRadius = r;
		SetupLineRenderer(healRadiusMaterial);
		enemyAnimation = GetComponent<EnemyAnimation>();
		enemyAnimation.SetBool("isHealer", true);
		StartCoroutine(HealAllEnemiesInRange());
	}

	public void SetupLineRenderer(Material healRadiusMaterial) {
		line = gameObject.AddComponent<LineRenderer>();
		line.widthCurve = AnimationCurve.Linear(0,0.1f,1,0.1f);
		line.material = healRadiusMaterial;
		line.endColor = Color.red;
		line.startColor = line.endColor;
		line.loop = true;
	}

	void Update () {
		line.positionCount = resolution + 1;
		for (var i = 0; i < line.positionCount; i++){
			var angle = (360/line.positionCount+1) * i;
			line.SetPosition(i, transform.position + pulsingRadius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)));
		}
	}

	IEnumerator HealAllEnemiesInRange(){
		float t;
		while(gameObject.activeSelf){
			t = 0;
			pulsingRadius = 0;
			while(t <= pulseOutTime){
				pulsingRadius = Mathf.Lerp(0, radius, t / pulseOutTime);
				t += Time.deltaTime;
				yield return null;
			}
			enemyAnimation.SetTrigger("castHeal");
			CheckAllEnemiesInRange();
			t = 0;
			while(t <= pulseInTime){
				pulsingRadius = Mathf.Lerp(radius, 0, t / pulseInTime);
				t += Time.deltaTime;
				yield return null;
			}
			yield return null;
		}
	}

	public void CheckAllEnemiesInRange(){
		foreach(Enemy e in GetComponentInParent<EnemySpawn>().enemies){
			if(Vector3.Distance(transform.position, e.transform.position) <= radius){
				e.GetEnemyHealth().HealByAmount(1f);
			}
		}
	}

	
}
