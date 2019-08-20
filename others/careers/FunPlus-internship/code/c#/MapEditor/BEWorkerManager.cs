using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEWorkerManager
///   Description:    manage workers 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class BEWorkerManager : MonoBehaviour {

		public	static BEWorkerManager instance;

		public  BEAStar			AStar = null;
		public 	GameObject 		prefWorker;
		public 	Transform 		trUnitRoot;
		private int 			Count = 0;
		private	List<BEWorker> 	Workers = new List<BEWorker>();
		private	List<BEWorker> 	PathCalcList = new List<BEWorker>();

		void Awake () {
			instance=this;
		}

		void Start () {
		}
		
		void Update () {

			// if path calc list has item
			// call worker's GetPath and remove from the list
			if(PathCalcList.Count != 0) {
				BEWorker worker = PathCalcList[0];
				worker.GetPath();
				PathCalcList.RemoveAt(0);
			}
			
		}

		// add worker with given count
		public void CreateWorker(int _Count) {
			//Debug.Log ("CreateWorker "+_Count.ToString());
			Workers.Clear();
			Count = _Count;
			for(int i=0 ; i < Count ; ++i) {
				AddWorker();
			}
		}

		// add worker one by one
		public void AddWorker() {
			GameObject go =(GameObject)Instantiate(prefWorker, Vector3.zero, Quaternion.identity);
			go.transform.SetParent(trUnitRoot);
			go.transform.localScale = Vector3.one;
			int tileX = Random.Range(0,AStar.width);
			int tileZ = Random.Range(0,AStar.height);
			BEWorker script = go.GetComponent<BEWorker>();
			script.Init(this, tileX, tileZ);
			Workers.Add (script);
			//Debug.Log ("AddWorker "+go.transform.localPosition.ToString());
		}

		// if worker need new destination
		// add worker to path calc list
		// to avoid call astar's path find function simultaneously
		public void RequestPath(BEWorker worker) {
			PathCalcList.Add(worker);
		}

		// when tle info changed set MovePathRecalc flag of all worker
		public void OnTileInfoChanged() {
			for(int i=0 ; i < Workers.Count ; ++i) {
				Workers[i].MovePathRecalc = true;
			}
		}

		// allocate worker to building
		public void SetWorker(Building building) {
			for(int i=0 ; i < Workers.Count ; ++i) {
				if(!Workers[i].HasWork()) {
					Workers[i].SetWork(building);
					return;
				}
			}
		}

		// is free worker exist
		public bool WorkerAvailable() {
			for(int i=0 ; i < Workers.Count ; ++i) {
				if(!Workers[i].HasWork())
					return true;
			}
		
			return false;
		}

		// get free worker
		public BEWorker GetAvailableWorker() {
			for(int i=0 ; i < Workers.Count ; ++i) {
				if(!Workers[i].HasWork())
					return Workers[i];
			}
			
			return null;
		}

		// get free worker count
		public int GetAvailableWorkerCount() {

			int iReturn = 0;
			for(int i=0 ; i < Workers.Count ; ++i) {
				if(!Workers[i].HasWork())
					iReturn++;
			}
			
			return iReturn;
		}
	}
}
