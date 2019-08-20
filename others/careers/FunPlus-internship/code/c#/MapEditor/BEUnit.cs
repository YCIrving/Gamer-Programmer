using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEUnit
///   Description:    implement unit movement & fight logic
///                   unit has origin position and can move with in originrange
///                   when unit is in idle state, find enemy with find range
///                   if enemy found, move to enemy until distance between enemy  and unit is
///                   less then attack range.
///                   then fire to enemy until they die
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.2 (2015-09-30)
///                   - animation added
///                   - dead sound added
///                   v1.3 (2016-01-07)
///                   - aniWalk apply bug fixed
///-----------------------------------------------------------------------------------------
namespace BE {

	// enemy move type
	public enum MovType {
		Ground 		= 0,
		Air 		= 1,
		Both 		= 2,
	};
	
	public enum TargetType {
		Building 	= 0,
		Deco	 	= 1,
		Resource 	= 2,
		Tower 		= 3,
		Wall 		= 4,
	};

	public enum UnitState {
		Idle 			= 0,
		Move	 		= 1,
		Chase 			= 2,
		Attack 			= 3,
		KeepPosition 	= 4,
	};

	[RequireComponent (typeof (BEHealth))]
	public class BEUnit : MonoBehaviour {

		public 	int				uuid = 0;
		public  int 			GroupID = 0;
		public 	int				Type = 0;
		public 	int				Level = 1;
		public  ArmyType		at = null;
		public  ArmyDef			def = null;

		public	BEAStar 		AStar = null;
		private AStarTile		tileCurrent = null;
		public	List<Vector3> 	subPath = new List<Vector3>();
		public	int 			subPathID = 0;
		public 	Vector2			MoveTargetTile = Vector2.zero;
		public	Vector3			vDir;
		private Vector3 		posByPath = Vector3.zero; 	// original position from path
		public	Building		frontWall;
		public	GameObject 		goSubTarget = null;		//
		public  Transform		trSubTarget = null;		//

		public  bool			InSideMove = false;
		public  float			SideMoveLife = 0.0f;
		public  Vector3 		vSideDir = Vector3.zero;
		public  bool			SideRight = true;

		public	UnitState		state = UnitState.Idle;
		//[HideInInspector]
		public	GameObject 		goTarget = null;		// target enemy gameobject
		public  Transform		trTarget = null;		// target enemy's transform
		private Vector3			vTargetOffset = Vector3.zero;	// 
		public	TargetType		ePreferType = TargetType.Building;
		[HideInInspector]
		private Transform		tr;
		private BEHealth		Health;
		public	BEWeapon		Weapon;
		public	GameObject 		goSelectMark = null;	// selected mark gameobject

		public	float			FindRange = 5.0f;		// find enemy range
		public	float			AttackRange = 3.0f;		// attack enabled range

		// variables for unit movement
		private float			fMoveDistance = 0.0f;
		public 	float 			rotateSpeed = 5.0f;
		public 	float 			moveSpeed = 5.0f;
		public	MovType			eMovType = MovType.Ground;

		private RaycastHit 		hitInfo;		// raycast to find enemy hit test

		public 	Animation		anim;
		public 	AnimationClip 	aniIdle;		// animatioclip for idle state
		public 	AnimationClip 	aniWalk;		// animatioclip for move state
		public 	AnimationClip 	aniAttack;		// animatioclip for attack state
		public 	AnimationClip	aniDead;		// animatioclip for dead state
		
		public 	AudioClip 		audioDead;		// audioclip plays when unit dead

		void Awake () {
			tr = transform;
			Health = GetComponent<BEHealth>();
			Health.DeadNotifier = "DeadNotifier";

			AStar = BEGround.instance.AStar;

			SideRight = (UnityEngine.Random.Range (0,2) == 0) ? true : false;
		}
		
		void Start () {
			if(goSelectMark != null)
				goSelectMark.SetActive(false);
		}
		
