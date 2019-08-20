using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BETurret
///   Description:    implementation of tower function.
/// 				  attack enemy with in attack range
///                   create unit if unit relative variables was set
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.1 (2015-09-26)
///                   - targetOrder added
///-----------------------------------------------------------------------------------------
namespace BE {

	public enum TargetOrder {
		FirstIn		= 0,
		Strongest 	= 1, // highest health 
		Weakest 	= 2, // lowest health
	};

	public class BETurret : MonoBehaviour {

		private GameObject 	goTarget = null;	// target enemy
		private Transform	target = null;		// target enemy transform
		public	Transform	Head;				// head transform which rotate to enemy
		public	GameObject	Range;				// game object that diaplay attack range
		public	BEWeapon	Weapon;
		public	TargetOrder	Order = TargetOrder.FirstIn;

		public	float		AttackRange = 4.0f;
		public  int			Type = 0;			// index in Towers at TDDatabase
		public  int			Level = 1;			// level of tower this is index in Defs array at TowerType class

		[HideInInspector]
		public	bool		PreviewMode = false;	// is in preview mode
		[HideInInspector]
		public	bool		RangeShow = false;		// is range gameobject visible

		// related to unit management
		//private Vector3		vOrigin = Vector3.zero;		// origin of units origin (user change this value by clicking flag button on circle menu)
		private List<BEUnit> Units=new List<BEUnit>();	// list of units created
		private float		UnitGenAge = 0.0f;			// age after last unit created
		private float		UnitGenPeriod = 15.0f;		// unit generation period
		public	GameObject	UnitPrefab;					// unit prefab
		public	int			UnitMax = 0;				// max unit count


		// temporary listing enemy to choose one by target order
		private List<BEHealth> Enemies=new List<BEHealth>();


		void Start () {
			// change range gameobject size according to attack range
			Range.transform.localScale = new Vector3(AttackRange*2.0f,1,AttackRange*2.0f);
			Range.SetActive (RangeShow);
		}
		
		void Update () {

			if(BEGround.scene.sceneType != SceneType.Battle) {
				return;
			}
		
			// if tower is in preview mode do nothing
			if(PreviewMode)
				return;

			// if no target and has head to rotate to enemy
			if((goTarget == null) && (Head != null)) {

				Enemies.Clear();

				// find all available enemy with in attack range 
				Collider [] colliders = Physics.OverlapSphere(Head.position, AttackRange);
				foreach(Collider col in colliders) {
					// if object's tag is enemy and mov type is available
					if((col.gameObject.tag == Weapon.EnemyTag)) { /// && (Weapon.MovTypeMatch(col.gameObject))) {
						// add to list
						Enemies.Add(col.gameObject.GetComponent<BEHealth>());
						break;
					}
				}

				// if enemy found choose enemy by target order
				if(Enemies.Count != 0) {

					// set first enemy to selected
					GameObject goEnemySelected = null;
					goEnemySelected = Enemies[0].gameObject;

					// in case first in use first item of list to target
					if(Order == TargetOrder.FirstIn) {
					}
					// if target order is strongest, find upper health enemy
					else if(Order == TargetOrder.Strongest) {
						float health = Enemies[0].health;
						for(int i=1 ; i < Enemies.Count ; ++i) {
							if(Enemies[i].health > health) {
								health = Enemies[i].health;
								goEnemySelected = Enemies[i].gameObject;
							}
						}
					}
					// if target order is weakest, find lower health enemy
					else if(Order == TargetOrder.Weakest) {
						float health = Enemies[0].health;
						for(int i=1 ; i < Enemies.Count ; ++i) {
							if(Enemies[i].health < health) {
								health = Enemies[i].health;
								goEnemySelected = Enemies[i].gameObject;
							}
						}
					}
					else {}

					// set target 
					if(goEnemySelected != null)
						TargetSet(goEnemySelected);
				}

			}
			// if tower has target
			else {
				// if target is too far to attack then reset target
				if((Head!= null) && (AttackRange < Vector3.Distance(Head.position, target.position))) {
					TargetReset();
				}
			}

			// if tower has target 
			if(goTarget != null) {

				// rotate head to target
				float speed = 10.0f;
				BoxCollider bc = goTarget.GetComponent<BoxCollider>();
				Vector3 targetDir = target.position + bc.center - Head.position;
				float step = speed * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards(Head.forward, targetDir, step, 0.0f);
				//Debug.DrawRay(Head.position, newDir, Color.red);
				Head.rotation = Quaternion.LookRotation(newDir);

				// id not in fire, then fire
				if((Weapon != null) && !Weapon.firing)
					Weapon.StartFire();
			}
			// if tower has no target
			else {
				// stop fire
				if((Weapon != null) && Weapon.firing)
					Weapon.StopFire();
			}

			// update range object visible state
			Range.SetActive (RangeShow);

			// if tower has unit
			if(UnitMax != 0) {
				// calc unit generation time
				UnitGenAge += Time.deltaTime;
				// if unit creation available
				if((UnitGenAge > UnitGenPeriod) && (Units.Count < UnitMax)) {
					// create unit
					UnitGen();
				}
			}
		}

