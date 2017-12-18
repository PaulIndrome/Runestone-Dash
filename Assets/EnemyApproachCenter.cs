using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproachCenter : MonoBehaviour {
	
	public float approachSpeed;
	public float differenceDueToEnemyDeath;
	public int enemiesKilledBefore;
	public ParticleSystem killedPS;

	public EnemyCounter enemyCounter;

	public void Start(){
		enemiesKilledBefore = enemyCounter.enemiesKilled;
	}
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * approachSpeed + ((enemyCounter.enemiesKilled - enemiesKilledBefore) / 10));	
	}

	public void OnTriggerEnter(Collider collision){
		if(collision.gameObject.CompareTag("Player"))
			StartCoroutine(HasBeenKilled());
		else if(collision.gameObject.GetComponent<EnemyDestroysRunestone>() != null)
			collision.gameObject.GetComponent<EnemyDestroysRunestone>().RuneStoneDestroyed(false);
	}

	IEnumerator HasBeenKilled(){
		approachSpeed = 0;
		killedPS.Play();
		GetComponent<BoxCollider>().enabled = false;
		while(killedPS.isPlaying){
			transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up, Time.deltaTime);
			yield return null;
		}
		Destroy(gameObject);
		yield return null;
	}

}