		void Update () {

			if(Health.dead) {
				return;
			}

			float deltaTime = Time.deltaTime;
			float fDelta = moveSpeed * deltaTime;

			if(state == UnitState.Idle) {
				// if unit has animation, play
				if (anim && aniIdle) {
					anim.Play(aniIdle.name);
				}
			}
			else if(state == UnitState.Move) {
				if(subPath.Count == 0) {
					state = UnitState.Idle;
					return;
				}

				// if unit has animation, play
				if (anim && aniWalk) {
					anim.Play(aniWalk.name);
				}

				//while mov distance is exit
				while(fMoveDistance > 0.001f) {
					Vector3 posByPathNew = subPath[subPathID];
					float newDistance = Vector3.Distance(posByPathNew, posByPath);
					
					// mov distance at this time
					float MoveSegment = 0.0f;
					
					// come to end of this waypoint
					if(newDistance < fMoveDistance) {
						MoveSegment = newDistance;
						
						// if last of subpath array
						if(++subPathID >= subPath.Count){
							// do move done process
							state = UnitState.Idle;
							return;
						}
					}
					else {
						MoveSegment = fMoveDistance;
					}
					
					fMoveDistance -= MoveSegment;
					
					//rotate object direction to mov direction
					Vector3 dir = (posByPathNew-posByPath).normalized;
					if(dir.magnitude > 0.01f) {
						Quaternion wantedRot = Quaternion.LookRotation(dir);
						tr.rotation = Quaternion.Slerp(tr.rotation, wantedRot, rotateSpeed*Time.deltaTime);
						posByPath = posByPath + dir*MoveSegment;
						tr.position = posByPath;
					}
				}
			}
			else if(state == UnitState.Chase) {
				//Debug.Log ("BEUnit InMove pos:"+tr.position);
				if(goTarget == null) {
					TargetFind();
					return;
				}

				Vector3 vDir = trTarget.position - tr.position;
				vDir.y = 0.0f;
				vDir.Normalize();

				Transform trFinal = (goSubTarget != null) ? trSubTarget : trTarget;

				// if distance between target and unit is less then attack range
				if(Vector3.Distance(tr.position, trFinal.position) < AttackRange+2.0f) {

					if(eMovType == MovType.Ground) {
						Ray ray = new Ray(tr.position+new Vector3(0,0.1f,0),vDir);
						RaycastHit hit;
						if (Physics.Raycast (ray.origin, ray.direction, out hit, AttackRange+2.0f)) {
							//Debug.Log ("hit.collider "+hit.collider.name);
							if(hit.collider.gameObject == goTarget) {
								//only there is no obstacle to the target 
								state = UnitState.Attack;
								return;
							}
						}
					}
					else {
						state = UnitState.Attack;
						return;
					}
				}

				AStarTile tileCurrent = BEGround.instance.AStar.GetTile(tr.position);

				//find next tile

				// get front tile
				Vector3 vPosNew = tr.position + vDir * (fDelta + 0.5f); // a* tile size
				AStarTile tileNext = BEGround.instance.AStar.GetTile(vPosNew);
				bool bTargetTemp = false;
				//bool WallExist = false;
				//if next tile was blocked, change mov dir to move to perpendicular to target
				if((eMovType == MovType.Ground) && (tileNext != tileCurrent) && (tileNext.type != 0)) {

					//Vector2 vTile0 = BEGround.instance.GetTilePos(tr.position, Vector2.one);
					Vector2 vTile = BEGround.instance.GetTilePos(tr.position + vDir, Vector2.one);
					frontWall = BEGround.instance.GetBuilding((int)vTile.x, (int)vTile.y);
					//WallExist = true;

					Vector3 vTempDir = Vector3.zero;
					if(Mathf.Abs(vDir.x) > Mathf.Abs(vDir.z)) {
						if(Mathf.Abs(vDir.z) < 0.01f) { bTargetTemp = true; }
						else { vTempDir = (vDir.z > 0.0f) ? new Vector3(0,0,1) : new Vector3(0,0,-1); }
					}
					else {
						if(Mathf.Abs(vDir.x) < 0.01f) { bTargetTemp = true; }
						else { vTempDir = (vDir.x > 0.0f) ? new Vector3(1,0,0) : new Vector3(-1,0,0); }
					}

					//Debug.Log ("vTempDir "+vTempDir.ToString ());
					// get modified next front tile
					Vector3 vPosChanged = tr.position + vTempDir * (fDelta + 0.5f);
					AStarTile tileNextChanged = BEGround.instance.AStar.GetTile (vPosChanged);
					if((tileNextChanged != tileCurrent) && (tileNextChanged != null) && (tileNextChanged.type == 0)) {
						vDir = vTempDir;
					}
					vDir.Normalize();
				}
				else {
					frontWall = null;
				}

				// already perpendicular to target
				if(bTargetTemp && (frontWall != null) && (frontWall.goCenter != null) && (frontWall.goCenter != goTarget)) {
					// if front blocked building is not target then set as a target
					SubTargetSet(frontWall.goCenter);
					state = UnitState.Attack;
					return;
				}

				// Local Avoid
				GameObject otherSubTarget = null;
				BEUnit unitOther = null;
				int OtherCount = 0;
				Vector3 vOthertoMe = Vector3.zero;
				bool bAllStopped = true;
				{
					//local avoid
					float CROWDING = 0.5f;//0.7f;
					Collider [] colliders = Physics.OverlapSphere(tr.position, CROWDING);
					foreach(Collider col in colliders) {

						// if object's tag was enemy & enemy's mov type is attackable?
						if((col.gameObject != gameObject) && (col.gameObject.tag == "Unit")) { // && (Weapon.MovTypeMatch(col.gameObject))) {

							Vector3 vToOther = col.transform.position - tr.position;
							float angle = Vector3.Angle(vDir, vToOther);
							unitOther = col.gameObject.GetComponent<BEUnit>();
							//Debug.Log ("angle "+angle);
							if((angle < 80.0f) && (unitOther.eMovType == eMovType)) {

								if(bAllStopped && (unitOther.state == UnitState.Chase))
									bAllStopped = false;

								if(unitOther.goSubTarget != null) 
									otherSubTarget = unitOther.goSubTarget;

								Vector3 vOtherDir = tr.position - col.transform.position;
								vOtherDir.Normalize();
								vOthertoMe += vOtherDir;
								OtherCount++;
							}
						}
					}

					vOthertoMe.y = 0.0f;
					vOthertoMe.Normalize();
				}

				Vector3 vFinal = vDir;
				if(OtherCount > 0) {
					float angle2 = Vector3.Angle(vDir, vOthertoMe);
					if((angle2 > 160.0f) && (unitOther.state != UnitState.Chase)) {
						if(unitOther.frontWall != null) {
							SubTargetSet(otherSubTarget);
							state = UnitState.Attack;
							return;
						}
						else {
							vOthertoMe = Quaternion.Euler(0,SideRight ? 90.0f : -90.0f,0.0f) * vDir - vFinal;
						}
					}
					else if((OtherCount >= 2) && bAllStopped) {
						SubTargetSet(otherSubTarget);
						state = UnitState.Attack;
						return;
					}
					else {}

					//Debug.Log ("vOthertoMe "+vOthertoMe);
					vFinal += vOthertoMe;// * 0.2f;
				}
				vFinal.y = 0.0f;
				vFinal.Normalize();

				//Debug.Log ("vFinal "+vFinal);
				Vector3 vPosNew2 = tr.position + vFinal * fDelta;
				if(eMovType == MovType.Ground) {
					AStarTile tileNext2 = BEGround.instance.AStar.GetTile(vPosNew2+vFinal*0.5f);
					if(tileNext2.type != 0) {
						if(frontWall) {
							SubTargetSet(frontWall.goCenter);
							state = UnitState.Attack;
						}
						return;
					}
				}

				tr.position = vPosNew2;

				if((goTarget != null) || (goSubTarget != null)) {
					
					// get dir to target
					//Transform trFinal = (goSubTarget != null) ? trSubTarget : trTarget;

					Vector3 targetDir = trFinal.position - tr.position;
					targetDir.Normalize();

					// change unit's rotation to target 
					Vector3 newDir = Vector3.RotateTowards(tr.forward, targetDir, rotateSpeed * Time.deltaTime, 0.0f);
					//Debug.DrawRay(tr.position, newDir, Color.red);
					tr.rotation = Quaternion.LookRotation(newDir);
				}
			}
			else if(state == UnitState.Attack) {
				if(goTarget == null) {
					TargetFind();
					return;
				}

				if((goTarget != null) || (goSubTarget != null)) {

					Transform trFinal = (goSubTarget != null) ? trSubTarget : trTarget;

					// get dir to target
					Vector3 targetDir = trFinal.position - tr.position;
					targetDir.Normalize();

					// change unit's rotation to target 
					Vector3 newDir = Vector3.RotateTowards(tr.forward, targetDir, rotateSpeed * Time.deltaTime, 0.0f);
					//Debug.DrawRay(tr.position, newDir, Color.red);
					tr.rotation = Quaternion.LookRotation(newDir);
					
					// if distance between target and unit is less then attack range
					if(Vector3.Distance(tr.position, trFinal.position) < AttackRange+2.0f) {
						// if not in fire, start fire
						if(!Weapon.firing) {
							Weapon.goTarget = (goSubTarget != null) ? goSubTarget : goTarget;
							Weapon.StartFire();
						}
						
						// if unit has animation, play
						if (anim && aniAttack) {
							anim.Play(aniAttack.name);
						}
					}
					else {
						state = UnitState.Chase;
					}
				}
				else {
					
					// if in fire, stop fire
					if(Weapon.firing) {
						Weapon.StopFire();
					}
					
					// if unit has idle animation, play idle animation
					if (anim && aniIdle) {
						anim.Play(aniIdle.name);
					}

					TargetFind();
				}
			}
			else if(state == UnitState.KeepPosition) {
				Vector3 vPosKeep = trTarget.position + vTargetOffset;
				float newDistance = Vector3.Distance(tr.position, vPosKeep);
				if(newDistance > 0.01f) {
					Vector3 vDir = vPosKeep - tr.position;
					vDir.y = 0.0f;
					vDir.Normalize();
					if(fDelta > newDistance) {
						tr.position = vPosKeep;
						state = UnitState.Idle;
					}
					else {
						tr.position += vDir * fDelta;

						// if unit has animation, play
						if (anim && aniWalk) {
							anim.Play(aniWalk.name);
						}
					}

					Vector3 newDir = Vector3.RotateTowards(tr.forward, vDir, rotateSpeed * Time.deltaTime, 0.0f);
					tr.rotation = Quaternion.LookRotation(newDir);
				}
			}
			else {}
		}

