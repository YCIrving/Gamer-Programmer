using UnityEngine;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEWeapon
///   Description:    implement of weapon function
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.4 (2015-09-30)
///-----------------------------------------------------------------------------------------
namespace BE {

	// fire types define
	public enum FireType {
		Semi_Auto, 	// fire bullets on by one
		Burst,		// fire 2~3 bullets at once 
		Full_Auto, 	// fire bullets repeatedly
	}

	public class BEWeapon : MonoBehaviour {

		public  FireType	fireType = FireType.Full_Auto;	// fire type
		public  int			bulletCount = 10;
		public  int			burstCount = 3;					// how many bullets at once when Burst mode
		public  bool		bulletInfinite = false;			// bullet sount no limited
		public  GameObject 	bulletPrefab;					// bullet prefab
		public  Transform 	spawnPoint;						// position from where bullet shows
		public  float 		frequency = 10;					// fire count per second
		public  float 		coneAngle = 1.5f;				// fire dir has cone shape, this is angle of cone
		[HideInInspector]
		public  bool 		firing = false;					// fire activated
		public  float 		damagePerSecond = 20.0f;		// damage per second 
		public  float 		forcePerSecond = 20.0f;			// force per second 
		//public  float 		hitSoundVolume = 0.5f;		// range hit sound effects
		public  GameObject 	muzzleFlashFront;				// muzzle effect gameobject
		[HideInInspector]
		public  GameObject 	goTarget;						// target gameobject
		public  string 		EnemyTag = "";					// enemy tag (used when raycast for enemy hit check)
		public 	AudioClip 	audioFire;						// audioclip plays when fire, if FireType is Full_Auto,this sound must be loop sound

		private AudioSource asource;						// audio source for Full_Auto sound effect
		private float 		lastFireTime  = -1.0f;			// last fire time
		private RaycastHit 	hitInfo;						// raycast for target hit picking

		public	MovType		eAttackable = MovType.Ground;	// what kind of target available

		void Start () {

			// get sudio source, if not add component
			asource = GetComponent<AudioSource>();
			if(asource == null)
				asource = (AudioSource)gameObject.AddComponent<AudioSource>();

			// set values for looping sound
			asource.playOnAwake = false;
			asource.loop = true;
			
			// hide muzzleflash
			if(muzzleFlashFront != null)
				muzzleFlashFront.SetActive (false);

			// if no spawn position, use this transform
			if (spawnPoint == null)
				spawnPoint = transform;
		}
		
		void Update () {
		
			if (firing && (goTarget == null)) {
				StopFire ();
				return;
			}

			// racast to find hitted enemy
			Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, Mathf.Infinity, LayerMask.GetMask(EnemyTag));

