using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEUnitManager
///   Description:    
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-01-30)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class BEUnitManager : MonoBehaviour {

		public	static BEUnitManager instance;
		
		public 	Transform 				trUnitRoot;
		public	List<List<BEUnit>>		UnitGroups = new List<List<BEUnit>>();
		private int						uuid = 0;

		public 	List<List<Building>>	Buildings = new List<List<Building>>();	// array of buildings categorized by building type

		void Awake () {
			instance=this;

			// Set UnitGroup Count to 16
			for(int i=0 ; i < 16 ; ++i) {
				UnitGroups.Add (new List<BEUnit>());
			}
		}
		
		void Start () {
		}
		
		void Update () {
		}

		public BEUnit Add(int GroupID, int UnitID, int Level, Vector3 vPos, Quaternion qRot) {
			//ArmyType at = TBDatabase.GetArmyType(UnitID);
			ArmyDef ad = TBDatabase.GetArmyDef(UnitID, Level);

			GameObject go = (GameObject)Instantiate(ad.prefab, vPos, qRot);
			go.transform.SetParent (trUnitRoot);
			go.transform.localPosition = vPos;
			go.transform.localScale = Vector3.one;

			BEUnit script = go.GetComponent<BEUnit>();
			script.Init (GroupID, UnitID, Level);
			script.uuid = uuid;
			uuid++;
			UnitGroups[GroupID].Add (script);

			if(script.eMovType == MovType.Air)
				go.transform.localPosition = vPos + new Vector3(0,2,0);

			return script;
		}
		
		public void RemoveAll() {
			for(int i=0 ; i < 16 ; ++i) {
				for(int j=0 ; j < UnitGroups[i].Count ; ++j) {
					UIInGame.instance.RemoveInGameUI(UnitGroups[i][j].transform);
					Destroy(UnitGroups[i][j].gameObject);
				}
				UnitGroups[i].Clear();
			}
		}

		public void UnitDestroyed(BEUnit unit) {
			if((unit.GroupID < 0) || (16 <= unit.GroupID)) return;

			int idx = UnitGroups[unit.GroupID].FindIndex(x => (x == unit));
			if(idx != -1) {
				UnitGroups[unit.GroupID].RemoveAt(idx);
				//Debug.Log ("UnitDestroyed group "+unit.GroupID);
			}
		}

		public int GetLiveUnitCount() {

			int LiveCount = 0;
			for(int i=0 ; i < 16 ; ++i) {
				LiveCount += UnitGroups[i].Count;
			}
			return LiveCount;
		}
		
		public GameObject TargetFind(BEUnit unit) {
			Vector3 vPosUnit = unit.transform.position;
			Building scriptNearest = null;
			float fDistanceMin = 0.0f;
			for(int i=0 ; i < TBDatabase.GetBuildingCount() ; ++i) {

				BuildingType bt = TBDatabase.GetBuildingType(i);
				if(unit.ePreferType == TargetType.Building) {
					if((bt.Category == TargetType.Deco) || (bt.Category == TargetType.Wall)) continue;
				}
				//else if(unit.ePreferType == TargetType.Resource) {
				//}

				List<Building> buildingList = BEGround.instance.Buildings[i];
				for(int j=0 ; j < buildingList.Count ; ++j) {

					Building building = buildingList[j];
					if(building.goCenter == null) continue;

					float fDistance = Vector3.Distance(vPosUnit, buildingList[j].transform.position);
					if((fDistanceMin < 0.001f) || (fDistance < fDistanceMin)) {
						fDistanceMin = fDistance;
						scriptNearest = buildingList[j];
					}
				}

			}

			return (scriptNearest != null) ? scriptNearest.goCenter : null;
		}
			
	}
}