		public void Init() {
			if(PreviewMode) return;

			// if tower has unit
			if(UnitMax == 0) return;

			// create unit to max size
			Units.Clear();
			//vOrigin = transform.position + Quaternion.Euler(0,Random.Range (0,360),0) * new Vector3(0,0,1) * AttackRange * 0.6f;
			for(int i=0 ; i < UnitMax ; ++i) {
				UnitGen();
			}
		}

		// create unit process
		public void UnitGen() {
			if(UnitPrefab == null) return;

			// unspawn unit prefab
			///GameObject go = BEObjectPool.Spawn(UnitPrefab, BESpawn.instance.transform, transform.position, Quaternion.identity);
			GameObject go = BEObjectPool.Spawn(UnitPrefab, transform.parent, transform.position, Quaternion.identity);
			BEUnit unit = go.GetComponent<BEUnit>();
			//unit.trParent = transform;

			// set unit's owner to this tower
			//unit.ownerTurret = this;
			// set unit;s origin values
			//int Angle = (360/UnitMax * Units.Count);
			//unit.SetOrigin(true, vOrigin + Quaternion.Euler(0,Angle,0) * new Vector3(0,0,1), AttackRange + 1.0f);
			//unit.Init();

			Units.Add(unit);
			// reset unit gen age to zero
			UnitGenAge = 0.0f;
		}

		public void UnitReset() {
			for(int i=0 ; i < Units.Count ; ++i) {
				// unspawn all unit 
				Units[i].gameObject.GetComponent<BEHealth>().Dead();
			}
			//unit list clear
			Units.Clear();
		}

		// when unit dead call this funtion
		public void UnitDestroyed(BEUnit unit) {

			// remove dead unit
			Units.Remove(unit);
			//Debug.Log ("UnitDestroyed UnitCount:"+Units.Count.ToString ());
		}

		// when user clicked ground after select flag button on circle manu
		public void SetOrigin(Vector3 origin) {
			// set origin value to new one
			//vOrigin = origin;
			for(int i=0 ; i < Units.Count ; ++i) {
				//int Angle = 360/UnitMax * i;
				// apply origin value to units
				//Units[i].SetOrigin(true, vOrigin + Quaternion.Euler(0,Angle,0) * new Vector3(0,0,1), AttackRange + 1.0f);
			}
		}

		// if unit's target is available
		public bool UnitTargetCheck(BEUnit unit, GameObject go) {
			bool bReturn = true;
			for(int i=0 ; i < Units.Count ; ++i) {
				if(Units[i] == unit) continue;

				// gameobject requested by unit is already target of other unit
				if(Units[i].goTarget == go) {
					// the return false
					bReturn = false;
					break;
				}
			}
			//Debug.Log ("UnitTargetCheck "+bReturn.ToString ());
			return bReturn;
		}
		
		public void TargetSet(GameObject go) {
			goTarget = go;
			target = goTarget.transform;
			Weapon.goTarget = go;
		}
		
		public void TargetReset() {
			goTarget = null;
			target = null;
			Weapon.goTarget = null;
		}

		public void SetPreviewMode(bool Enable) {
			PreviewMode = Enable;
		}

		public void UpgradeTo(BETurret newTurret) {
			// if turret has unit
			if(UnitMax != 0) {
				UnitReset();
			}
		}

		public void SetRangeShow(bool Enable) {
			RangeShow = Enable;
			Range.SetActive (RangeShow);
		}
	}

}