using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyShieldCollision : MonoBehaviour {

	EnemyHealth enemyHealth;
	Enemy enemy;
	Animator enemyAnimator;
	private RaycastHit raycastHit;
	private BoxCollider boxCollider;
	private MeshRenderer meshRenderer;

	
	private Vector3 firstContactPointWorld;
	private Vector3 playerPosAtColLocal;
	private Vector3 enemyForward, playerForward;
	[Header("Shield status variables")]
	int maxShieldDurability;
	[SerializeField] bool indestructibleShield;
	[SerializeField] int shieldDurability;
	[SerializeField] List<EnemyShieldPiece> shieldPieces;
	[SerializeField] HealthBar durabilityBarPrefab;
	HealthBar durabilityBar;
	[SerializeField] Transform durabilityBarPosition;
		
	[Header("Particle spawners")]
	public ParticlePooler playerHitShieldPooler;
	public ParticlePooler playerDestroyShieldPooler;
	
	private AudioSource enemyAudioSource;
	[Header("Audio Source and Events")]
	public AudioEvent audioShieldDestroyed;
	public AudioEvent audioShieldHit;

	public void Activate(bool indestructible, int durability){
		enemyHealth = GetComponentInParent<EnemyHealth>();
		enemyAudioSource = GetComponentInParent<AudioSource>();
		enemy = GetComponentInParent<Enemy>();
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
			if(!player.playerState.isDashing){
				enemyHealth.TriggerIFrames(0.2f);
				//playerHitShieldPooler.SpawnFromQueueAndPlay(null, col.contacts[0].point, player.transform.position);
				ShovePlayerBack(player);
				return;
			}
			//if we've made it this far, the player is dashing ... get it?
			enemyForward = transform.parent.forward.normalized;
			playerForward = player.transform.forward.normalized;
			float dot = Vector3.Dot(enemyForward, playerForward);
			if(dot > 0.05f){
				PlayerDestroysShield();
			} else {
				enemyHealth.TriggerIFrames(0.2f);
				DurabilityHit();
				playerHitShieldPooler.SpawnFromQueueAndPlay(transform, col.contacts[0].point, player.transform.position);
				ShovePlayerBack(player);
			}
		} 
	}

	public void PlayerDestroysShield(){
		//shieldAnimator.SetTrigger("shieldDestroyed");
		playerDestroyShieldPooler.SpawnFromQueueAndPlay(null, transform.position, transform.forward);
		audioShieldDestroyed.PlayOneShot(enemyAudioSource);
		DestroyShieldObject();
	}

	public void ShovePlayerBack(Player player){
		enemyAnimator.SetTrigger("shieldBash");
		player.playerState.hitEnemyShield = true;
		player.transform.position -= player.transform.forward * 3;
		player.SetAnimatorTrigger("hitEnemyShield");
		audioShieldHit.PlayOneShot(enemyAudioSource);
		player.SetAnimatorBool("isDashing", false);
		player.ResetColliderWidth();
	}

	public void DestroyShieldObject(){
		Destroy(durabilityBar.gameObject);

		GetComponentInParent<EnemyHealth>().hasShield = false;
		GetComponentInParent<Enemy>().GetEnemyAnimator().SetBool("isShielded", false);
		foreach(EnemyShieldPiece esp in shieldPieces){
			esp.StartFadeOut(5f);
		}
		boxCollider.enabled = false;
		meshRenderer.enabled = false;
	}

	public void SetupDurabilityBar(RectTransform healthBarCanvas){
		durabilityBar = Instantiate(durabilityBarPrefab);
		durabilityBar.SetTarget(durabilityBarPosition);
		durabilityBar.transform.SetParent(healthBarCanvas);
		if(indestructibleShield) durabilityBar.ChangeBarColorTo(Color.gray);
	}

	void DurabilityHit(){
		if(indestructibleShield) return;
		shieldDurability -= 1;
		durabilityBar.SetBarTo((float)shieldDurability / (float)maxShieldDurability);
		if(shieldDurability <= 0){
			PlayerDestroysShield();
		}
	}


}
