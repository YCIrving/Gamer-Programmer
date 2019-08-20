using UnityEngine;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEBullet
///   Description:    bullet move by bullettype
///                   when collide with enemy, do damage to enemy
///                   bullet can have time limit, distance limit, and horming features
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-08-30)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public enum BulletType {
		Simple,
		Homing,			// chase enemy
		Projectile,		// move projectile
		Laser,			// not move
	}

	public class BEBullet : MonoBehaviour {

		public  BulletType	bulletType = BulletType.Simple;
		public  Transform 	target = null;
		public  float 		damage = 2.0f;				// add damage to enemy health
		public  float 		force = 2.0f;				// add force to enemy rigidbody
		public  float		hitRange = 0.05f; 			// if homing type, bullet do damage when distance with enemy is shorter then hitrange
		public  bool        broadAttackEnabled = false;	// range attack enabled
		public  float		broadAttackRange = 1.0f; 	// range of broadattack

		public  float 		speed = 10.0f;
		public  float 		turnSpeed = 1.3f;			// rotation speed when bullet is homing
		public  float 		lifeTime = 1.0f;			// time limit
		public  bool 		lifeTimeEnabled = true;		// set time limit
		public  float 		distMax = 10000.0f;			// siatance limit
		public  string 		EnemyTag = "";				// enemy tag name
		public  bool 		HitCheck = true;			// use hit check

		private float 		dist = 0.0f;			// current distance with target
		private float 		fAge = 0.0f;			// current age of lifetime
		private Transform 	tr;
		private bool 		Initialized = false;
		private bool 		Live = false;

		//for Homing & Projectile
		private Vector3		dir;					// direction to target
		
		//for Projectile
		private Vector3 	startPos;
		private Vector3 	v = Vector3.zero;
		private Vector3 	s = Vector3.zero;
		private float 		gravity = 9.8f;

		void OnEnable () {
			tr = transform;
			fAge = 0.0f;
		}

		void Awake() {
			tr = transform;
		}
		
		void Start() {
			if(target != null)
				Init (target);
		}

		public void Init (Transform t) {
			target = t;

			if(bulletType == BulletType.Simple) {
				dist = distMax;
			}
			else if(bulletType == BulletType.Homing) {
				dir = tr.forward;
			}
			else if(bulletType == BulletType.Projectile) {
				startPos = tr.position;
				v.x = (target.position.x - startPos.x) / 2.0f; 
				v.z = (target.position.z - startPos.z) / 2.0f; 
				v.y = (target.position.y - startPos.y + 2.0f * gravity) / 2.0f;  
				lifeTimeEnabled = false;
				dir = v;
				dir.Normalize();
				tr.rotation = Quaternion.LookRotation(dir); 
			}
			else {}

			fAge = 0.0f;
			Initialized = true;
			Live = true;
		}

		void Update () {
			if(!Initialized) return;
			if(!Live) return;

			fAge += Time.deltaTime;

			// if bullettype is simple
			if(bulletType == BulletType.Simple) {
				// move bullet to target
				tr.position += tr.forward * speed * Time.deltaTime;
				dist -= speed * Time.deltaTime;
				// if distance between target is less then hit range, then process end
				if (dist < hitRange)
					End();

				if(target == null) {
					Vector3 vPos = tr.position;
					if(vPos.y < 1.0f) {
						End();
					}
				}
			}
			else if(bulletType == BulletType.Homing) {

				// if bullet has target, rotate dir to target
				if (target) {
					Vector3 targetDir = (target.position + new Vector3(0,0.5f,0) - tr.position);	
					targetDir.Normalize();

					Quaternion qTarget = Quaternion.LookRotation(targetDir);
					tr.rotation = Quaternion.Slerp(tr.rotation, qTarget, Time.deltaTime * turnSpeed);
				}

				// move position
				tr.position += tr.forward * speed * Time.deltaTime;
				if (target) {

					// if distance between target is too far, reset target
					dist = Vector3.Distance(target.position + new Vector3(0,0.5f,0), tr.position);

					if(dist > 100.0f) {
						target = null;
					}
				}
				//Debug.Log ("Bullet dist:"+dist.ToString ());
			}
			else if(bulletType == BulletType.Projectile) {
				s.x = startPos.x + v.x * fAge; 
				s.y = startPos.y + v.y * fAge - 0.5f * gravity * fAge * fAge; 
				s.z = startPos.z + v.z * fAge; 
				dir = s-tr.position;
				dir.Normalize();
				tr.rotation = Quaternion.LookRotation(dir); 
				tr.position = s; 

				dist = Vector3.Distance(target.position, tr.position);
			}
			else {}

			if ((lifeTimeEnabled && (fAge > lifeTime)) || (HitCheck && (dist < hitRange)))
				End();
		}

		void End() {
			if(!Live || !Initialized) return;

			Initialized = false;
			Live = false;

			// if range attack enabled
			if(broadAttackEnabled) {
				// find all enemy with in broad attack range
				Collider [] colliders = Physics.OverlapSphere(tr.position, broadAttackRange);
				foreach(Collider col in colliders) {
					if(col.gameObject.tag == EnemyTag) {
						Vector3 vDir = col.transform.position - tr.position;
						// do damage
						DamageApply(col.gameObject, vDir, tr.position, vDir * force);
					}
				}
			}
			else {
				//  do damage to target
				if(target) 
					DamageApply(target.gameObject, tr.forward, tr.position, tr.forward);
			}

			// if bullet type is simple
			if(bulletType == BulletType.Simple) {
				// unspawn immediatly
				BEObjectPool.Unspawn(gameObject);
			}
			// if not
			else {
				//give some delay needed
				StartCoroutine(Unspawn(0.0f));
			}
		}

		public IEnumerator Unspawn(float delay){
			if(delay > 0.0001f) yield return new WaitForSeconds(delay);
			BEObjectPool.Unspawn(gameObject);
		}

		public void DamageApply(GameObject go, Vector3 HitDir, Vector3 vPos, Vector3 vForce) {
			if(damage < 0.01f) return;

			// get health class from gameobject
			BEHealth targetHealth = go.GetComponent<BEHealth>();
			if (targetHealth) {
				// do damage
				targetHealth.OnDamage (damage, -HitDir);
			}

			//if (go.rigidbody) {
			//	go.rigidbody.AddForceAtPosition (vForce, vPos, ForceMode.Impulse);
			//}
		}	
	}
}
