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
	bool isDestructible = false;
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

	//setup for the EnemyShield via the variables of the EnemyShieldEffect ScriptableObject
	public void Activate(bool indestructible, int durability){
		enemy = GetComponentInParent<Enemy>();
		enemyHealth = enemy.enemyHealth;
		enemyAudioSource = GetComponentInParent<AudioSource>();
		enemyAnimator = enemy.GetEnemyAnimator();

		boxCollider = GetComponent<BoxCollider>();
		boxCollider.enabled = true;

		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.enabled = true;

		gameObject.SetActive(true);

		//this is a remnant from an earlier iteration of the shields and will be used again
		//at a later time to make the shield actually break to pieces on destruction
		foreach(EnemyShieldPiece esp in shieldPieces){
			esp.gameObject.SetActive(true);
		}

		shieldDurability = maxShieldDurability = durability;
		indestructibleShield = indestructible;
	}

	//utility method used during the ChainKill scripted events to make sure the player
	//isn't knocked back by shields anymore after he's won the game
	public void SetDestructible(bool destructible){
		isDestructible = destructible;
	}

	public void OnCollisionEnter(Collision col){
		Player player = col.gameObject.GetComponent<Player>();

		if(player != null){
			
			if(isDestructible){
				PlayerDestroysShield();
				return;
			}

			if(!player.playerState.isDashing){
				enemyHealth.TriggerIFrames(0.2f);
				ShieldBash(player);
				return;
			}
			enemyForward = durabilityBarPosition.forward.normalized;
			playerForward = player.transform.forward.normalized;
			playerForward.y = enemyForward.y;
			//determine the direction the player has hit the shield from via dot product
			float dot = Vector3.Dot(enemyForward, playerForward);
			//if we've made it this far, the player is dashing ... get it?
			if(dot > 0.4f){
				//shield hit from behind
				PlayerDestroysShield();
			} else {
				//shield hit from the front
				if(!DurabilityHit()){
					//at this point the shield has been hit from the front and is still intact enough to bash the player
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
		durabilityBar.Unregister();
		Destroy(durabilityBar.gameObject);

		GetComponentInParent<EnemyHealth>().hasShield = false;
		GetComponentInParent<Enemy>().GetEnemyAnimator().SetBool("isShielded", false);

		//this is a remnant from an earlier iteration of the shields and will be used again
		//at a later time to make the shield actually break to pieces on destruction
		foreach(EnemyShieldPiece esp in shieldPieces){
			esp.StartFadeOut(5f);
		}
		boxCollider.enabled = false;
	}

	//similar to EnemyHealth.SetupHealthBar()
	public void SetupDurabilityBar(EnemyHealthBarsHandler healthBarHandler){
		durabilityBar = Instantiate(durabilityBarPrefab);
		durabilityBar.SetTarget(durabilityBarPosition);
		durabilityBar.transform.SetParent(healthBarHandler.transform);

		if(indestructibleShield) {
			Color indestructibleShieldColor = Color.gray;
			indestructibleShieldColor.a = 0.8f;
			durabilityBar.ChangeBarColorTo(indestructibleShieldColor);
			indestructibleShieldColor.a = 0.95f;
			durabilityBar.ChangeBarEndColorTo(indestructibleShieldColor);
		}

		healthBarHandler.RegisterBar(durabilityBar);
	}

	//this method returns true if the shield has been destroyed so a lasthit on a shield
	//from the front doesn't knock back the player
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
