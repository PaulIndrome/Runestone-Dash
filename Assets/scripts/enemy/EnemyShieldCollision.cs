using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyShieldCollision : MonoBehaviour {

	EnemyHealth enemyHealth;
	Enemy enemy;
	Animator enemyAnimator;
	BoxCollider boxCollider;
	MeshRenderer meshRenderer;

	
	Vector3 enemyForward, playerForward;
	HealthBar durabilityBar;
	int maxShieldDurability;
	[Header("Shield status variables")]
	[SerializeField] bool indestructibleShield;
	[SerializeField] int shieldDurability;
	[SerializeField] List<EnemyShieldPiece> shieldPieces;
	[SerializeField] HealthBar durabilityBarPrefab;
	[SerializeField] Transform durabilityBarPosition;
		
	[Header("Particle spawners")]
	public ParticlePooler playerHitShieldPooler;
	public ParticlePooler playerDestroyShieldPooler;
	
	AudioSource enemyAudioSource;
	[Header("Audio Source and Events")]
	public AudioEvent audioShieldDestroyed;
	public AudioEvent audioShieldHit;

	void Awake(){
		enemyHealth = GetComponentInParent<EnemyHealth>();
	}

	public void Activate(bool indestructible, int durability){
		enemy = GetComponentInParent<Enemy>();
		enemyHealth = enemy.GetEnemyHealth();
		enemyAudioSource = GetComponentInParent<AudioSource>();
		enemyAnimator = enemy.GetEnemyAnimator();

		boxCollider = GetComponent<BoxCollider>();
		boxCollider.enabled = true;

		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.enabled = true;

		gameObject.SetActive(true);

		foreach(EnemyShieldPiece esp in shieldPieces){
			esp.gameObject.SetActive(true);
		}

		shieldDurability = maxShieldDurability = durability;
		indestructibleShield = indestructible;
	}

	public void OnCollisionEnter(Collision col){
		Player player = col.gameObject.GetComponent<Player>();

		if(player != null){
			enemyForward = durabilityBarPosition.forward.normalized;
			playerForward = player.transform.forward.normalized;
			playerForward.y = enemyForward.y;
			float dot = Vector3.Dot(enemyForward, playerForward);

			if(!player.playerState.isDashing){
				//Debug.Log("ooooo shield hit " + dot);
				enemyHealth.TriggerIFrames(0.2f);
				//playerHitShieldPooler.SpawnFromQueueAndPlay(null, col.contacts[0].point, player.transform.position);
				ShieldBash(player);
				return;
			}
			//if we've made it this far, the player is dashing ... get it?
			if(dot > 0.4f){
				//Debug.Log("shield destroy " + dot);
				PlayerDestroysShield();
			} else {
				//Debug.Log("xxxxx shield hit " + dot);
				if(!DurabilityHit()){
					enemyHealth.TriggerIFrames(0.2f);
					playerHitShieldPooler.SpawnFromQueueAndPlay(transform, col.contacts[0].point, player.transform.position);
					ShieldBash(player);
				}
			}
		} 
	}

	public void PlayerDestroysShield(){
		//shieldAnimator.SetTrigger("shieldDestroyed");
		playerDestroyShieldPooler.SpawnFromQueueAndPlay(null, transform.position, transform.forward);
		audioShieldDestroyed.PlayOneShot(enemyAudioSource);
		DestroyShieldObject();
	}

	public void ShieldBash(Player player){
		//player.playerState.hitEnemyShield = true;
		player.playerDashChaining.PlayerShovedBack();
		enemyAnimator.SetTrigger("shieldBash");
		audioShieldHit.PlayOneShot(enemyAudioSource);
	}

	public void DestroyShieldObject(){
		Destroy(durabilityBar.gameObject);

		GetComponentInParent<EnemyHealth>().hasShield = false;
		GetComponentInParent<Enemy>().GetEnemyAnimator().SetBool("isShielded", false);
		foreach(EnemyShieldPiece esp in shieldPieces){
			esp.StartFadeOut(5f);
		}
		boxCollider.enabled = false;
	}

	public void SetupDurabilityBar(RectTransform healthBarCanvas){
		durabilityBar = Instantiate(durabilityBarPrefab);
		durabilityBar.SetTarget(durabilityBarPosition);
		durabilityBar.transform.SetParent(healthBarCanvas);

		if(indestructibleShield) {
			Color indestructibleShieldColor = Color.gray;
			indestructibleShieldColor.a = 0.8f;
			durabilityBar.ChangeBarColorTo(indestructibleShieldColor);
			indestructibleShieldColor.a = 0.95f;
			durabilityBar.ChangeBarEndColorTo(indestructibleShieldColor);
		}
	}

	bool DurabilityHit(){
		if(indestructibleShield) return false;
		shieldDurability -= 1;
		durabilityBar.SetBarTo((float)shieldDurability / (float)maxShieldDurability);
		if(shieldDurability <= 0){
			PlayerDestroysShield();
			return true;
		}
		return false;
	}


}