		// initialize
		public void Init(int groupID, int type, int level) {
			GroupID = groupID;
			Type = type;
			Level = level;
			at = TBDatabase.GetArmyType(type);
			def = TBDatabase.GetArmyDef(Type, Level);

			state = UnitState.Idle;
		}

		// when unit dead
		public void DeadNotifier() {
			// play dead sound
			BEAudioManager.SoundPlay(audioDead);

			// if unit has dead animation
			if (anim && aniDead) {
				// play dead animation
				anim.Play(aniDead.name);
				// call dead apply after dead animation end
				StartCoroutine(DeadApply(aniDead.length));
			}
			else {
				// call health's dead function (this will unspawn gameobject)
				Health.Dead();
			}

			BEUnitManager.instance.UnitDestroyed(this);
		}
		
		IEnumerator DeadApply(float fPeriod) {
			
			yield return new WaitForSeconds(fPeriod);
			
			// call health's dead function (this will unspawn gameobject)
			Health.Dead();
			if(tileCurrent != null) {
				tileCurrent.type2 -= 1;
			}
		}

		// find target available
		public void TargetFind() {
			GameObject go = BEUnitManager.instance.TargetFind(this);
			if(go != null) {
				TargetSet(go);
				//state = UnitState.Chase;
			}
		}

