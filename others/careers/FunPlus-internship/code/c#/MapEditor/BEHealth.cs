using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEHealth
///   Description:    common class to process ralated with health & shield
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.1 (2015-09-30)
///                   - dead sound removed
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class BEHealth : MonoBehaviour {

		public 	GameObject	prefUI;				// healthbar ui prefab
		private UIHealthBar	UIHealth = null;	// ui 

		public  float 		maxShield = 0.0f;
		public  float 		shield = 0.0f;
		public  float 		maxHealth = 100.0f;
		public  float 		health = 100.0f;
		public  float 		regenerateSpeed = 0.0f;
		public  bool		invincible = false;
		[HideInInspector]
		public  bool 		dead = false;

		public  GameObject 		goShield = null;	// gameobject of shield

		public  GameObject 		damagePrefab;
		public  Transform 		damageEffectTransform;
		public  bool 			damageEffectCentered = true;
		private ParticleSystem damageEffect;
		private float 			damageEffectCenterYOffset;
		
		private float 			colliderRadiusHeuristic = 1.0f;

		[HideInInspector]
		public  string 			DeadNotifier = "";	// call function when dead
		public 	GameObject		effectDead = null;	// create this effect when dead

		private float			UIShowPeriod = 0.0f;

		public 	UIInfo			uiInfo = null;

		void Awake () {
			//enabled = false;
		}

		void Start() {
			//damage effect create
			if (damagePrefab) {
				if (damageEffectTransform == null)
					damageEffectTransform = transform;
				GameObject effect = BEObjectPool.Spawn (damagePrefab, Vector3.zero, Quaternion.identity);
				effect.transform.parent = damageEffectTransform;
				effect.transform.localPosition = Vector3.zero;
				damageEffect = effect.GetComponent<ParticleSystem>();
				Vector2 tempSize = new Vector2(GetComponent<Collider>().bounds.extents.x,GetComponent<Collider>().bounds.extents.z);
				colliderRadiusHeuristic = tempSize.magnitude * 0.5f;
				damageEffectCenterYOffset = GetComponent<Collider>().bounds.extents.y;
			}
			//health bar ui create
			if(prefUI && UIInGame.instance) {
				UIHealth = UIInGame.instance.AddInGameUI(prefUI, transform, new Vector3(0,1.5f,0)).GetComponent<UIHealthBar>();
				UpdateHealthUI();
			}
			
			if(UIHealth != null)
				UIHealth.gameObject.SetActive(false);

			if(uiInfo != null)
				uiInfo.groupProgress.gameObject.SetActive(false);
		}

		void Update () {
			if(UIShowPeriod > 0.0f) {
				UIShowPeriod -= Time.deltaTime;
				if(UIShowPeriod <= 0.0f) {

					if(UIHealth != null)
						UIHealth.gameObject.SetActive(false);

					if(uiInfo != null) {
						uiInfo.TimeLeft.text = "";
						uiInfo.groupProgress.gameObject.SetActive(false);
					}
				}
				else {
					if((UIHealth != null) && (!UIHealth.gameObject.activeInHierarchy)) {
						UIHealth.gameObject.SetActive(true);
					}

					if(uiInfo != null) {
						uiInfo.TimeLeft.text = "";
						uiInfo.groupProgress.gameObject.SetActive(true);
					}
				}
			}
		}

		public void OnDamage (float amount, Vector3 fromDirection) {
			if (invincible) return;
			if (dead) return;
			if (amount <= 0) return;

			UIShowPeriod = 10.0f;

			// if shield available
			if(shield > 0.01f) {

				// decrease shield before decrease health
				shield -= amount;
				if(shield < 0.0f) {
					shield = 0.0f;

					// if shield object visible, hide it
					if((goShield != null) && goShield.activeInHierarchy)
						goShield.SetActive(false);
				}
			}
			else {
				Building script = BEGround.instance.BuildingFromObject(gameObject);
				if((script != null) && (SceneBattle.instance != null)) {

					float hpDelta = (health > amount) ? amount : health;
					float fRatio = hpDelta/maxHealth;
					float CapacityGold = script.Capacity[(int)PayType.Gold];
					if(CapacityGold > 0.001f) {
						float GoldDelta = CapacityGold*fRatio;
						//Debug.Log ("Gold Decrease "+GoldDelta);
						SceneBattle.instance.Gold.ChangeDelta(-GoldDelta);
						SceneBattle.instance.PlunderGold.ChangeDelta(GoldDelta);
					}
					float CapacityElixir = script.Capacity[(int)PayType.Elixir];
					if(CapacityElixir > 0.001f) {
						float ElixirDelta = CapacityElixir*fRatio;
						//Debug.Log ("Elixir Decrease "+ElixirDelta);
						SceneBattle.instance.Elixir.ChangeDelta(-ElixirDelta);
						SceneBattle.instance.PlunderElixir.ChangeDelta(ElixirDelta);
					}
				}
				
				
				// decrease health
				health -= amount;


				// if shield object visible, hide it
				if((goShield != null) && goShield.activeInHierarchy)
					goShield.SetActive(false);
			}

			if (regenerateSpeed > 0)
				enabled = true;
			
			// Show damage effect
			if (damageEffect) {
				damageEffect.transform.rotation = Quaternion.LookRotation (fromDirection, Vector3.up);
				if(!damageEffectCentered) {
					Vector3 dir = fromDirection;
					dir.y = 0.0f;
					damageEffect.transform.position = (transform.position + Vector3.up * damageEffectCenterYOffset) + colliderRadiusHeuristic * dir;
				}
				damageEffect.Emit(10);
			}
			
			// Die if no health left
			if (health <= 0) {
				//BEUtil.instance.SoundPlay(4+Random.Range(0,2));
				if(effectDead != null) {
					GameObject go = (GameObject)Instantiate(effectDead, transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
					Destroy (go,1.0f);
				}

				health = 0;
				dead = true;
				enabled = false;

				// if notify function exist, then send message
				if(!string.Equals(DeadNotifier, "")) {
					gameObject.SendMessage(DeadNotifier);
				}
				// or process to dead
				else {
					Dead();
				}
			}

			UpdateHealthUI();
		}

		public void Dead() {
			// hide ui
			UIInGame.instance.RemoveInGameUI(transform);
			//UIHealth.gameObject.SetActive(false);
			// unspawn object pool
			BEObjectPool.Unspawn(gameObject);

			// if this is building
			Building script = BEGround.instance.BuildingFromObject(gameObject);
			if(script != null) {
				if(script.goCenter != null) { BEObjectPool.Unspawn(script.goCenter); 	script.goCenter = null; }
				if(script.goXInc != null) 	{ BEObjectPool.Unspawn(script.goXInc); 		script.goXInc = null; }
				if(script.goZInc != null) 	{ BEObjectPool.Unspawn(script.goZInc); 		script.goZInc = null; }
				if(script.goRuins != null) 	{ script.goRuins.SetActive(true); }

				BEGround.instance.OccupySet(script, false);

				BECameraRTS.instance.Shake(0.4f,0.5f);
				BEAudioManager.SoundPlay(4);

				if(SceneBattle.instance != null)
					SceneBattle.instance.BuildingDestroyed(script);
			}
		
			if(uiInfo != null) {
				uiInfo.groupProgress.gameObject.SetActive(false);
			}
		}

		public void Reset() {
			health = maxHealth;
			shield = maxShield;

			if(UIHealth != null)
				UIHealth.gameObject.SetActive(true);

			UpdateHealthUI();
			dead = false;
		}

		public void UpdateHealthUI() {
			// Update progress of ui
			if(UIHealth != null) {
				UIHealth.Health.value = health/maxHealth;
				UIHealth.Shield.value = shield/maxShield;
				UIHealth.Shield.gameObject.SetActive((maxShield < 0.01f) ? false : true);
			}

			if(uiInfo != null) {
				uiInfo.TimeLeft.text = "";
				uiInfo.Progress.fillAmount = health/maxHealth;
			}
		}
	}

}