			// check hitinfo with transform, tag, mov type
			if (firing && hitInfo.transform && (hitInfo.transform.tag == EnemyTag)) {// && MovTypeMatch(hitInfo.transform.gameObject)) {

				// is time for new bullet creation? 
				if (Time.time > lastFireTime + 1 / frequency) {

					//update last fire time 
					lastFireTime = Time.time;

					//if bullet count is limited, decrease bullet count
					if(!bulletInfinite) {
						bulletCount--;
						// if bullet left is zero, then stop fire
						if(bulletCount <= 0)
							StopFire();
					}

					// if fire type is not burst, set burst count to 1
					if(fireType != FireType.Burst)
						burstCount = 1;

					//create new bullet with for loop
					for(int i=0 ; i < burstCount ; ++i) {

						// get dir with cone angle
						Quaternion coneRandomRotation = Quaternion.Euler (Random.Range (-coneAngle, coneAngle), Random.Range (-coneAngle, coneAngle), 0);

						// get bullet instance from objectpool
						GameObject go = BEObjectPool.Spawn (bulletPrefab, spawnPoint.position, spawnPoint.rotation * coneRandomRotation);
						// if object has no BEBullet script, then continue
						BEBullet bullet = (BEBullet)go.GetComponent("BEBullet");
						if(bullet == null) continue;

						// set enemy tag to bullet
						bullet.EnemyTag = EnemyTag;

						// if fitetype is not fullauto
						if(fireType != FireType.Full_Auto) {

							// if there is fire sound id, play sound
							if(audioFire != null)
								BEAudioManager.SoundPlay(audioFire);
						}

						// if bullet is homing function
						if(bullet.bulletType == BulletType.Homing) {
							// set bullet lifetime limit
							bullet.lifeTimeEnabled = true;
							// initialize with target transform to chase target
							bullet.Init (goTarget.transform);
						}
						// if buulet has laser function
						else if(bullet.bulletType == BulletType.Laser) {
							//set no lifetime limit
							//bullet.lifeTimeEnabled = false;
							// no hit check
							//bullet.HitCheck = false;
							// init with target transform
							bullet.Init (goTarget.transform);

							// process damage nere not at bullet
							bullet.damage = damagePerSecond / frequency;
							Vector3 force = spawnPoint.forward * (forcePerSecond / frequency);
							bullet.DamageApply(hitInfo.transform.gameObject, -spawnPoint.forward, hitInfo.point, force);
							bullet.transform.SetParent(spawnPoint,true);
						}
						// if bullet is normal type
						else {

							// process damage nere not at bullet
							bullet.damage = damagePerSecond / frequency;
							Vector3 force = spawnPoint.forward * (forcePerSecond / frequency);
							bullet.DamageApply(hitInfo.transform.gameObject, -spawnPoint.forward, hitInfo.point, force);
								
							// set max distance
							bullet.distMax = hitInfo.distance;
							// set no lifetime limit
							bullet.lifeTimeEnabled = false;

							// init with target transform
							bullet.Init (hitInfo.transform);
						}
					}
				}
			}	

			// if firetype is full auto, fire sound playes with loop
			if(fireType == FireType.Full_Auto) {
				if(firing) {

					// check sound stop if user change sound setting while fire sound is running
					if(asource.isPlaying && ((BESetting.SoundVolume==0) || (Time.timeScale < 0.01f)))
						asource.Stop();

					// play fire sound with loop
					if(!asource.isPlaying && ((BESetting.SoundVolume!=0) && (Time.timeScale > 0.01f))) {
						asource.clip = audioFire;
						asource.Play();
					}
				}
				else {
					// if not fire, stop firesound with loop
					if(asource.isPlaying)
					   asource.Stop();
				}
			}
		}

		public void StartFire() {
			if(firing) return;
			if(Time.timeScale == 0) return;
			if(Time.time < lastFireTime + 1 / frequency) return;
			if(!bulletInfinite && (bulletCount<=0)) return;

			// set firing state true
			firing = true;
			// show muzzle flash
			if(muzzleFlashFront != null)
				muzzleFlashFront.SetActive (true);
		}
	
		// stop fire with time delay
		public IEnumerator StopFireCR(float delay){
			if(delay > 0.0001f) yield return new WaitForSeconds(delay);
			StopFire();
		}

		public void StopFire() {
			if(!firing) return;

			// set firing state false
			firing = false;
			// hide muzzle flash
			if(muzzleFlashFront != null)
				muzzleFlashFront.SetActive (false);
		}

/*		//check enemy's move type whether move type is match to weapon's attackable type 
		public bool MovTypeMatch(GameObject go) {
			BEEnemy enemyScript = go.GetComponent<BEEnemy>();
			if(enemyScript != null) {
				if((eAttackable == MovType.Ground) && (enemyScript.eMovType != MovType.Ground)) return false;
				else if((eAttackable == MovType.Air) && (enemyScript.eMovType != MovType.Air)) return false;
				else  return true;
			}
			BEUnit unitScript = go.GetComponent<BEUnit>();
			if(unitScript != null) {
				if((eAttackable == MovType.Ground) && (unitScript.eMovType != MovType.Ground)) return false;
				else if((eAttackable == MovType.Air) && (unitScript.eMovType != MovType.Air)) return false;
				else  return true;
			}

			return false;
		}
*/
	}

}