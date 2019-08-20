using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEWorker
///   Description:    worker is npc it wonder map and go to building and play working animation when building is working 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public enum BEWorketState {
		None,
		Idle,
		Move,
		Work,
	}
	
	public class BEWorker : MonoBehaviour {
		public 	BEWorkerManager	Manager = null;
		public	BEAStar 		AStar = null;
		public	List<Vector3> 	subPath = new List<Vector3>();
		public	int 			subPathID=0;
		public  Transform		tr;
		public 	BEWorketState	state = BEWorketState.Idle;
		public	Vector3			vDir;
		public 	float 			rotateSpeed=5.0f;
		public 	float 			moveSpeed=2.0f;
		private Vector3 		posByPath = Vector3.zero; 	// original position from path

		public 	bool			MovePathRecalc = false;
		public 	Vector2			MoveTargetTile = Vector2.zero;
		public 	Building		WorkBuilding = null;
		public 	bool			WorkInitialized = false;
		public 	float			IdlePeriod = 0.0f;

		public  bool			bJumped = false;
		public  float			JumpAge = 0.0f;
		public  float			JumpHeight = 0.0f;

		public 	Animation		anim = null;
		public 	AnimationClip 	aniIdle = null;		// animatioclip for idle state
		public 	AnimationClip 	aniMove = null;		// animatioclip for move state
		public 	AnimationClip 	aniWork = null;		// animatioclip for build state


		void Start () {
			tr = transform;
			GetPathSub();
		}
		
		// Update is called once per frame
		void Update() {

			float deltaTime = Time.deltaTime;

			// work building is exist
			if(WorkBuilding != null) {

				// first time
				if(!WorkInitialized) {
					// Get Target Building's AStar and move to that point
					AStarTile tileBuilding = BEGround.instance.GetBuidingAStarTile(WorkBuilding);
					MoveTargetTile = new Vector2(tileBuilding.x,tileBuilding.y);
					GetPathRecalc();
					WorkInitialized = true;
				}
				else {
					// if work is done, then wandering random map point
					if(BuildingWorkDone())
						return;
				}
			}

			if(state == BEWorketState.None) {
			}
			else if(state == BEWorketState.Idle) {
				// keep idle animation while idle period
				IdlePeriod -= deltaTime;
				if(IdlePeriod < 0.0f) {
					Manager.RequestPath(this);
					SetState(BEWorketState.None);
				}
			}
			else if(state == BEWorketState.Move) {
				float fMoveDistance = moveSpeed * deltaTime;
				Move(fMoveDistance);
			}
			else if(state == BEWorketState.Work) {

				// if work is done, then wandering random map point
				BuildingWorkDone();
			}
			else {}
		}

		public bool BuildingWorkDone() {
			if((WorkBuilding != null) && !WorkBuilding.IsWorking()) {
				WorkBuilding = null;
				Manager.RequestPath(this);
				SetState(BEWorketState.None);
				return true;
			}

			return false;
		}

		// initialize worker with AStar tile pos
		public void Init(BEWorkerManager _Manager, int x, int z) {
			Manager = _Manager;
			AStar = Manager.AStar;
			vDir = new Vector3(0,0,1);
			SetPosition(x,z);
		}

		public bool HasWork() {
			return ((WorkBuilding != null) || (state == BEWorketState.Work)) ? true : false;
		}

		public void SetPosition(int x, int z) {
			//Debug.Log ("SetPosition ("+x.ToString ()+","+z.ToString ()+")");
			tr = transform;
			tr.localPosition = AStar.GetTilePos(x,z);
			posByPath = tr.localPosition;
		}

		// move process
		public void Move(float fMoveDistance) {

			if(subPath.Count == 0) return;

			// if recalc flag setted
			if(MovePathRecalc) {
				// check any tile of worker's path have occupied
				bool bPathBlocked = false;
				for(int i=subPathID ; i < subPath.Count ; ++i) {
					if(AStar.GetTile (subPath[i]).type != 0) {
						// in case finded
						bPathBlocked = true;
						break;
					}
				}

				// reset flag
				MovePathRecalc = false;

				// if blocked tile in path finded, then recalc path
				if(bPathBlocked) {
					//Debug.Log ("Worker GetPathRecalc");
					GetPathRecalc();
					return;
				}
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
						MoveEnd();
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

/*					// if worker move above wall, jump worker 
					Vector2 TileCurrent = BEGround.instance.GetTilePos(tr.position, new Vector2(1,1));
					Building building = BEGround.instance.GetBuilding((int)TileCurrent.x, (int)TileCurrent.y);
					if((building != null) && (building.Type == 2)) {
						if(!bJumped) {
							bJumped = true;
							JumpAge = 0.0f;
							JumpHeight = 0.0f;
						}
					}

					if(bJumped) {
						Vector3 vPosUpjusted = new Vector3(posByPath.x,JumpHeight,posByPath.z);
						tr.position = vPosUpjusted;

						JumpAge += Time.deltaTime;
						JumpHeight += (JumpAge < 0.5f) ? 4.0f * Time.deltaTime : -4.0f * Time.deltaTime;
						Debug.Log ("JumpHeight:"+JumpHeight.ToString()+",JumpAge:"+JumpAge.ToString ());
						if(JumpHeight < 0.0f) JumpHeight = 0.0f;

						if(JumpAge > 1.0f)
							bJumped = false;
					}
*/				}
			}
		}
		
		// call when path move end
		void MoveEnd() {

			if(WorkBuilding != null) {
				SetState(BEWorketState.Work);
			}
			else {
				// choose next state, idle:move ratio is 3:7
				int iRandom = UnityEngine.Random.Range(0,10);
				if(iRandom < 3) {
					SetState(BEWorketState.Idle);
				}
				else {
					Manager.RequestPath(this);
					SetState(BEWorketState.None);
				}
			}
		}

		public void GetPath() {
			GetPathSub();
			while(subPath.Count <= 1)
				GetPathSub();
		}

		// find new path - call this function incase tile occupy info changed(if building land changes)
		void GetPathRecalc() {
			subPath.Clear();
			AStarTile end = AStar.tiles[(int)MoveTargetTile.x,(int)MoveTargetTile.y];
			AStarTile start = AStar.GetTile (tr.localPosition);
			int startX = start.x;
			int startZ = start.y;

			// if start tile is occupied, then find blank tile while increase z coordinate idx
			// and move to that tile with priority
			while(start.type != 0) {
				startZ += 1;
				start = AStar.tiles[startX, startZ];
			}
			Vector3 vNew = AStar.GetTilePos(startX,startZ);
			subPath.Add (vNew);

			// then find path with start point with that empty tile
			bool bSuccess = AStar.PathFind(start, end, null);
			if(bSuccess) {
				// add finded tile position to subpath array 
				for(int i=0 ; i < AStar.Path.Count ; ++i) {
					vNew = AStar.Path[i];
					subPath.Add (vNew);
				}
				subPathID = 0; // set initial subpath offset to zero
				SetState(BEWorketState.Move);
			}
			else {
				// if no available path founded, work on current position
				SetState(BEWorketState.Work);
			}
		}	

		// find new path
		void GetPathSub() {
			AStarTile end = AStar.FindEmpty(-1,-1);
			MoveTargetTile = new Vector2(end.x, end.y);

			AStarTile start = AStar.GetTile(tr.localPosition);
			// find path with start point
			bool bSuccess = AStar.PathFind(start, end, null);
			if(bSuccess) {
				subPath.Clear();
				// add finded tile position to subpath array 
				for(int i=0 ; i < AStar.Path.Count ; ++i) {
					Vector3 vNew = AStar.Path[i];
					subPath.Add (vNew);
				}
				subPathID = 0;
				SetState(BEWorketState.Move);
			}
		}
		
		public void SetState(BEWorketState _state) {

			state = _state;

			if(state == BEWorketState.Idle) {
				if(anim && aniIdle) 
					anim.Play(aniIdle.name);

				// set idle period randomly
				IdlePeriod = UnityEngine.Random.Range(3.0f, 10.0f);
			}
			else if(state == BEWorketState.Move) {
				if(anim && aniMove) 
					anim.Play(aniMove.name);
			}
			else if(state == BEWorketState.Work) {
				if(anim && aniWork) 
					anim.Play(aniWork.name);
			}
			else {}
		}

		public void SetWork(Building building) {
			WorkBuilding = building;
			WorkInitialized = false;
		}

		// draw subpath array
		void OnDrawGizmos(){
			if(subPath.Count < 2) return;

			Gizmos.color = Color.yellow;
			
			for(int i=0 ; i < subPath.Count-1 ; ++i) {
				//Gizmos.DrawWireSphere(units[i].vDest, 0.1f);
				Gizmos.DrawLine(subPath[i], subPath[i+1]);
			}

		}

	}

}