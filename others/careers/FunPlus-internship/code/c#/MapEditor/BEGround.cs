using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wod.ThirdParty.Util;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEGround
///   Description:    class about tiled map for create and manage buildings 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class BEGround : MonoBehaviour {

		public	static BEGround  instance;
		public  static Building  buildingSelected = null;
		public  static List<Building> buildingMultiSelected = new List<Building>();
		public  static SceneBase scene = null;

		public	Plane 			xzPlane;
		public 	Vector2 		UnitSize = Vector2.one;	// actual width, height size of one tile
		public 	Vector2 		GridSize;				// map size 
		public 	int 			SubGridSize = 2;		// how many sub tiles in on tile (sub tiles are used when AStar path finding)
		public	BEAStar			AStar;					// a star path finding class
		public	BEWorkerManager	WorkerManager;			// control worker (build building, wondering map)
		public	Transform 		trDecoRoot = null;

		public 	Building [,] 			Cells;			// 2-dimensional array of each tile(cell)
		
		// dic of buildings categorized by building type
		public 	Dictionary<int, List<Building>>	Buildings = new Dictionary<int, List<Building>>();	

		public 	int 			TroopHousingSpaceTotal = 0;
		public 	int 			TroopCapacityTotal = 0;
		private bool 			WorkerCanPassWall = false;


		public void Awake () {
			instance=this;
			xzPlane = new Plane(new Vector3(0f, 1f, 0f), 0f);

			// initialize cells
			Cells = new Building[(int)GridSize.x,(int)GridSize.y];
			for(int y=0 ; y < (int)GridSize.y ; ++y) {
				for(int x=0 ; x < (int)GridSize.x ; ++x) {
					Cells[x,y] = null;
					// if cell is blank(not occupied) value is null
					// otherwise, cell has script of occupying building
				}
			}

			// assume max building type count
			for(int i=0 ; i < TBDatabase.GetBuildingCount() ; ++i) {
				Buildings.Add (TBDatabase.GetBuildingType(i).ID, new List<Building>());
			}

			// initialzize A Star map
			AStar.Init((int)GridSize.x*SubGridSize, (int)GridSize.y*SubGridSize, 1.0f/(float)SubGridSize);
		}

		void Update () {

			/*{
				int Count = 0;
				for(int j=0 ; j < Buildings[7].Count ; ++j) { Count += Buildings[7][j].UnitGenHousingSpaceTotal;   }
				for(int j=0 ; j < Buildings[8].Count ; ++j) { Count += Buildings[8][j].GetUnitHousingSpaceTotal(); }
				
				TroopHousingSpaceTotal = Count;
			}
			
			{
				int Count = 0;
				for(int j=0 ; j < Buildings[8].Count ; ++j) { 
					if(Buildings[8][j].def == null) continue;

					Count += Buildings[8][j].def.TroopCapacity; 
				}
				
				TroopCapacityTotal = Count;
			}*/
			
		}

		// get AStar tile from building script
		public AStarTile GetBuidingAStarTile(Building building) {
			int x = (int)building.tilePos.x * BEGround.instance.SubGridSize;
			int y = (int)building.tilePos.y * BEGround.instance.SubGridSize;
			return AStar.tiles[x,y];
		}

		// get boundary of map
		public Vector2 GetBorder(Vector2 tileSize) {
			Vector2 vReturn = Vector2.zero;
			vReturn.x = (GridSize.x-tileSize.x) * -0.5f * UnitSize.x;
			vReturn.y = (GridSize.y-tileSize.y) * -0.5f * UnitSize.y;
			return vReturn;
		}

		// ove gameobject to given tilepos and tile size(1x1,2x2,...)
		public void Move(GameObject go, Vector2 tilePos, Vector2 tileSize) {

			Vector3 localPos = TilePosToWorldPos(tilePos, tileSize);

			if(go != null) {
				go.transform.position = localPos;
				go.transform.rotation = transform.rotation;
			}
		}

		// get tilepos(x,y coordinate index) from actual position and tilesize(1x1,2x2,...)
		public Vector2 GetTilePos(Vector3 vTarget, Vector2 tileSize) {
			Vector2 tilePos = Vector2.zero;
			Vector3 posLocal = vTarget;//transform.InverseTransformPoint(pos);
			Vector2 border = GetBorder(tileSize);
			posLocal.x = Mathf.Clamp(posLocal.x, border.x, -border.x);
			posLocal.z = Mathf.Clamp(posLocal.z, border.y, -border.y);
			tilePos.x = (int)(posLocal.x - border.x)/(int)UnitSize.x;
			tilePos.y = (int)(posLocal.z - border.y)/(int)UnitSize.y;
			if(tilePos.x > GridSize.x-tileSize.x) tilePos.x = GridSize.x-tileSize.x;
			if(tilePos.y > GridSize.y-tileSize.y) tilePos.y = GridSize.y-tileSize.y;

			return tilePos;
		}
		
		public Vector2 GetTilePosInAddMode(Vector3 vTarget, Vector2 tileSize) {
//			Vector2 tilePos = Vector2.zero;
//			Vector3 posLocal = vTarget;//transform.InverseTransformPoint(pos);
//			Vector2 border = GetBorder(tileSize);
//			posLocal.x = Mathf.Clamp(posLocal.x, border.x, -border.x);
//			posLocal.z = Mathf.Clamp(posLocal.z, border.y, -border.y);
//			tilePos.x = (int)(posLocal.x - border.x)/(int)UnitSize.x;
//			tilePos.y = (int)(posLocal.z - border.y)/(int)UnitSize.y;
//			if(tilePos.x > GridSize.x-tileSize.x) tilePos.x = GridSize.x-tileSize.x;
//			if(tilePos.y > GridSize.y-tileSize.y) tilePos.y = GridSize.y-tileSize.y;
			
			Vector2 tilePos = Vector2.zero;
			tilePos.x = (int) (vTarget.x + GridSize.x * 0.5f);
			tilePos.y = (int) (vTarget.z + GridSize.y * 0.5f);

			return tilePos;
		}		

		// get actual position from tilepos(x,y coordinate index)
		public Vector3 TilePosToWorldPos(Vector2 tilePos, Vector2 tileSize) {

			Vector2 border = GetBorder(tileSize);
			if(tilePos.x > GridSize.x-tileSize.x) tilePos.x = GridSize.x-tileSize.x;
			if(tilePos.y > GridSize.y-tileSize.y) tilePos.y = GridSize.y-tileSize.y;
			
			Vector3 localPos = Vector3.zero;
			localPos.x = border.x+(int)(tilePos.x+0.5f)*UnitSize.x;
			localPos.y = 0.01f;
			localPos.z = border.y+(int)(tilePos.y+0.5f)*UnitSize.y;	

			return localPos;
		}

		// move building to proper position 
		// which has enough blank tiles to cover building's tilesize and nearest from current screen center
		// this function used when new building is created, set initial position to building
		public bool MoveToVacantTilePos(Building bd) {
			List<Vector2> posTile = new List<Vector2>();

			for(int y=0 ; y < (int)GridSize.y ; ++y) {
				for(int x=0 ; x < (int)GridSize.x ; ++x) {

					bool bOccupied = false;
					for(int v=0 ; v < (int)bd.tileSize.y ; ++v) {
						for(int u=0 ; u < (int)bd.tileSize.x ; ++u) {

							if(x+u >= (int)GridSize.x) continue;
							if(y+v >= (int)GridSize.y) continue;
							if(Cells[x+u,y+v] != null) {
								bOccupied = true;
								break;
							}
						}

						if(bOccupied) break;
					}

					if(bOccupied) {
						continue;
					}
					else {
						// has enough blank tiles cover builsing's tile size
						posTile.Add(new Vector2(x,y));
					}
				}
			}

			// no vacant tilepos found
			if(posTile.Count == 0)
				return false;

			// get xzplane position with camera raycast 
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			float enter;
			xzPlane.Raycast(ray, out enter);
			Vector3 vTarget = ray.GetPoint(enter);
			Vector2 TileCameraCenter = GetTilePos(vTarget, bd.tileSize);

			// sort nearest tile from screen center
			int iFind = -1;
			float fDistMin = 0.0f;
			for(int j=0 ; j < posTile.Count ; ++j) {
				float fDist = Vector2.Distance(posTile[j], TileCameraCenter);
				if((j==0) || (fDist < fDistMin)) {
					iFind = j;
					fDistMin = fDist;
				}
			}

			//iFind = 0;
			// sort listed point from camera canter to plane intersection
			bd.tilePos = posTile[iFind];
			Move(bd.gameObject, bd.tilePos, bd.tileSize);

			// if new building is outside of frustrum
			// move camera to show building
			Vector3 vPosCamera = TilePosToWorldPos(bd.tilePos, Vector2.one);
			GameObject.Find ("CameraRoot").transform.position = vPosCamera;
			return true;
		}

		//check tile is vacant with given tilepos and tilesize
		public bool IsVacant(Vector2 tilePos, Vector2 tileSize) {
			for(int y=0 ; y < (int)tileSize.y ; ++y) {
				for(int x=0 ; x < (int)tileSize.x ; ++x) {

					int cx = (int)tilePos.x+x;
					int cy = (int)tilePos.y+y;
					//Debug.Log ("IsVacant "+cx+","+cy);

					if((cx < 0) || (GridSize.x <= cx) || (cy < 0) || (GridSize.y <= cy))
						return false;

					if(Cells[cx,cy] != null)
						return false;
				}
			}

			return true;
		}

		// write tile cell was occupied by building
		public void OccupySet(Building bd, bool bSet) {

			if(scene.sceneType == SceneType.Battle) {
				if(bSet && (bd.bt.Category == TargetType.Deco)) 
					return;
			}

			for(int y=0 ; y < (int)bd.tileSize.y ; ++y) {
				for(int x=0 ; x < (int)bd.tileSize.x ; ++x) {
					Cells[(int)bd.tilePos.x+x,(int)bd.tilePos.y+y] = bd.Landed ? bd : null;

					//set subtile value for AStar map
					if(AStar.tiles != null) {
						int asx = SubGridSize * ((int)bd.tilePos.x+x);
						int asy = SubGridSize * ((int)bd.tilePos.y+y);
						for(int yy=0 ; yy < SubGridSize ; ++yy) {
							for(int xx=0 ; xx < SubGridSize ; ++xx) {

								if(bSet) {
									// like COC, worker moves through wall, so wall building not set it's tile occupation info to AStar
									if(WorkerCanPassWall) {
										if((bd.Type == 2) || ((x==0) && (xx==0)) || ((x==(int)bd.tileSize.x-1) && (xx==SubGridSize-1)) || ((y==0) && (yy==0)) || ((y==(int)bd.tileSize.y-1) && (yy==SubGridSize-1)))
											AStar.tiles[asx+xx,asy+yy].type = 0;
										else
											AStar.tiles[asx+xx,asy+yy].type = bd.Landed ? 1 : 0;
									}
									else {
										AStar.tiles[asx+xx,asy+yy].type = bd.Landed ? 1 : 0;
									}
								}
								else {
									AStar.tiles[asx+xx,asy+yy].type = 0;
								}
							}
						}
					}
				}
			}
		}

		// get building with tile x, y coorsinate index
		public Building GetBuilding(int x, int y) {
			if((x<0) || ((int)GridSize.x<=x)) return null;
			if((y<0) || ((int)GridSize.y<=y)) return null;

			return Cells[x, y];
		}

		// list whole buildinge created.
		// used when debugging
		public void BuildingListing() {
			//Debug.Log ("BEGround::BuildingListing");
			for(int i=0 ; i < TBDatabase.GetBuildingCount() ; ++i)
			{
				int BuildingID = TBDatabase.GetBuildingType(i).ID;
				for(int j=0 ; j < Buildings[BuildingID].Count ; ++j) {
					Debug.Log ("Building Type:"+i.ToString ()+ " "+Buildings[BuildingID][j].gameObject.name);
				}
			}
		}

		// get building count with given building type
		public int GetBuildingCount(int BuildingType) {
			return Buildings[BuildingType].Count;
		}

		// get max count of given building type from database
		public int GetBuildingCountMax(int BuildingType) {
			BuildingType 	bt = TBDatabase.GetBuildingTypeByID(BuildingType);
			//Debug.Log ("bt:"+bt.Name);
			Building 		buildingTown = Buildings[0][0];
			return bt.MaxCount[buildingTown.Level-1];
		}

		// for initial building batch
		public void BuildingAdd(int type, int level, Vector3 vPos) {
			Building script = BuildingAdd (type,level);
			script.Move(vPos);
			BuildingSelect(script);
			BuildingLandUnselect();
		}

		// create new building with type and level
		public Building BuildingAdd(int type, int level) {
			//Debug.Log ("BEGround::BuildingAdd");

			// if previous selected building is exist, unselect that building
			// because newly created building must be in selection state
			if(buildingSelected != null) {
				BuildingLandUnselect();
			}

			// create building base from resource
			// each buildings are combination of buildingbase and building mesh
			GameObject goBuildingBase = Resources.Load ("Prefabs/Building/BuildingBase") as GameObject;
			GameObject go = (GameObject)Instantiate(goBuildingBase, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (trDecoRoot);

			Building script = go.GetComponent<Building>();
			script.ground = this;
			// initialize building
			script.Init (type, level);
			LoggerHelper.Debug("Build: ID:"+(type).ToString()+" ,level:"+level.ToString());
			// add building to array
			Buildings[type].Add (script);

			if((scene.sceneType == SceneType.Battle) && (script.bt.Category != TargetType.Deco)) {

				BuildingDef bd = TBDatabase.GetBuildingDef(type, level);
				BEHealth healthScript = script.goCenter.AddComponent<BEHealth>();
				//healthScript.prefUI = Resources.Load ("Prefabs/UI/UIHealthBar") as GameObject;
				healthScript.uiInfo = script.uiInfo;
				healthScript.maxHealth = bd.HitPoint;
				healthScript.health = bd.HitPoint;
				healthScript.enabled = true;
			}

			return script;
		}

		// remove building
		public void BuildingRemove (Building script) {
			//Debug.Log ("BEGround::BuildingRemove");
			int idx = Buildings[script.Type].FindIndex(x => x==script);
			//Debug.Log ("idx:"+idx.ToString());
			if(idx != -1) {
				Buildings[script.Type].RemoveAt(idx);
			}
			//BuildingListing();
		}

		// get building script
		// if child object was hitted, check parent 
		public Building BuildingFromObject(GameObject go) {
			Building buildingNew = go.GetComponent<Building>();
			if(buildingNew == null)  
				buildingNew = go.transform.parent.gameObject.GetComponent<Building>();
			
			return buildingNew;
		}
		
		// select building
		public void BuildingSelect(Building buildingNew) {
			
			// if user select selected building again
			bool SelectSame = (buildingNew == buildingSelected) ? true : false;
			
			if(buildingSelected != null) {

				buildingSelected.Select (false);

				// if initialy created building, then pass
				if(!buildingSelected.OnceLanded) return;
				// building can't land, then pass 
				if(!buildingSelected.Landed && !buildingSelected.Landable) return;
				
				// land building
				BuildingLandUnselect();
				if(scene.sceneType == SceneType.Town)
					UICommand.Hide();
			}
			
			if(SelectSame) 
				return;
			
			buildingSelected = buildingNew;
			
			if(buildingSelected != null) {
				buildingSelected.Select (true);

				//Debug.Log ("Building Selected:"+buildingNew.gameObject.name+" OnceLanded:"+buildingNew.OnceLanded.ToString ());
				// set scale animation to newly selected building
				BETween bt = BETween.scale(buildingSelected.gameObject, 0.1f, new Vector3(1.0f,1.0f,1.0f), new Vector3(1.4f,1.4f,1.4f));
				bt.loopStyle = BETweenLoop.pingpong;
				// se tbuilding state unland
				//buildingSelected.Land(false, true);
			}
		}

		public void BuildingMultiSelect(Building buildingNew) {
			
			// if user select selected building again
			bool SelectSame = (buildingMultiSelected.Contains(buildingNew)) ? true : false;

			if (SelectSame)
			{
				buildingMultiSelected.Remove(buildingNew);
				buildingNew.Select (false);

				// if initialy created building, then pass
				if(!buildingNew.OnceLanded) return;
				// building can't land, then pass 
				if(!buildingNew.Landed && !buildingNew.Landable) return;
				
				// land building
				BuildingLandUnselectWithArg(buildingNew);
//				if(scene.sceneType == SceneType.Town)
//					UICommand.Hide();				
			}
			else
			{
				buildingMultiSelected.Add(buildingNew);
				buildingNew.Select(true);
				BETween bt = BETween.scale(buildingNew.gameObject, 0.1f, new Vector3(1.0f,1.0f,1.0f), new Vector3(1.4f,1.4f,1.4f));
				bt.loopStyle = BETweenLoop.pingpong;
			}
		}
		
		public void BuildingLandUnselect() {
			if(buildingSelected == null) return;
			buildingSelected.Land(true, true);
			buildingSelected.Select (false);
			buildingSelected = null;

			scene.Save ();
			if(scene.sceneType == SceneType.Town) {
				UICommand.Hide();
			}
		}
		
		public void BuildingLandUnselectWithArg(Building buildingNew) {
			if(buildingNew == null) return;

			buildingNew.Land(true, true, false);
//			if (!buildingNew.Landed)
//			{
//				// 对于不能落地的建筑物处理：
//				// 选项1：底座标红，已在SceneEdit中实现
//				BEUtil.SetObjectColor(buildingNew.goGrid, Color.red);
//				
//
//				// 选项2：将冲突的建筑物随机移动到某个不冲突的位置
//				MoveToVacantTilePos(buildingNew);
//				buildingNew.CheckLandable();
//				buildingNew.Land(true, true);	
//			}

			buildingNew.Select (false);
			buildingNew = null;

			scene.Save ();
			if(scene.sceneType == SceneType.Town) {
				UICommand.Hide();
			}
		}
		
		public void BuildingLand() {
			if(buildingSelected == null) return;
			
			buildingSelected.Land(true, true);

			scene.Save ();
		}

		public void BuildingMultiLand()
		{
			if(!buildingMultiSelected .Any()) return;

			foreach (Building bd in buildingMultiSelected)
			{
				if (bd.Landable)
				{
					bd.Land(true, true);
					scene.Save();
				}
			}
			
		}
		
		public void BuildingDelete(Building building) {
			if(building == null) return;
			
			building.Land (false, false);
			BuildingRemove (building);
			Destroy (building.gameObject);

			if(building == buildingSelected)
				buildingSelected = null;

			scene.Save ();
		}
		
		public void BuildingDeleteAll() {

			Building building = null;
			for(int i=0 ; i < TBDatabase.GetBuildingCount() ; ++i)
			{
				// 根据下标，从TBDatabase中的列表Buildings，得到下标对应Building的ID
				int buildingType = TBDatabase.GetBuildingType(i).ID;
				
				for(int j=0 ; j < Buildings[buildingType].Count ; ++j) {

					building = Buildings[buildingType][j];

					if(building == buildingSelected) {
						buildingSelected = null;
					}
					else {
						building.Land (false, false);
					}

					Destroy (building.gameObject);
				}

				Buildings[buildingType].Clear();
			}

			scene.Save ();
		}
		
		/*public void CapacityCheck() {
			
			// Capacity Max Check Gold & Elixir
			for(int i=0 ; i < 2 ; ++i) {
				PayDef pd = TBDatabase.GetPayDef(i);
				int CapacityTotal = GetCapacityTotal((PayType)i);
				if(pd.Number != null) {
					pd.Number.MaxSet(CapacityTotal);
					if(pd.Number.Target() > CapacityTotal) pd.Number.ChangeTo(CapacityTotal);
				
					DistributeByCapacity((PayType)i, (float)pd.Number.Target());
				}
			}
		}*/

		// get total resource capacity with given resource type
		public int GetCapacityTotal(PayType type) {
			int iReturn = 0;
			for(int i=0 ; i < TBDatabase.GetBuildingCount() ; ++i) 
			{
				// 根据下标，从TBDatabase中的列表Buildings，得到下标对应Building的ID
				int buildingType = TBDatabase.GetBuildingType(i).ID;
				for(int j=0 ; j < Buildings[buildingType].Count ; ++j) {

					// exclude in creation building
					if(Buildings[buildingType][j].Level == 0) continue;

					// exclude production building such as gold mine, because gols mine has it's own capacity 
					if(Buildings[buildingType][j].def.eProductionType == type) continue;

					iReturn += Buildings[buildingType][j].def.Capacity[(int)type];
				}
			}

			return iReturn;
		}

		// when user collect resources from resource generation buildings(like gold mine, elixir extracter)
		// distribute resources to storage buildings by their capacity ratio
		public void DistributeByCapacity(PayType type, float value) {

			// find all storage buildings can store given resourcetype
			int CapacitySum = 0;
			List<Building> capacitylist = new List<Building>();

			for(int i=0 ; i < TBDatabase.GetBuildingCount() ; ++i) 
			{
				// 根据下标，从TBDatabase中的列表Buildings，得到下标对应Building的ID
				int buildingType = TBDatabase.GetBuildingType(i).ID;
				for(int j=0 ; j < Buildings[buildingType].Count ; ++j) {
					
					// exclude in creation building
					if(Buildings[buildingType][j].Level == 0) continue;
					
					// exclude production building such as gold mine, because gols mine has it's own capacity 
					if(Buildings[buildingType][j].def.eProductionType == type) continue;

					capacitylist.Add (Buildings[buildingType][j]);
					CapacitySum += Buildings[buildingType][j].def.Capacity[(int)type];
				}
			}

			//distribute resources
			float ValueStart = value;
			for(int i=0 ; i < capacitylist.Count ; ++i) {

				float SetValue = 0.0f;

				// in normal case, add resource with their capacity ratio
				if(i != capacitylist.Count-1) {
					float fRatio = (float)(capacitylist[i].def.Capacity[(int)type])/(float)CapacitySum;
					SetValue = value * fRatio;
					ValueStart -= SetValue;
				}
				// if it is last storage, give all rest resources
				else {
					SetValue = ValueStart;
				}

				capacitylist[i].Capacity[(int)type] = SetValue;
			}
		}

		// when game started, set workers to working(upgrading)buildings
		public void SetWorkingBuildingWorker() {
			for(int i=0 ; i < TBDatabase.GetBuildingCount() ; ++i) 
			{
				// 根据下标，从TBDatabase中的列表Buildings，得到下标对应Building的ID
				int buildingType = TBDatabase.GetBuildingType(i).ID;
				for(int j=0 ; j < Buildings[buildingType].Count ; ++j) {
					
					// exclude if building is not working
					if(!Buildings[buildingType][j].InUpgrading()) continue;

					// get free worker
					BEWorker Worker = BEWorkerManager.instance.GetAvailableWorker();
					if(Worker != null) {

						AStarTile tile = GetBuidingAStarTile(Buildings[buildingType][j]);
						// set worker position to building
						Worker.SetPosition(tile.x, tile.y);
						// set worker state to work
						Worker.SetWork(Buildings[buildingType][j]);
					}
				}
			}
		}

		// find armycamp with given housingspace
		// 建立兵营，待修改
		public Building FindCampWithSpace(int space) {
			for(int j=0 ; j < Buildings[8].Count ; ++j) {
				if(Buildings[8][j].def == null) continue;

				if(Buildings[8][j].GetUnitHousingSpaceTotal() + space <= Buildings[8][j].def.TroopCapacity)
					return Buildings[8][j];
			}
			
			return null;
		}
	}

}