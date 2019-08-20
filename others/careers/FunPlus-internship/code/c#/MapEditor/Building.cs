using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CocView;
using Wod.ThirdParty.Util;

using UnityEngine.UI;
using UnityEditor;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          Building
///   Description:    class of building
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	// if building can training unit
	// use this class
	public class GenQueItem {
		public 	Building 		building;			// owner building 
		public 	int 			unitID;				// unit id
		public	ArmyType 		at = null;			// definition of unit
		public 	int 			Count;				// allocated count of unit
		public 	float 			timeLeft = 0.0f;	// training time left of current training unit

		public GenQueItem(Building _building, int _unitID, int _Count) {
			building 	= _building;
			unitID 		= _unitID;
			at 			= TBDatabase.GetArmyType(unitID);
			Count		= _Count;
			timeLeft 	= at.TrainingTime;

			building.UnitGenHousingSpaceTotal += Count;
		}

		public float Update(float deltaTime) {

			if(Count <= 0) return deltaTime;

			//Debug.Log ("GenQueItem unitID:"+unitID.ToString ()+" Count:"+Count.ToString ()+" timeLeft:"+timeLeft.ToString ()+" deltaTime:"+deltaTime.ToString () );
			Building buildingArmyCamp = null;
			// if timeleft is lower then zero
			while(deltaTime > timeLeft) {

				buildingArmyCamp = BEGround.instance.FindCampWithSpace(at.HousingSpace);
				if(buildingArmyCamp != null) {
					deltaTime -= timeLeft;
					// unit created, so decrease count
					Count--;
					building.UnitGenHousingSpaceTotal--;
					building.UnitCreated(unitID);
					timeLeft = at.TrainingTime;
					if(Count == 0) {
						//Debug.Log ("Return1 deltaTime:"+deltaTime.ToString () );
						return deltaTime;
					}
				}
				else {
					return 0.0f;
				}
			}

			buildingArmyCamp = BEGround.instance.FindCampWithSpace(at.HousingSpace);
			if(buildingArmyCamp != null) {
				timeLeft -= deltaTime;
			}
			//Debug.Log ("Return1 deltaTime:0.0f");
			return 0.0f;
		}

		public void CountDelta(int delta) {
			Count += delta;
			building.UnitGenHousingSpaceTotal += delta;
		}

		public float GetTimeLeftTotal() {
			return (Count == 0) ? 0 :  (float)(at.TrainingTime * (Count-1)) + timeLeft;
		}
	
	}

	// what property to tween
	public enum BDWorkType {
		None,
		Upgrade,
		Remove,
	}

	public class BDWork {
		public 	BDWorkType 		Type = BDWorkType.None;
		public  bool			Completed = false;
		public	bool			Ing = false;
		public 	float			TimeTotal = 1.0f;
		public	float			TimeLeft = 0.0f;
		public	float			CompleteRatio = 0.0f;

		public BDWork(BDWorkType _Type) {
			Type = _Type;
			Completed = false;
			Ing = false;
		}

		public void Update(float deltaTime) {
			if(!Ing) return;

			// if upgrading proceed
			if(!Completed) {
				// decrease left time
				TimeLeft -= deltaTime;
					
				// if upgrading done
				if(TimeLeft < 0.0f) {
					TimeLeft = 0.0f;
					Completed = true;
					CompleteRatio = 1.0f;
				}
				else {
					CompleteRatio = (TimeTotal-TimeLeft)/TimeTotal;
				}
			}
		}

		public void Start(float fTime) {
			Completed = false;
			Ing = true;
			TimeTotal = TimeLeft = fTime;
			CompleteRatio = 0.0f;
		}
	}

	// building is common class of all building
	// it has capacity relative functions and
	// production relative functions and training relative functions.
	public class Building : MonoBehaviour {

		private	Vector2		tilePosOld = new Vector2(0,0);	// remember old tile pos while drag building
		public  Vector2		tilePos = new Vector2(0,0);		// tile position
		public  Vector2		tileSize = new Vector2(1,1);	// building's size (forexample, 1x1, 2x2, 3x3 tiles)
		[HideInInspector]
		public 	bool		OnceLanded = false;				// if newly created building, oncelanded is false
		[HideInInspector]
		public 	bool		Landed = false;					// current building's land state
		public 	int			Type = 0;						// building type. distinguish building from others with this value
		public 	int			Level = 1;						// level of building

		//ganeobjects of building base prefab
		public 	GameObject	goGrid = null;					// show how many tiles building has occupied	
		public 	GameObject	goArea = null;					// yellow area 
		public 	GameObject	goArrowRoot = null;
		public 	GameObject	goArrowXMinus = null;
		public 	GameObject	goArrowXPlus = null;
		public 	GameObject	goArrowZMinus = null;
		public 	GameObject	goArrowZPlus = null;
		public 	GameObject	goCenter = null;				// building mesh
		public 	GameObject	goXInc = null;					// if building is wall, has more mesh next wall in direction x
		public 	GameObject	goZInc = null;					// if building is wall, has more mesh next wall in direction z
		public 	GameObject	goRangeIn = null;				// for tower, show range
		public 	GameObject	goRangeOut = null;
		public 	GameObject	goRuins = null;
		[HideInInspector]
		public 	BEGround	ground = null;
		[HideInInspector]
		public 	bool		Landable = true;

		public 	GameObject		prefUIInfo;					// prefab for building info ui
		public 	GameObject		prefUICollect;				// prefab for resource collect ui
		public	UIInfo			uiInfo = null;				
		public	BuildingType 	bt = null;					// building type class of this building
		public	BuildingDef		def = null;					// current building's def
		public	BuildingDef 	defNext = null;				// def of next level 
		public	BuildingDef 	defLast = null;				// last def of certain building type

		[HideInInspector]
		public  bool		Collectable = false;			// if building can produce resource, whether collect is enable 
		public	float		Production = 0.0f;				// production value 
		public 	float [] 	Capacity = new float[(int)PayType.Max];	// if building can store resources, this is resource count being stored

		[HideInInspector]
		public  bool		UpgradeCompleted = false;		// is upgrading completed?
		[HideInInspector]
		public	bool		InUpgrade = false;				// in upgrading?
		public 	float		UpgradeTimeTotal = 0.0f;
		public	float		UpgradeTimeLeft = 0.0f;

		[HideInInspector]
		public  bool		RemoveCompleted = false;		// is removing completed?
		[HideInInspector]
		public	bool		InRemove = false;				// in removing?
		public 	float		RemoveTimeTotal = 0.0f;
		public	float		RemoveTimeLeft = 0.0f;

//		[HideInInspector]
//		private List<BDWork>	WorkList = new List<BDWork>();

		[HideInInspector]
		public 	List<List<BEUnit>>	Units = new List<List<BEUnit>>();
		[HideInInspector]
		public 	List<GenQueItem>	queUnitGen = new List<GenQueItem>();
		public 	int 				UnitGenHousingSpaceTotal = 0;

		public	bool		bSelected = false;				// in removing?

		void Awake () {

			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				Units.Add (new List<BEUnit>());
			}
		}
		
		void Update () {

			//Debug.Log ("Building::Update start id:"+bt.ID);
			// use BETime to revise times
			float deltaTime = BETime.deltaTime;
			//Debug.Log ("Building::Update deltaTime:"+deltaTime);

			// if building is in selected state, keep color animation
			if(bSelected && (goCenter != null)) {
				float fColor = Mathf.PingPong(Time.time * 1.5f, 1) * 0.5f + 0.5f;
				Color clrTemp = new Color(fColor,fColor,fColor,1);
				BEUtil.SetObjectColor(goCenter, clrTemp);
				BEUtil.SetObjectColor(goXInc, clrTemp);
				BEUtil.SetObjectColor(goZInc, clrTemp);
			}

			if(BEGround.scene.sceneType != SceneType.Town)
				return;

			// if building can produce resources
			if(Landed && !InUpgrade && (def != null) && (def.eProductionType != PayType.None)) {
				Production += def.GetProductionRate() * deltaTime;

				// check maximum capacity
				if((int)Production >= def.Capacity[(int)def.eProductionType])
					Production = def.Capacity[(int)def.eProductionType];

				// is minimum resources generated, then ser collectable flagand show dialog
				if(((int)Production >= 10) && !uiInfo.groupCollect.gameObject.activeInHierarchy) {
					Collectable = true;
					uiInfo.CollectIcon.sprite = TBDatabase.GetPayDefIcon((int)def.eProductionType);
					BETween.alpha(uiInfo.groupCollect.gameObject, 0.2f, 0.0f, 1.0f);
					uiInfo.groupCollect.gameObject.SetActive(true);
					uiInfo.groupInfo.gameObject.SetActive(false);
				}

				if(Collectable) {
					// when production count is reach to max count, then change dialog color to red
					uiInfo.CollectDialog.color = (Production == def.Capacity[(int)def.eProductionType]) ? Color.red : Color.white;
				}
			}

			// if in upgrade
			if(InUpgrade) {

				// if upgrading proceed
				if(!UpgradeCompleted) {
					// decrease left time
					UpgradeTimeLeft -= deltaTime;

					// if upgrading done
					if(UpgradeTimeLeft < 0.0f) {
						UpgradeTimeLeft = 0.0f;
						UpgradeCompleted = true;

						// if building is selected, then update command dialog
						if(UICommand.Visible && (BEGround.buildingSelected == this) && (BEGround.scene.sceneType == SceneType.Town))
							UICommand.Show(this);
					}
				}

				// update ui info
				uiInfo.TimeLeft.text = UpgradeCompleted ? "Completed!" : BENumber.SecToString(Mathf.CeilToInt(UpgradeTimeLeft));
				uiInfo.Progress.fillAmount = (UpgradeTimeTotal-UpgradeTimeLeft)/UpgradeTimeTotal;
				uiInfo.groupProgress.alpha = 1;
				uiInfo.groupProgress.gameObject.SetActive(true);
			}

			// if in remove
			if(InRemove) {
				
				// if removing proceed
				if(!RemoveCompleted) {
					// decrease left time
					RemoveTimeLeft -= deltaTime;
					
					// if upgrading done
					if(RemoveTimeLeft < 0.0f) {
						RemoveTimeLeft = 0.0f;
						RemoveCompleted = true;
						RemoveEnd();
					}
				}
				
				// update ui info
				uiInfo.TimeLeft.text = RemoveCompleted ? "Completed!" : BENumber.SecToString(Mathf.CeilToInt(RemoveTimeLeft));
				uiInfo.Progress.fillAmount = (RemoveTimeTotal-RemoveTimeLeft)/RemoveTimeTotal;
				uiInfo.groupProgress.alpha = 1;
				uiInfo.groupProgress.gameObject.SetActive(true);
			}

			//Debug.Log ("Building::Update end-1 id:"+bt.ID);
			UnitGenUpdate(deltaTime);
			//Debug.Log ("Building::Update end id:"+bt.ID);
		}

		// get gem count to finish current upgrading
		public int GetUpgradeFinishGemCount() {
			if(!InUpgrade || UpgradeCompleted) return 0;

			int GemCount = ((int)UpgradeTimeLeft+1)/60;
			if(GemCount == 0) GemCount = 1;
			return GemCount;
		}

		public bool InUpgrading() {
			return (InUpgrade && !UpgradeCompleted) ? true : false;
		}

		// get gem count to remove current upgrading
		public int GetRemoveFinishGemCount() {
			if(!InRemove || RemoveCompleted) return 0;
			
			int GemCount = ((int)RemoveTimeLeft+1)/60;
			if(GemCount == 0) GemCount = 1;
			return GemCount;
		}
		
		// initialize building
		public void Init(int type, int level) {
			Type = type;
			Level = level;

			if (Level >3)
			{
				Level = 3;
			}
			// delete old meshes
			if(goCenter != null) 	{ Destroy (goCenter); goCenter = null; }
			if(goXInc != null)	 	{ Destroy (goXInc); goXInc = null; }
			if(goZInc != null) 		{ Destroy (goZInc); goZInc = null; }
			


			bt = TBDatabase.GetBuildingTypeByID(type);
			LoggerHelper.Debug("Build: init type:"+type.ToString());
			// get mesh path type and level
			int displayLevel = (level == 0) ? 1 : level;
			//string meshPath = "Prefabs/Building/"+bt.Name+"_"+displayLevel.ToString ();
			// string meshPath = "Prefabs/Building/" + bt.Name;
			string meshPath = "Assets/_Res/Building/" + bt.Name;
			// Debug.Log ("Loading Mesh "+meshPath);
			GameObject prefabMesh = new GameObject(bt.displayName);
			Debug.Log(prefabMesh);


			if (bt.Name.Contains("[") && bt.Name.Contains("]"))
			{
				var split = bt.Name.IndexOf('[');
				var mainStr = bt.Name.Substring(0, split);
				var str = bt.Name.Substring(split+1, bt.Name.Length - split-2);
				var strs = str.Split(',');

				
				foreach (var s in strs)
				{
					GameObject go = new GameObject("Holder");
					Debug.Log(mainStr+s+".png");
                	var sprite = go.AddComponent<SpriteRenderer>();
#if UNITY_EDITOR   
                	sprite.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(mainStr+s+".png");
#endif

                    go.transform.SetParent(prefabMesh.transform);
				}

				//prefabMesh = go;

			}
			else
			{
				//			prefabMesh.AddComponent<CanvasRenderer>();
				//			var rectTransform = prefabMesh.AddComponent<RectTransform>();
				//			rectTransform.localScale = Vector3.one;
				//			rectTransform.sizeDelta = new Vector2(1,1);
				//			var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Res/Building/" + bt.Name + ".png");
				//
				//			prefabMesh.AddComponent<Image>().sprite = sprite;
				//			def.prefab = go;
                
                
				var sprite = prefabMesh.AddComponent<SpriteRenderer>();
#if UNITY_EDITOR  
				sprite.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(meshPath + ".png");
#endif
			}

			

			

			// instantiate mesh and set to goCenter
//			GameObject prefabMesh = Resources.Load (meshPath) as GameObject;
			if (prefabMesh== null)
			{
				Debug.Log("Reading Assets error!");
				displayLevel = 3;//town 暂时3级
				//meshPath = "Prefabs/Building/"+bt.Name+"_"+displayLevel.ToString ();
				//meshPath = "Prefabs/Building/"+bt.Name+"_1";
				meshPath = "Prefabs/Building/"+bt.Name;
				prefabMesh = Resources.Load(meshPath) as GameObject;
			}

			goCenter = prefabMesh;//(GameObject)Instantiate(prefabMesh, Vector3.zero, Quaternion.identity);
			goCenter.transform.SetParent (gameObject.transform);
			goCenter.transform.localRotation = Quaternion.Euler(45,45,0);

			goCenter.transform.localPosition = bt.Pos;
			goCenter.transform.localScale = bt.Scale;//暂时不用
			
			if(bt.Main_type == 99)//出兵点
				goCenter.transform.localRotation = Quaternion.Euler(90,45,0);
			// 城墙的Main_type为1)
			if(bt.Main_type == 1) {

				// create x,z side mesh
				string meshSidePath = meshPath+"1Side";
				GameObject prefabMeshSide = new GameObject("Wall_Side");
				var sprite_side = prefabMeshSide.AddComponent<SpriteRenderer>();
#if UNITY_EDITOR 
				sprite_side.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(meshSidePath +".png");
#endif
				
				// GameObject prefabMeshSide = Resources.Load (meshSidePath+".png") as GameObject;

				LoggerHelper.Debug("Build:"+ meshSidePath);
				//goXInc = (GameObject)Instantiate(prefabMeshSide, Vector3.zero, Quaternion.identity);
				goXInc = prefabMeshSide;
				goXInc.transform.SetParent (gameObject.transform);
//				goXInc.transform.localPosition = new Vector3(-0.2f,0.3f,0.3f);
//				goXInc.transform.localRotation = Quaternion.Euler(45,45,-120);
				goXInc.transform.localPosition = Vector3.zero;
				goXInc.transform.localRotation = Quaternion.Euler(0,180,0); // rotate to x direction

				//goZInc = (GameObject)Instantiate(prefabMeshSide, Vector3.zero, Quaternion.identity);
				goZInc = prefabMeshSide;
				goZInc.transform.SetParent (gameObject.transform);
				goZInc.transform.localPosition = Vector3.zero;
				goZInc.transform.localRotation = Quaternion.Euler(0,90,0); // rotate to z direction
//				goXInc.transform.localPosition = new Vector3(-0.2f,0.3f,0.3f);
//				goXInc.transform.localRotation = Quaternion.Euler(45,45,-120);
			}

			// set tile size
			tileSize = new Vector2(bt.TileX, bt.TileZ);
			// set proper material to gogrid
			goGrid.GetComponent<Renderer>().material = Resources.Load ("Materials/Tile"+bt.TileX.ToString ()+"x"+bt.TileZ.ToString ()) as Material;
			goGrid.transform.localScale = new Vector3(bt.TileX, 1, bt.TileZ);
			goArrowXMinus.transform.localPosition = new Vector3(-(0.4f+(float)bt.TileX*0.5f), 0.01f, 0);
			goArrowXPlus.transform.localPosition  = new Vector3( (0.4f+(float)bt.TileX*0.5f), 0.01f, 0);
			goArrowZMinus.transform.localPosition = new Vector3(0, 0.01f, -(0.4f+(float)bt.TileX*0.5f));
			goArrowZPlus.transform.localPosition  = new Vector3(0, 0.01f,  (0.4f+(float)bt.TileX*0.5f));

			goArea.transform.localScale = new Vector3(bt.TileX*0.95f, 1, bt.TileZ*0.95f);
			if(bt.Category == TargetType.Deco) {
				goArea.SetActive(false);
				//goGrid.SetActive(false);
				goArrowRoot.SetActive(false);
			}

			// get ui
			if(transform.Find ("UIBuilding")) {
				uiInfo = transform.Find ("UIBuilding").transform.Find ("UIInfo").GetComponent<UIInfo>();
			}
			else {
				uiInfo = UIInGame.instance.AddInGameUI(prefUIInfo, transform, new Vector3(0,1.5f,0)).GetComponent<UIInfo>();
			}
			uiInfo.groupInfo.gameObject.SetActive(false);
			uiInfo.groupProgress.gameObject.SetActive(false);
			uiInfo.groupCollect.gameObject.SetActive(false);
			uiInfo.Name.text = TBDatabase.GetBuildingDisplayNameByID(Type);
			uiInfo.Level.text = (bt.Category == TargetType.Deco) ? "" : "Level "+displayLevel.ToString ();
			uiInfo.building = this;

			// currently not used
			goRangeIn.SetActive(false);
			goRangeOut.SetActive(false);
			goRuins.SetActive(false);
			goRuins.transform.localScale = new Vector3(bt.TileX*0.9f, 1, bt.TileZ*0.9f);
			goRuins.transform.localRotation = Quaternion.Euler(0,UnityEngine.Random.Range(0,360),0);

			// initialize values 
			tilePosOld = tilePos;
			CheckLandable();
			CheckNeighbor();
//			def = TBDatabase.GetBuildingDef(Type, Level);
			def = null;
			defNext = bt.GetDefine(Level+1);
			//defLast = bt.GetDefLast();
			UpgradeTimeTotal = (defNext != null) ? defNext.BuildTime : 0;

			RemoveTimeTotal = (def != null) ? def.BuildTime : 0;

			Select(false);
		}

		public void UpjustYByState() {
			Vector3 vPos = transform.localPosition;
			vPos.y = Landed ? 0.0f : 0.1f;
			transform.localPosition = vPos;
		}

		public void Move(Vector3 vTarget)
		{
			if(Landed) {
				Land (false, false);
			}

			tilePos = ground.GetTilePos(vTarget, tileSize);
			ground.Move(gameObject, tilePos, tileSize);
			//Debug.Log ("Move Pos: "+Landed.ToString ()+" "+TilePosX.ToString()+","+TilePosY.ToString());
			CheckLandable();
			UpjustYByState();
		}

		public void Move(int TileX, int TileZ) {
			tilePos = new Vector2(TileX, TileZ);
			ground.Move(gameObject, tilePos, tileSize);
			//Debug.Log ("Move Pos: "+Landed.ToString ()+" "+TilePosX.ToString()+","+TilePosY.ToString());
			CheckLandable();
			UpjustYByState();
		}

		// check tiles of building is vacant and set color of grid object
		public void CheckLandable() {
			bool IsVacant = ground.IsVacant(tilePos, tileSize);
			//Debug.Log ("CheckLandable Vacant"+IsVacant.ToString ());
			Color clrTemp = IsVacant ? new Color(0,1,0,0.5f) : new Color(1,0,0,0.5f);
			// BEUtil.SetObjectColor(goGrid, "_TintColor", clrTemp);
			Landable = IsVacant;
		}

		public void Select(bool _bSelected) {
			bSelected = _bSelected;

			if((bt.Category != TargetType.Deco) && (goArrowRoot != null))
				goArrowRoot.SetActive(bSelected);

			if(goRangeOut != null) {
				BETurret scriptTurret = goCenter.GetComponent<BETurret>();
				if(scriptTurret != null) {
				
					goRangeOut.transform.localScale = new Vector3(scriptTurret.AttackRange,1,scriptTurret.AttackRange);
					goRangeOut.SetActive(bSelected);
				}
			}

			uiInfo.groupInfo.alpha = bSelected ? 1 : 0;
			uiInfo.groupInfo.gameObject.SetActive(bSelected);

			if(!bSelected) {
				BEUtil.SetObjectColor(goCenter, Color.white);
				BEUtil.SetObjectColor(goXInc, Color.white);
				BEUtil.SetObjectColor(goZInc, Color.white);
			}
		}

		public void Land(bool landed, bool animate, bool useTilePosOld=true) {
			//Debug.Log (gameObject.name+" Building::Land landed-"+landed+" animate-"+animate);
			if(Landed == landed) return;

			if(landed && !Landable ) {
				if (useTilePosOld)
				{
					if(((int)tilePosOld.x == -1) && ((int)tilePosOld.y == -1))
                    	return;
    
                    tilePos = tilePosOld;
                    //Debug.Log ("Land RecoverOldPos: "+TilePosOldX.ToString()+","+TilePosOldY.ToString());
                    ground.Move(gameObject, tilePos, tileSize);
                    CheckLandable();
    
                    if(!Landable)
                    	return;
				}
				else
				{
					return;
				}

			}

			Landed = landed;
			ground.OccupySet(this, true);

			if(!Landed) {
				tilePosOld = tilePos;
				//Debug.Log ("Land Save OldPos: "+TilePosOldX.ToString()+","+TilePosOldY.ToString());
			}
			else {
				if(!OnceLanded)
					OnceLanded = true;
			}

			CheckLandable();
			goGrid.SetActive(Landed ? false : true);

			// if building is wall, check neighbor
			if(bt.Main_type == 1)
				CheckNeighbor();

			if(Landed && (goCenter != null)) {
				BEGround.scene.Save();
			}

			UpjustYByState();
			if(!BEGround.scene.InLoading && (BEWorkerManager.instance != null))
				BEWorkerManager.instance.OnTileInfoChanged();

			if(Landed && (bt.ID == 8)) {
				int unitOffset = 0;
				for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
					int Count = Units[i].Count;	
					for(int j=0 ; j < Count ; ++j) {
						Units[i][j].SetState (UnitState.KeepPosition, gameObject, GetUnitOffset(unitOffset));
						unitOffset++;
					}
				}
			}
		}

		public void CheckNeighbor() {
			if((goXInc == null) || (goZInc == null)) return;

			// in x and y coordination, next or prev building is samely wall, then show side mesh
			Building bdNeighbor = null;
			bdNeighbor = ground.GetBuilding((int)tilePos.x-1, (int)tilePos.y);		if((bdNeighbor != null) && (bdNeighbor.Type == 2))  bdNeighbor.CheckNeighbor();
			bdNeighbor = ground.GetBuilding((int)tilePos.x+1, (int)tilePos.y);		goXInc.SetActive((Landed && (bdNeighbor != null) && (bdNeighbor.Type == 2)) ? true : false);
			bdNeighbor = ground.GetBuilding((int)tilePos.x,   (int)tilePos.y-1);	if((bdNeighbor != null) && (bdNeighbor.Type == 2)) bdNeighbor.CheckNeighbor();
			bdNeighbor = ground.GetBuilding((int)tilePos.x,   (int)tilePos.y+1);	goZInc.SetActive(((Landed && bdNeighbor != null) && (bdNeighbor.Type == 2)) ? true : false);
		}

		// save building info to xml format
		public void Save(XmlDocument d) {
			XmlElement ne = d.CreateElement("Building"); 					
			ne.SetAttribute("ID", Type.ToString ());														
			ne.SetAttribute("X", tilePos.x.ToString ());							
			ne.SetAttribute("Y", tilePos.y.ToString ());	
			d.DocumentElement.AppendChild (ne);

			
			// 待修改
			// if barrack
			if(Type == 7) {
				for(int i=0 ; i < queUnitGen.Count ; ++i) {
					GenQueItem item = queUnitGen[i];
					XmlElement neUnit = d.CreateElement("GenQue"); 					
					neUnit.SetAttribute("unitID", item.unitID.ToString ());							
					neUnit.SetAttribute("Count", item.Count.ToString ());	
					neUnit.SetAttribute("timeLeft", item.timeLeft.ToString ());	
					ne.AppendChild (neUnit);
				}
			}
			// if army camp
			else if(Type == 8) {

				string strUnits = "";
				for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
					strUnits += Units[i].Count.ToString ();

					if(i != TBDatabase.GetArmyTypeCount()-1)
						strUnits += ",";
				}

				XmlElement neUnit = d.CreateElement("Unit"); 					
				neUnit.SetAttribute("value", strUnits);							
				ne.AppendChild (neUnit);
			}
			else {}
		}

		public void LoadJson(MapItem building)
		{
			Type = building.ID;
			tilePos.x = building.X;
			tilePos.y = building.Y;
			
			ground.Move(gameObject, tilePos, tileSize);
			CheckLandable();
			Land(true, false);
			
			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				Units[i].Clear();
			}

			queUnitGen.Clear();
		}
		
		// load building info to xml format
		public void Load(XmlElement e) {
			Type 				= int.Parse(e.GetAttribute("ID"));
			tilePos.x 			= float.Parse(e.GetAttribute("X"));
			tilePos.y 			= float.Parse(e.GetAttribute("Y"));

			ground.Move(gameObject, tilePos, tileSize);
			CheckLandable();
			Land(true, false);

			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				Units[i].Clear();
			}

			queUnitGen.Clear();

			XmlNodeList list = e.ChildNodes;
			foreach(XmlElement ele in list) {
				if(ele.Name == "Unit") {	
					if(BEGround.scene.sceneType != SceneType.Battle) {
						string strUnits = ele.GetAttribute("value");
						string [] strUnitsSub = strUnits.Split(',');
						int unitOffset = 0;
						for(int i=0 ; i < strUnitsSub.Length ; ++i) {
							int Count = int.Parse(strUnitsSub[i]);	
							for(int j=0 ; j < Count ; ++j) {
								BEUnit script = BEUnitManager.instance.Add (0, i, 1, transform.position, Quaternion.identity);
								script.SetState (UnitState.KeepPosition, gameObject, GetUnitOffset(unitOffset), true);
								Units[i].Add(script);
								unitOffset++;
								script.transform.localRotation = Quaternion.identity;
							}
						}
					}
				}
				else if(ele.Name == "GenQue") {	
					int unitID = int.Parse(ele.GetAttribute("unitID"));	 		
					int Count = int.Parse(ele.GetAttribute("Count"));	
					float timeLeft = float.Parse(ele.GetAttribute("timeLeft"));	
					
					GenQueItem item = UnitGenAdd(unitID, Count);
					item.timeLeft = timeLeft;
				}
				else {}
			}
		}

		public int GetUnitHousingSpaceTotal() {
			int Count = 0;
			for(int i=0 ; i < Units.Count ; ++i) {
				for(int j=0 ; j < Units[i].Count ; ++j) {
					Count += Units[i][j].at.HousingSpace;
				}
			}

			return Count;
		}
		
		public Vector3 GetUnitOffset(int idx) {

			float margin = 1.0f;
			int columnMax = (int)((float)bt.TileX/margin);
			int rowMax = (int)((float)bt.TileZ/margin);
			Vector3 vStart = new Vector3(-(float)(columnMax-1)*0.5f, 0, -(float)(rowMax-1)*0.5f);

			int row = idx/columnMax;
			int column = idx - row*columnMax;
			Vector3 vReturn = vStart + new Vector3(margin*(float)column,0,margin*(float)row);
			//Debug.Log ("GetUnitOffset "+vReturn.ToString ());
			return vReturn;
		}
		
		// when user clicked this building
		// if building has any kind of completed job
		// do complete and return true
		public bool HasCompletedWork() {

			if(UpgradeCompleted) 	{ UpgradeEnd (); return true; }
			if(Collectable) 		{ Collect (); return true; }
		
			return false;
		}

		// collect resources
		public void Collect() {

			string textColor=""; 

			// increase resource count
			PayDef pd = TBDatabase.GetPayDef((int)def.eProductionType);
			if(pd != null) {
				pd.Number.ChangeDelta((double)Production);
				textColor = pd.textColor;
			}

			// show collect ui to show how many resources was collected
			UICollect script = UIInGame.instance.AddInGameUI(prefUICollect, transform, new Vector3(0,1.5f,0)).GetComponent<UICollect>();
			script.Name.text = textColor+((int)Production).ToString ("#,##0")+"</color>";
			script.Init(transform, new Vector3(0,1.0f,0));

			// reset values related to production
			Collectable = false;
			Production = 0;
			// hide collect dialog
			//Debug.Log ("22");
			BETween.alpha(uiInfo.groupCollect.gameObject, 0.3f, 1.0f, 0.0f);
			BETween.enable(uiInfo.groupCollect.gameObject, 0.3f, true, false);
			// save game - save game when action is occured. not program quit moment
			BEGround.scene.Save();
		}

		// whether upgrading is enable
		public bool IsUpgradeEnable() {

			BuildingType bt = TBDatabase.GetBuildingType(Type);
			// if this level is max level
			if(bt.Defs.Count <= Level) return false;
			// if no next level
			if(defNext == null) return false;

			// if user don't have enough resources to upgrade
			for(int i=0 ; i < (int)PayType.Max ; ++i) {

				PayDef pd = TBDatabase.GetPayDef (i);
				if((defNext.BuildPrice[i] != 0) && (pd.Number.Target() < defNext.BuildPrice[i]))
					return false;
			}

			return true;
		}

		// start upgrade
		public bool Upgrade() {

			if(InUpgrade) return false;
			if(bt.Defs.Count <= Level) return false;
			if(defNext == null) return false;

			// in case initial build (level0->level1), the upgrading price already payed.
			if(Level != 0) {
				// check whether user has enough resources or not
				// decrease user's resource
				PayType payTypeReturn = PayforBuild(defNext);
				if(payTypeReturn != PayType.None) {
					UIDialogMessage.Show("Insufficient "+payTypeReturn.ToString (), "Ok", "Error");

					return false;
				}
			}

			// prepare upgrade
			UpgradeCompleted = false;
			InUpgrade = true;

			// if upgrade time is zero(like wall) then upgrade immediately
			if(UpgradeTimeTotal > 0) {
				UpgradeTimeLeft = UpgradeTimeTotal;
				uiInfo.TimeLeft.text = BENumber.SecToString(Mathf.CeilToInt(UpgradeTimeLeft));
				uiInfo.Progress.fillAmount = (UpgradeTimeTotal-UpgradeTimeLeft)/UpgradeTimeTotal;
				BETween.alpha(uiInfo.groupProgress.gameObject, 0.3f, 0.0f, 1.0f);
				uiInfo.groupProgress.gameObject.SetActive(true);
			}
			else {
				UpgradeEnd();
			}

			return true;
		}

		// cancel current upgrade
		public void UpgradeCancel() {
			InUpgrade = false;
			UpgradeCompleted = false;

			uiInfo.groupProgress.gameObject.SetActive(false);

			RefundBuild(defNext);
			BEGround.scene.Save();
		}

		// when upgraded ended, user clicked this building
		public void UpgradeEnd() {

			// initialize with next level
			Init(Type, Level+1);
			InUpgrade = false;
			UpgradeCompleted = false;

			// increase experience
			BEGround.scene.GainExp(def.RewardExp);
			// if building has capacity value, then recalc capacity of all resources

//			if(def.HasCapacity())
//				BEGround.instance.CapacityCheck();

			// save game - save game when action is occured. not program quit moment
			BEGround.scene.Save();
		}

		public bool IsWorking() {
		
			if(InUpgrade && !UpgradeCompleted) 		return true;
			else if(InRemove && !RemoveCompleted)	return true;
			else return false;
		}

		// start upgrade
		public bool Remove() {
			
			if(InRemove) return false;

			// in case initial build (level0->level1), the upgrading price already payed.
			if(Level != 0) {
				// check whether user has enough resources or not
				// decrease user's resource
				PayType payTypeReturn = PayforBuild(def);
				if(payTypeReturn != PayType.None) {
					UIDialogMessage.Show("Insufficient "+payTypeReturn.ToString (), "Ok", "Error");
					
					return false;
				}
			}
			
			// prepare upgrade
			RemoveCompleted = false;
			InRemove = true;
			
			// if remove time is zero(like wall) then upgrade immediately
			if(RemoveTimeTotal > 0) {
				RemoveTimeLeft = RemoveTimeTotal;
				uiInfo.TimeLeft.text = BENumber.SecToString(Mathf.CeilToInt(RemoveTimeLeft));
				uiInfo.Progress.fillAmount = (RemoveTimeTotal-RemoveTimeLeft)/RemoveTimeTotal;
				BETween.alpha(uiInfo.groupProgress.gameObject, 0.3f, 0.0f, 1.0f);
				uiInfo.groupProgress.gameObject.SetActive(true);
			}
			else {
				RemoveEnd();
			}

			return true;
		}
		
		// cancel current upgrade
		public void RemoveCancel() {
			InRemove = false;
			RemoveCompleted = false;
			
			uiInfo.groupProgress.gameObject.SetActive(false);

			RefundBuild(def);
			BEGround.scene.Save();
		}
		
		public void RemoveEnd() {
			// initialize with next level
			InRemove = false;
			RemoveCompleted = false;

			if(BEGround.buildingSelected == this) {
				BEGround.buildingSelected = null;

				// do not call BuildingSelect. because gameObject will be destroyed soon
				//BEGround.instance.BuildingSelect(null);
			}

			//Debug.Log ("0000");
			Land(false, false);
			BEGround.instance.BuildingRemove (this);

			gameObject.transform.SetParent (null);
			Destroy (gameObject);
			// save game - save game when action is occured. not program quit moment
			BEGround.scene.Save();
		}

		// check user has enough resources to upgrade.
		// incase yes, decrease resources and return Paytype.None
		// othewise return the type of resource needed.
		public PayType PayforBuild(BuildingDef _bd) {

			// get pay type
			PayType payTypeReturn = PayType.None;
			for(int i=0 ; i < (int)PayType.Max ; ++i) {
				if(payTypeReturn != PayType.None) continue;

				PayDef pd = TBDatabase.GetPayDef(i);
				if((_bd.BuildPrice[i] != 0) && (pd.Number.Target () < _bd.BuildPrice[i])) 	
					payTypeReturn = (PayType)i;
			}

			// if building is not free to build
			if(payTypeReturn == PayType.None) {

				//decrease resources
				for(int i=0 ; i < (int)PayType.Max ; ++i) {
					PayDef pd = TBDatabase.GetPayDef(i);
					if(_bd.BuildPrice[i] != 0) 
						pd.Number.ChangeDelta (-_bd.BuildPrice[i]);	
				}

//				BEGround.instance.CapacityCheck();
			}
			
			return payTypeReturn;
		}

		// when user cancel upgrade, refund half of resources
		public void RefundBuild(BuildingDef _bd) {
			if(_bd == null) return ;
			
			// when user cancel upgrade, refund half of upgrade price 
			for(int i=0 ; i < (int)PayType.Max ; ++i) {
				PayDef pd = TBDatabase.GetPayDef(i);
				if(_bd.BuildPrice[i] != 0) 
					pd.Number.ChangeDelta (_bd.BuildPrice[i]/2);	
			}
		}

		// if building can training unit
		// add unit to training aue
		public GenQueItem UnitGenAdd(int unitID, int Count) {

			if(UnitGenHousingSpaceTotal >= def.TrainingQueueMax)
				return null;

			//Debug.Log ("Building::UnitGenAdd "+unitID.ToString()+"x"+Count.ToString());
			// search unit que list with given unit id
			int idx = queUnitGen.FindIndex(x => x.unitID==unitID);
			if(idx == -1) {
				GenQueItem item = new GenQueItem(this, unitID, Count);
				queUnitGen.Add (item);
				return item;
			}
			else {
				GenQueItem item = queUnitGen[idx];
				item.CountDelta(Count);
				return item;
			}
		}

		// if user cancel training unit, decrease count of training unit
		public bool UnitGenRemove(int unitID, int Count) {
			//Debug.Log ("Building::UnitGenAdd "+unitID.ToString()+"x"+Count.ToString());
			// search unit que list with given unit id
			int idx = queUnitGen.FindIndex(x => x.unitID==unitID);
			if(idx != -1) {
				GenQueItem item = queUnitGen[idx];
				item.CountDelta(-Count);
				return false;
			}

			//Debug.Log ("Building::UnitGenRemove decrease unitID:"+unitID.ToString()+" Not Found");
			return false;
		}

		// 
		public void UnitGenUpdate(float deltaTime) {

			for(int i=0 ; i < queUnitGen.Count ; ++i) {
				GenQueItem item = queUnitGen[i];
				if(item.Count == 0) {
					UIDialogTraining.instance.UnitGenRemoveByBuilding(this, item.unitID);
					queUnitGen.RemoveAt(i);
					break;
				}
			}

			while(queUnitGen.Count > 0) {
				GenQueItem item = queUnitGen[0];
				deltaTime = item.Update (deltaTime);
				if(item.Count == 0) {
					//Debug.Log ("Building::UnitGenUpdate delete "+item.unitID.ToString());
					queUnitGen.RemoveAt(0);
					UIDialogTraining.instance.UnitGenRemoveByBuilding(this, item.unitID);
				}

				if(deltaTime < 0.01f) {
					return;
				}
			}
		}

		public float UnitGenTimeLeftTotal() {
			float timeTotal = 0.0f;
			for(int i=0 ; i < queUnitGen.Count ; ++i) {
				GenQueItem item = queUnitGen[i];
				timeTotal += item.GetTimeLeftTotal();
			}

			return timeTotal;
		}

		// a unit was created
		public void UnitCreated(int unitID) {
			// find army camp with space
			// create unit
			// set unit's base camp position
			//Debug.Log ("Building::UnitCreated "+unitID.ToString()+" UnitCount:"+GenUnitCount[unitID].ToString ());
			ArmyType at = TBDatabase.GetArmyType(unitID);

			Building buildingArmyCamp = BEGround.instance.FindCampWithSpace(at.HousingSpace);
			if(buildingArmyCamp != null) {
				BEUnit script = BEUnitManager.instance.Add (0, unitID, 1, transform.position, Quaternion.identity);
				buildingArmyCamp.UnitAdd (script);
			}
		}

		public void UnitAdd(BEUnit script) {
			int unitOffset = 0;
			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				int Count = Units[i].Count;	
				for(int j=0 ; j < Count ; ++j) {
					unitOffset++;
				}
			}

			bool bImmediately = (BETime.deltaTime > 10.0f) ? true : false;
			script.SetState (UnitState.KeepPosition, gameObject, GetUnitOffset(unitOffset), bImmediately);
			Units[script.at.ID].Add (script);
		}

		// fill building info in building info dialog
		public void UIFillProgress(ProgressInfo progress, BDInfo type) {

			if(type == BDInfo.CapacityGold) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)PayType.Gold);
				progress.textInfo.text = "Capacity : "+Capacity[(int)PayType.Gold].ToString("#,##0")+"/"+def.Capacity[(int)PayType.Gold].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Capacity[(int)PayType.Gold]/(float)(def.Capacity[(int)PayType.Gold]);
			}
			else if(type == BDInfo.CapacityElixir) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)PayType.Elixir);
				progress.textInfo.text = "Capacity : "+Capacity[(int)PayType.Elixir].ToString("#,##0")+"/"+def.Capacity[(int)PayType.Elixir].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Capacity[(int)PayType.Elixir]/(float)(def.Capacity[(int)PayType.Elixir]);
			}
			else if(type == BDInfo.Capacity) {
				Debug.Log ("def.eProductionType "+def.eProductionType);
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)def.eProductionType);
				progress.textInfo.text = "Capacity : "+Production.ToString("#,##0")+"/"+def.Capacity[(int)def.eProductionType].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Production/(float)(def.Capacity[(int)def.eProductionType]);
			}
			else if(type == BDInfo.ProductionRate) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)def.eProductionType);
				progress.textInfo.text = "ProductionRate : "+def.GetProductionPerHour().ToString("#,##0")+" per Hour";
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = def.GetProductionRate()/defLast.GetProductionRate();
			}
			else if(type == BDInfo.HitPoint) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/Heart");
				progress.textInfo.text = "HitPoints : "+def.HitPoint.ToString("#,##0")+"/"+def.HitPoint.ToString("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = (float)def.HitPoint/(float)def.HitPoint;
			}
			else if(type == BDInfo.StorageCapacity) {
				PayType payType = PayType.None;
				for(int i=0 ; i < (int)PayType.Max ; ++i) {
					if(def.Capacity[i] != 0) {
						payType = (PayType)i;
						break;
					}
				}
				if(payType == PayType.None) return;

				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)payType);
				progress.textInfo.text = "Storage Capacity : "+Capacity[(int)payType].ToString("#,##0")+"/"+def.Capacity[(int)payType].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Capacity[(int)payType]/(float)def.Capacity[(int)payType];
			}
			else if(type == BDInfo.TroopCapacity) {
				int UsedCapacity = GetUnitHousingSpaceTotal();

				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/People");
				progress.textInfo.text = "Total Troop Capacity : "+UsedCapacity.ToString("#,##0")+"/"+def.TroopCapacity.ToString("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = (float)UsedCapacity/(float)def.TroopCapacity;
			}
			else if(type == BDInfo.TrainingCapacity) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/People");
				progress.textInfo.text = "Training Capacity : "+UnitGenHousingSpaceTotal.ToString("#,##0")+"/"+def.TrainingQueueMax.ToString("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = (float)UnitGenHousingSpaceTotal/(float)def.TrainingQueueMax;
			}
			else if(type == BDInfo.DamagePerSecond) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/Training");
				progress.textInfo.text = "Damage Per Second : "+def.DamagePerSecond.ToString("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = def.DamagePerSecond/defLast.DamagePerSecond;
			}
			else {}
		}

		// fill building upgrading info in building upgrade ask dialog
		public void UIFillProgressWithNext(ProgressInfo progress, BDInfo type) {
			
			if(type == BDInfo.CapacityGold) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)PayType.Gold);
				progress.textInfo.text = "Capacity : "+def.Capacity[(int)PayType.Gold].ToString ("#,##0")+"+"+(defNext.Capacity[(int)PayType.Gold]-def.Capacity[(int)PayType.Gold]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)PayType.Gold]/(float)defLast.Capacity[(int)PayType.Gold];
				progress.imageFront.fillAmount = (float)def.Capacity[(int)PayType.Gold]/(float)defLast.Capacity[(int)PayType.Gold];
			}
			else if(type == BDInfo.CapacityElixir) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)PayType.Elixir);
				progress.textInfo.text = "Capacity : "+def.Capacity[(int)PayType.Elixir].ToString ("#,##0")+"+"+(defNext.Capacity[(int)PayType.Elixir]-def.Capacity[(int)PayType.Elixir]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)PayType.Elixir]/(float)defLast.Capacity[(int)PayType.Elixir];
				progress.imageFront.fillAmount = (float)def.Capacity[(int)PayType.Elixir]/(float)defLast.Capacity[(int)PayType.Elixir];
			}
			else if(type == BDInfo.Capacity) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)def.eProductionType);
				progress.textInfo.text = "Capacity : "+def.Capacity[(int)def.eProductionType].ToString ("#,##0")+"+"+(defNext.Capacity[(int)defNext.eProductionType]-def.Capacity[(int)def.eProductionType]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)defNext.eProductionType]/(float)defLast.Capacity[(int)defLast.eProductionType];
				progress.imageFront.fillAmount = (float)def.Capacity[(int)def.eProductionType]/(float)defLast.Capacity[(int)defLast.eProductionType];
			}
			else if(type == BDInfo.ProductionRate) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)def.eProductionType);
				progress.textInfo.text = "ProductionRate : "+def.GetProductionPerHour().ToString("#,##0")+"+"+(defNext.GetProductionPerHour()-def.GetProductionPerHour()).ToString("#,##0")+" per Hour";
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : defNext.GetProductionRate()/defLast.GetProductionRate();
				progress.imageFront.fillAmount = def.GetProductionRate()/defLast.GetProductionRate();
			}
			else if(type == BDInfo.HitPoint) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/Heart");
				progress.textInfo.text = "HitPoints : "+def.HitPoint.ToString("#,##0")+"+"+(defNext.HitPoint-def.HitPoint).ToString("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.HitPoint/(float)defLast.HitPoint;
				progress.imageFront.fillAmount = (float)def.HitPoint/(float)defLast.HitPoint;
			}
			else if(type == BDInfo.StorageCapacity) {
				PayType payType = PayType.None;
				for(int i=0 ; i < (int)PayType.Max ; ++i) {
					if(def.Capacity[i] != 0) {
						payType = (PayType)i;
						break;
					}
				}
				if(payType == PayType.None) return;
				
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)payType);
				progress.textInfo.text = "Storage Capacity : "+def.Capacity[(int)payType].ToString("#,##0")+"+"+(defNext.Capacity[(int)payType]-def.Capacity[(int)payType]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = defNext.Capacity[(int)payType]/(float)defLast.Capacity[(int)payType];
				progress.imageFront.fillAmount = def.Capacity[(int)payType]/(float)defLast.Capacity[(int)payType];
			}
			else if(type == BDInfo.TroopCapacity) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/People");
				progress.textInfo.text = "Total Troop Capacity : "+def.TroopCapacity.ToString("#,##0")+"+"+(defNext.TroopCapacity-def.TroopCapacity).ToString("#,##0");
				progress.imageMiddle.fillAmount = (float)defNext.TroopCapacity/(float)defLast.TroopCapacity;
				progress.imageFront.fillAmount = (float)def.TroopCapacity/(float)defLast.TroopCapacity;
			}
			else if(type == BDInfo.TrainingCapacity) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/People");
				progress.textInfo.text = "Training Capacity : "+def.TrainingQueueMax.ToString("#,##0")+"+"+(defNext.TrainingQueueMax-def.TrainingQueueMax).ToString("#,##0");
				progress.imageMiddle.fillAmount = (float)defNext.TrainingQueueMax/(float)defLast.TrainingQueueMax;
				progress.imageFront.fillAmount = (float)def.TrainingQueueMax/(float)defLast.TrainingQueueMax;
			}
			else if(type == BDInfo.DamagePerSecond) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/Training");
				progress.textInfo.text = "Damage Per Second : "+def.DamagePerSecond.ToString("#,##0")+"+"+(defNext.DamagePerSecond-def.DamagePerSecond).ToString("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = def.DamagePerSecond/defLast.DamagePerSecond;
			}
			else {}
		}

	}

}