		// set target info
		// this function will be call when owner tower's UnitTargetCheck function returns ture
		// this means thet unit is main attacker of the target
		public void TargetSet(GameObject go) {
			goTarget = go;
			if(goTarget != null) {
				trTarget = goTarget.transform;
				state = UnitState.Chase;
			}
			else {
				trTarget = null;
			}

			goSubTarget = null;
			trSubTarget = null;
		}

		// reset target info
		public void TargetReset() {
			goTarget = null;
			trTarget = null;
			goSubTarget = null;
			trSubTarget = null;

			// if in fire, then stop
			if(Weapon.firing)
				Weapon.StopFire();
		}

		public void SubTargetSet(GameObject go) {
			goSubTarget = go;
			if(goSubTarget != null) {
				trSubTarget = goSubTarget.transform;
			}
			else {
				trSubTarget = null;
			}
		}

		public void SetState(UnitState _state, GameObject go, Vector3 vOffset, bool Immediately=false) {
			if(go != null) {
				TargetSet(go);
				vTargetOffset = vOffset;
			}

			state = _state;

			if((go != null)  && Immediately)
				tr.position = trTarget.position + vTargetOffset;
		}

		void OnDrawGizmos(){

			Gizmos.color = Weapon.firing ? Color.red : Color.yellow;
			if(goSubTarget != null) {
				Gizmos.DrawWireSphere(trSubTarget.position, 0.1f);
				Gizmos.DrawLine(tr.position, trSubTarget.position);
			}
			else if(goTarget != null) {
				Gizmos.DrawWireSphere(trTarget.position, 0.1f);
				Gizmos.DrawLine(tr.position, trTarget.position);
			}
			else {}

			if(subPath.Count < 2) return;
			
			for(int i=0 ; i < subPath.Count-1 ; ++i) {
				//Gizmos.DrawWireSphere(units[i].vDest, 0.1f);
				Gizmos.DrawLine(subPath[i], subPath[i+1]);
			}
		}

	}
	
}