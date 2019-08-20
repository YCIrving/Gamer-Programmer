using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using conf;
using CocView;
using Newtonsoft.Json;
using Wod.ThirdParty.Util;
using UnityEditor;
using Object = UnityEngine.Object;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          TBDatabase
///   Description:    
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.2 (2016-03-03)
///-----------------------------------------------------------------------------------------
namespace BE {

	// resource price to build buildings
	public enum PayType {
		None	= -1,
		Gold	= 0, 	
		Elixir	= 1,	
		Gem		= 2,
		Max		= 3,
	}

	[System.Serializable]
	public class PayDef {
		
		public int 		ID;
		public string 	Name;
		public Sprite 	Icon;
		public BENumber Number = null;
		public string 	textColor = "";
		
		public PayDef(int _ID, string _Name, Sprite _Icon, string _textColor) {
			ID		= _ID;
			Name 	= _Name;
			Icon	= _Icon;
			textColor	= _textColor;
		}
	}

	// definition of inapp purchase item
	[System.Serializable]
	public class InAppItem {
		
		public string 	Name;	// item name (for example 'pack of gems')
		public int 		Gem;	// gem count to get
		public string 	Price;	// price (for example 0.99$)

		public InAppItem(string _Name, int _Gem, string _Price) {
			Name	= _Name;
			Gem 	= _Gem;
			Price	= _Price;
		}
	}
	
	// definition of troop(army) unit
	[System.Serializable]
	public class ArmyDef {

		public GameObject 	prefab;
		public int 			DamagePerSecond;
		public int 			HitPoint;
		public int 			TrainingCost;
		public int  		ResearchCost;				// research cost to level up in laboratory
		public int			LaboratoryLevelRequired;	// Laboratory Level Required	
		public int  		ResearchTime;				// reserch time

		public ArmyDef(int _DamagePerSecond, int _HitPoint, int _TrainingCost, int _ResearchCost, int _LaboratoryLevelRequired, int _ResearchTime) {
			DamagePerSecond 		= _DamagePerSecond;
			HitPoint				= _HitPoint;
			TrainingCost			= _TrainingCost;
			ResearchCost			= _ResearchCost;
			LaboratoryLevelRequired	= _LaboratoryLevelRequired;
			ResearchTime			= _ResearchTime;		
		}

		public ArmyDef(XmlElement e) {
			DamagePerSecond 		= int.Parse(e.GetAttribute("DamagePerSecond"));
			HitPoint				= int.Parse(e.GetAttribute("HitPoint"));
			TrainingCost			= int.Parse(e.GetAttribute("TrainingCost"));
			ResearchCost			= int.Parse(e.GetAttribute("ResearchCost"));
			LaboratoryLevelRequired	= int.Parse(e.GetAttribute("LaboratoryLevelRequired"));
			ResearchTime			= int.Parse(e.GetAttribute("ResearchTime"));

		}
		
		// check if user has enough resource to training and set value to the text ui with color
		public bool PriceInfoCheck(Text _Price) {
			bool Available = false;
			_Price.text = TrainingCost.ToString ("#,##0");
			if(SceneTown.Elixir.Target () >= TrainingCost)	{ _Price.color = Color.white; Available = true;  }
			else 											{ _Price.color = Color.red;   Available = false; }

			return Available;
		}
		
		public void Save(XmlDocument d, XmlElement parent) {
			
			XmlElement ne = d.CreateElement("ArmyDef"); 					
			ne.SetAttribute("DamagePerSecond", DamagePerSecond.ToString ());		
			ne.SetAttribute("HitPoint", HitPoint.ToString ());		
			ne.SetAttribute("TrainingCost", TrainingCost.ToString ());		
			ne.SetAttribute("ResearchCost", ResearchCost.ToString ());		
			ne.SetAttribute("LaboratoryLevelRequired", LaboratoryLevelRequired.ToString ());		
			ne.SetAttribute("ResearchTime", ResearchTime.ToString ());		

			parent.AppendChild (ne);
		}
	}

	// unit's preferred targets
	public enum FavoriteTarget {
		None 		= 0,
		Resource 	= 1,
		Defense 	= 2,
		Wall	 	= 3,
	};

	// unit's attack type
	public enum AttackType {
		None 		= 0,
		Melee 		= 1,
		Ranged 		= 2,
	};

	// definition of army category type
	[System.Serializable]
	public class ArmyType {

		public int 				ID;
		public string 			Name;
		public Sprite			Icon = null;
		public string 			Info;
		public string 			Desc;

		public FavoriteTarget 	eFavoriteTarget;
		public DamageType 		eDamageType;
		public TargetMove		eTargets;
		public int 				HousingSpace;			// need how many space per i unit
		public int 				TrainingTime=10;
		public float 			MoveSpeed;
		public float 			AttackSpeed;
		public int 				BarrackLevelRequired;	//Barrack Level Required	
		public float 			AttackRange;

		public List<ArmyDef> 	Defs=new List<ArmyDef>();
		
		public ArmyType(int _ID, string _Name, FavoriteTarget _eFavoriteTarget, DamageType _eDamageType, TargetMove _eTargets, 
		                int _HousingSpace, int _TrainingTime, float _MoveSpeed, float _AttackSpeed, int _BarrackLevelRequired, float _AttackRange, 
		                string _Info, string _Desc) {
			ID = _ID;
			Name = _Name;
			Icon = Resources.Load<Sprite>("Icons/Army/"+Name);

			eFavoriteTarget = _eFavoriteTarget;
			eDamageType = _eDamageType;
			eTargets = _eTargets;
			HousingSpace = _HousingSpace;
			TrainingTime = _TrainingTime;
			MoveSpeed = _MoveSpeed;
			AttackSpeed = _AttackSpeed;
			BarrackLevelRequired = _BarrackLevelRequired;	
			AttackRange = _AttackRange;

			Info = _Info;
			Desc = _Desc;
		}
		
		public ArmyType(XmlElement e) {
			
			ID 		= int.Parse(e.GetAttribute("ID"));
			Name 	= e.GetAttribute("Name");
			Icon 	= Resources.Load<Sprite>("Icons/Army/"+Name);
			
			eFavoriteTarget			= (FavoriteTarget)System.Enum.Parse(typeof(FavoriteTarget), e.GetAttribute("FavoriteTarget"));
			eDamageType 			= (DamageType)System.Enum.Parse(typeof(DamageType), e.GetAttribute("DamageType"));
			eTargets 				= (TargetMove)System.Enum.Parse(typeof(TargetMove), e.GetAttribute("Targets"));
			HousingSpace 			= int.Parse(e.GetAttribute("HousingSpace"));
			TrainingTime 			= int.Parse(e.GetAttribute("TrainingTime"));
			MoveSpeed 				= float.Parse(e.GetAttribute("MoveSpeed"));
			AttackSpeed 			= float.Parse(e.GetAttribute("AttackSpeed"));
			BarrackLevelRequired 	= int.Parse(e.GetAttribute("BarrackLevelRequired"));	
			AttackRange 			= float.Parse(e.GetAttribute("AttackRange"));

			LoadInfo(e);

			Defs.Clear ();
			XmlNodeList list = e.ChildNodes;
			foreach(XmlElement ele in list) {
				if(ele.Name == "ArmyDef")
					Add(new ArmyDef (ele));
			}
		}
		
		
		public void Add(ArmyDef def) {
			Defs.Add (def);
			def.prefab = Resources.Load ("Prefabs/Army/"+Name+"_"+Defs.Count.ToString ()) as GameObject;
		}
		
		public ArmyDef GetDefLast() {
			return Defs[Defs.Count -1];
		}
		
		public ArmyDef GetDefine(int level) {
			return Defs[level-1];
		}

		public void Save(XmlDocument d, XmlElement parent) {
			
			XmlElement ne = d.CreateElement("ArmyType"); 					
			ne.SetAttribute("ID", ID.ToString ());							
			ne.SetAttribute("Name", Name);							

			ne.SetAttribute("FavoriteTarget", 		eFavoriteTarget.ToString ());							
			ne.SetAttribute("DamageType", 			eDamageType.ToString ());							
			ne.SetAttribute("Targets", 				eTargets.ToString ());							
			ne.SetAttribute("HousingSpace", 		HousingSpace.ToString ());							
			ne.SetAttribute("TrainingTime", 		TrainingTime.ToString ());							
			ne.SetAttribute("MoveSpeed", 			MoveSpeed.ToString ());							
			ne.SetAttribute("AttackSpeed", 			AttackSpeed.ToString ());							
			ne.SetAttribute("BarrackLevelRequired", BarrackLevelRequired.ToString ());							
			ne.SetAttribute("AttackRange", 			AttackRange.ToString ());							

			parent.AppendChild (ne);
			
			SaveInfo(d, ne);
			
			for(int i=0 ; i < Defs.Count ; ++i) {
				Defs[i].Save(d, ne);
			}
		}
		
		public void SaveInfo(XmlDocument d, XmlElement parent) {
			XmlElement ne = d.CreateElement("Info"); 					
			ne.SetAttribute("Info", Info);		
			ne.SetAttribute("Desc", Desc);		
			parent.AppendChild (ne);
		}
		
		public void LoadInfo(XmlElement e){
			XmlNodeList list = e.ChildNodes;
			foreach(XmlElement ele in list) {
				if(ele.Name == "Info") {	
					Info = ele.GetAttribute("Info");
					Desc = ele.GetAttribute("Desc");
				}
			}
		}

		// fill building info in building info dialog
		public void UIFillProgress(ProgressInfo progress, UnitInfo type, int level) {

			ArmyDef ad = Defs[level-1];
			//ArmyDef adNext = (level < Defs.Count) ? Defs[level] : null;
			ArmyDef adLast = Defs[Defs.Count-1];

			if(type == UnitInfo.DamagePerSecond) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/Damage");
				progress.textInfo.text = "Damage Per Second : "+ad.DamagePerSecond.ToString("#,##0")+"/"+adLast.DamagePerSecond.ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = (float)ad.DamagePerSecond/(float)adLast.DamagePerSecond;
			}
			else if(type == UnitInfo.HitPoint) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/UI/Heart");
				progress.textInfo.text = "HitPoint : "+ad.HitPoint.ToString("#,##0");//+"/"+adLast.HitPoint.ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = (float)ad.HitPoint/(float)adLast.HitPoint;
			}
			else if(type == UnitInfo.TrainingCost) {
				progress.imageIcon.sprite = TBDatabase.GetPayDefIcon((int)PayType.Elixir);
				progress.textInfo.text = "Training Cost : "+ad.TrainingCost.ToString("#,##0");//+"/"+adLast.TrainingCost.ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = (float)ad.TrainingCost/(float)adLast.TrainingCost;
			}
			else {}
		}
	}
	
	public enum DamageType {
		None 			= 0,
		SingleTarget	= 1,
		Splash 			= 2,
	};

	public enum TargetMove {
		None 			= 0,
		Ground			= 1,
		Air 			= 2,
		GroundnAir		= 3,
	};
	
	[System.Serializable]
	public class BuildingDef {

		public GameObject 	prefab;
		public int 			HitPoint;
		public int [] 		BuildPrice = new int[(int)PayType.Max];
		public int 			BuildTime;
		public int 			RewardExp;
		public int			TownHallLevelRequired;		//Town Hall Level Required	

		////Gold Mine, Elixir Collector
		public PayType		eProductionType = PayType.None;
		public int []		ProductionPerHour = new int[(int)PayType.Max];
		public float []		ProductionRate = new float[(int)PayType.Max];
		public int [] 		Capacity = new int[(int)PayType.Max];

		//Barracks
		public int  		TrainingQueueMax=16;

		//Army Camp
		public int  		TroopCapacity=16;

		//Defense
		public float  		DamagePerSecond = 0.0f;
		public float  		DamagePerShot = 0.0f;
		public float  		Range = 0.0f;
		public float  		AttackSpeed = 0.0f;
		public DamageType  	eDamageType = DamageType.None;
		public TargetMove  	eTagetType = TargetMove.None;

		public BuildingDef(int _HitPoint, string _BuildPrice, int _BuildTime, int _LevelRequired, string _Capacity, string _Production, int _TrainingQueueMax, int _TroopCapacity) {
			Init(_HitPoint, _BuildPrice, _BuildTime, _LevelRequired, _Capacity, _Production, _TrainingQueueMax, _TroopCapacity);
		}

		public BuildingDef(XmlElement e) {

			Init(	int.Parse(e.GetAttribute("HitPoint")),
			        e.GetAttribute("BuildPrice"),
			        int.Parse(e.GetAttribute("BuildTime")),
			        int.Parse(e.GetAttribute("TownHallLevelRequired")),
			     	e.GetAttribute("Capacity"),
			        e.GetAttribute("Production"),
			     	int.Parse(e.GetAttribute("TrainingQueueMax")),
			     	int.Parse(e.GetAttribute("TroopCapacity")));

			//ParseStringToArray(Capacity, e.GetAttribute("Capacity"));
			//ParseStringToArray(ProductionPerHour, e.GetAttribute("Production"));
		}

		public void Init(int _HitPoint, string _BuildPrice, int _BuildTime, int _LevelRequired, string _Capacity, string _Production, int _TrainingQueueMax, int _TroopCapacity) {
			HitPoint 				= _HitPoint;
			ParseStringToArray(BuildPrice, _BuildPrice);
			BuildTime				= _BuildTime;
			RewardExp				= (int)Mathf.Sqrt(BuildTime);
			TownHallLevelRequired	= _LevelRequired;		
			ParseStringToArray(Capacity, _Capacity);
			ParseStringToArray(ProductionPerHour, _Production);
			TrainingQueueMax 		= _TrainingQueueMax;	
			TroopCapacity 			= _TroopCapacity;	

			for(int i=0 ; i < (int)PayType.Max ; ++i) {

				if(ProductionPerHour[i] != 0) 
					eProductionType = (PayType)i;

				ProductionRate[i] = (float)ProductionPerHour[i] / 3600.0f; //change time base hr -> sec
			}
		}

		public bool HasCapacity() {
			for(int i=0 ; i < (int)PayType.Max ; ++i) {
				
				if(Capacity[i] != 0)
					return true;
			}

			return false;
		}

		void ParseStringToArray(int [] array, string strValue) {
			string [] strValueSub 	= strValue.Split(',');
			for(int i=0 ; i < (int)PayType.Max ; ++i)
				array[i] = int.Parse(strValueSub[i]);
		}

		public int GetProductionPerHour() {
			return (eProductionType != PayType.None) ? ProductionPerHour[(int)eProductionType] : 0;
		}

		public float GetProductionRate() {
			return (eProductionType != PayType.None) ? ProductionRate[(int)eProductionType] : 0;
		}

		// set tower related values
		public void SetTower(float _DamagePerSecond, float _DamagePerShot, float _Range, float _AttackSpeed, DamageType _eDamageType, TargetMove _eTagetType) {
			DamagePerSecond = _DamagePerSecond;
			DamagePerShot 	= _DamagePerShot;
			Range 			= _Range;
			AttackSpeed 	= _AttackSpeed;
			eDamageType 	= _eDamageType;
			eTagetType 		= _eTagetType;
		}

		// set price icon and value
		public void PriceInfoApply(Image _PriceIcon, Text _Price) {
			for(int i=0 ; i < (int)PayType.Max ; ++i) {
				
				PayDef pd = TBDatabase.GetPayDef(i);
					
				if(BuildPrice[i] != 0) {
					_PriceIcon.sprite = pd.Icon;
					_Price.text = BuildPrice[i].ToString ("#,##0");
					return;
				}
			}			
		}

		// check user has enough resource to build 
		public bool PriceInfoCheck(Text _Price) {

			bool Available = false;
			for(int i=0 ; i < (int)PayType.Max ; ++i) {

				if(BuildPrice[i] != 0) {

					PayDef pd = TBDatabase.GetPayDef(i);

					if(_Price != null) _Price.text = BuildPrice[i].ToString ("#,##0");
					if(pd.Number.Target () >= BuildPrice[i]) 		{ if(_Price != null) _Price.color = Color.white; Available = true;  }
					else 											{ if(_Price != null) _Price.color = Color.red;   Available = false; }
				}
			}			

			return Available;
		}

		public void Save(XmlDocument d, XmlElement parent) {
			
			XmlElement ne = d.CreateElement("BuildingDef"); 					
			ne.SetAttribute("HitPoint", HitPoint.ToString ());		
			string strPrice = "";
			for(int i=0 ; i < (int)PayType.Max ; ++i) {
				strPrice += BuildPrice[i].ToString ();
				if(i < (int)PayType.Max-1) 
					strPrice += ",";
			}
			ne.SetAttribute("BuildPrice", strPrice);	
			ne.SetAttribute("BuildTime", BuildTime.ToString ());	
			ne.SetAttribute("TownHallLevelRequired", TownHallLevelRequired.ToString ());	

			//capacity
			string strCapacity = "";
			for(int i=0 ; i < (int)PayType.Max ; ++i) {
				strCapacity += Capacity[i].ToString ();
				if(i < (int)PayType.Max-1) 
					strCapacity += ",";
			}
			ne.SetAttribute("Capacity", strCapacity);	

			string strProduction = "";
			for(int i=0 ; i < (int)PayType.Max ; ++i) {
				strProduction += ProductionPerHour[i].ToString ();
				if(i < (int)PayType.Max-1) 
					strProduction += ",";
			}
			ne.SetAttribute("Production", strProduction);	

			ne.SetAttribute("TrainingQueueMax", TrainingQueueMax.ToString ());	
			ne.SetAttribute("TroopCapacity", TroopCapacity.ToString ());	

			parent.AppendChild (ne);
		}
	}

	// class for building category type
	[System.Serializable]
	public class BuildingType
	{
		//public int 					TB;
		public Vector3 				Scale;
		public Vector3 				Pos;
		public int 					ID;
		public string 				Name;		// name og the building
		public string 				displayName;
		public Sprite				Icon = null;
		public string 				Info;		// description of the building
		public string 				Desc;		// description of the building
		public int 					TileX;		// needed tile size with
		public int 					TileZ;		// needed tile size height
		public TargetType 			Category;
		public int []				MaxCount;//MaxCountByTownHallLevel
		public List<BuildingDef> 	Defs=new List<BuildingDef>();
		public int 					Main_type;

		public BuildingType() {
		}

		public BuildingType(int _ID, string _Name, string _displayName, int _TileX, int _TileZ, TargetType _Category, string _MaxCount, string _Info, string _Desc) {
			ID = _ID;
			Name = _Name;
			displayName = _displayName;
			//Icon = Resources.Load<Sprite>("Icons/Building/"+Name);
			ResourceManager.Instance.LoadAsset<Sprite>("Assets/_Res/Building/" + Name,OnLoadSprite);
			Info = _Info;
			Desc = _Desc;
			TileX = _TileX;
			TileZ = _TileZ;
			Category = _Category;

			string [] Sub 	= _MaxCount.Split(',');
			if(Sub.Length > 0) {
				MaxCount = new int[Sub.Length];
				for(int i=0 ; i < Sub.Length ; ++i) {
					MaxCount[i] = int.Parse (Sub[i]);
				}
			}
			//Debug.Log ("BuildingType:BuildingType Icon:"+Icon);
		}

		private void OnLoadSprite(Sprite arg1, bool arg2)
		{
			if (arg2)
			{
				Sprite sp = (Sprite)arg1;
				Icon = sp;
				LoggerHelper.Debug("xxx:");
			}
		}

		public BuildingType(XmlElement e) {

			ID 					= int.Parse(e.GetAttribute("ID"));
			Name 				= e.GetAttribute("Name");
			Icon 				= Resources.Load<Sprite>("Icons/Building/"+Name);

			string [] SubTileSize 	= e.GetAttribute("TileSize").Split(',');
			TileX 				= int.Parse(SubTileSize[0]);
			TileZ 				= int.Parse(SubTileSize[1]);
			Category 			= (TargetType)System.Enum.Parse(typeof(TargetType), e.GetAttribute("Category"));

			LoadInfo(e);

			string [] Sub 	= e.GetAttribute("MaxCount").Split(',');
			if(Sub.Length > 0) {
				MaxCount = new int[Sub.Length];
				for(int i=0 ; i < Sub.Length ; ++i) {
					MaxCount[i] = int.Parse (Sub[i]);
				}
			}
			
			Defs.Clear ();
			XmlNodeList list = e.ChildNodes;
			foreach(XmlElement ele in list) {
				if(ele.Name == "BuildingDef")
					Add(new BuildingDef (ele));
			}
		}


		public void Add(BuildingDef def) {
			// deprecated in reading json file
			Defs.Add (def);
			// set mesh prefab to each Building definition
			//def.prefab = Resources.Load ("Prefabs/Building/"+Name+"_"+Defs.Count.ToString ()) as GameObject;
			
			def.prefab = Resources.Load ("Prefabs/Building/"+Name) as GameObject;
			
			// directly build prefabs rather than reading from files
			// GameObject go = new GameObject(Name);
			// go.AddComponent<CanvasRenderer>();
			// go.AddComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Res/Building/" + Name);
			// def.prefab = go;			
		}

		public BuildingDef GetDefLast() {
			return Defs[Defs.Count -1];
		}

		public BuildingDef GetDefine(int level) {
			return ((level < 1) || (Defs.Count < level)) ? null : Defs[level-1];
		}
		
		public int GetMoreBuildTownLevel(int CurrentCount) {
			for(int i=0 ; i < MaxCount.Length ; ++i) {
				if(MaxCount[i] > CurrentCount)
					return i+1;
			}
			return -1;
		}

		public int GetMaxLevleByTownLevel(int TownLevel) {
			int Level = 1;
			for(int i=0 ; i < Defs.Count ; ++i) {
				if(Defs[i].TownHallLevelRequired <= TownLevel)
					Level = i+1;
			}
			return Level;
		}

		public void Save(XmlDocument d, XmlElement parent) {

			string strMaxCount = "";
			for(int i=0 ; i < MaxCount.Length ; ++i) {
				strMaxCount += MaxCount[i].ToString ();
				if(i != MaxCount.Length-1) strMaxCount += ",";
			}

			XmlElement ne = d.CreateElement("BuildingType"); 					
			ne.SetAttribute("ID", ID.ToString ());							
			ne.SetAttribute("Name", Name);							
			ne.SetAttribute("TileSize", TileX.ToString ()+","+TileZ.ToString ());	
			ne.SetAttribute("Category", Category.ToString ());	
			ne.SetAttribute("MaxCount", strMaxCount);	

			parent.AppendChild (ne);

			SaveInfo(d, ne);

			for(int i=0 ; i < Defs.Count ; ++i) {
				Defs[i].Save(d, ne);
			}
		}

		public void SaveInfo(XmlDocument d, XmlElement parent) {
			XmlElement ne = d.CreateElement("Info"); 					
			ne.SetAttribute("Info", Info);		
			ne.SetAttribute("Desc", Desc);		
			parent.AppendChild (ne);
		}

		public void LoadInfo(XmlElement e){
			XmlNodeList list = e.ChildNodes;
			foreach(XmlElement ele in list) {
				if(ele.Name == "Info") {	
					Info = ele.GetAttribute("Info");
					Desc = ele.GetAttribute("Desc");
				}
			}
		}
	}


	[System.Serializable]
	public class TBDatabase : MonoBehaviour {

		public  static TBDatabase instance;
		public  static int		 ConfigVersion = 2;
		public  static int		 LoadedVersion = 1;

		private string 				dbFilename = "Database.xml";
		private string 				strDataBasePath;
		public  const int 			MAX_LEVEL = 230;

		public 	AudioClip[]   		audioClip;

		public  List<PayDef>		Pays			= new List<PayDef>();
		public  List<InAppItem> 	InApps			= new List<InAppItem>();
		public  int []				LevelExp		= new int[MAX_LEVEL+1];
		public  int []				LevelExpTotal	= new int[MAX_LEVEL+1];
		public  List<BuildingType> 	Buildings		= new List<BuildingType>();
		public 	Dictionary<int, BuildingType>	BuildingsDic = new Dictionary<int, BuildingType>();
		public  List<ArmyType> 		Armies			= new List<ArmyType>();
		
		void Awake () {
			instance=this;


			//TargetType tt = (TargetType)System.Enum.Parse( typeof(TargetType), "Building" );
			//TargetType tt2 = (TargetType)System.Enum.Parse( typeof(TargetType), "Tower" );
			//TargetType tt3 = (TargetType)System.Enum.Parse( typeof(TargetType), "TargetType.Building" );


			Pays.Add (new PayDef(Pays.Count, "Gold",   Resources.Load<Sprite>("Icons/UI/Gold"), 	"<color=orange>"));
			Pays.Add (new PayDef(Pays.Count, "Elixir", Resources.Load<Sprite>("Icons/UI/Elixir"), 	"<color=purple>"));
			Pays.Add (new PayDef(Pays.Count, "Gem",    Resources.Load<Sprite>("Icons/UI/Gem"), 		"<color=purple>"));

			//add InApp purchase item
			InApps.Add (new InAppItem("Pile of Diamonds",    500, "$4.99"));
			InApps.Add (new InAppItem("Pouch of Diamonds",  1200, "$9.99"));
			InApps.Add (new InAppItem("Bag of Diamonds",    2500, "$19.99"));
			InApps.Add (new InAppItem("Box of Diamonds",    6500, "$49.99"));
			InApps.Add (new InAppItem("Crate of Diamonds", 14000, "$99.99"));

			// set experience values to each level
			for(int Level=0 ; Level <= MAX_LEVEL ; ++Level) {
				LevelExp[Level] = Level * 50 + Mathf.Max(0, (Level - 199) * 450);
				LevelExpTotal[Level] = Level * (Level - 1) * 25;
				//Debug.Log ("Level "+Level.ToString ()+" - Exp:"+LevelExp[Level].ToString ()+" ExpTotal:"+LevelExpTotal[Level].ToString ());
			}


/*			//textInfo.text = "Cannons are great for point defense. Upgrade cannons to increase their firepower, but beware that your defensive turrets cannot shoot while being upgraded!";
			//textInfo.text = "Archer Towers have longer range than cannons, and unlike cannons they can attack flying enemies.";

			// if set building type and definition data by coding
			// use this code
			//0-Town Hall
			{
				BuildingType bt = new BuildingType(0, "Town Hall", 4, 4, TargetType.Building, "1,1,1,1,1,1,1,1,1,1", "Info", "This is the heart of your village. Upgrading your Town Hall unlocks new defenses, buildings, traps and much more.");
				bt.Add(new BuildingDef (1500,   "0,0,0",         0, 0, "1000,1000,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef (1600,   "1000,0,0",     10, 1, "1000,1000,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef (1850,   "4000,0,0",  10800, 2, "1000,1000,0", "0,0,0", 0, 0));
				Buildings.Add (bt);
			}

			//1-Hut
			{
				BuildingType bt = new BuildingType(1, "Hut", 2, 2, TargetType.Building, "0,0,0,0,0,0,0,0,0,0", "Info", "Nothing gets done around here without Builders! You can hire more Builders to start multiple construction projects, or speed up their work by using green gems.");
				bt.Add(new BuildingDef ( 250,   "0,0,0",         0, 0, "0,0,0", "0,0,0", 0, 0));
				Buildings.Add (bt);
			}
			
			//2-Wall
			{
				BuildingType bt = new BuildingType(2, "Wall", 1, 1, TargetType.Wall, "0,25,50,75,100,125,175,225,250,250", "Info", "Walls are great for keeping your village safe and your enemies in the line of fire.");
				bt.Add(new BuildingDef ( 300,  "50,0,0",         0, 2, "0,0,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef ( 500,  "1000,0,0",       0, 2, "0,0,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef ( 700,  "5000,0,0",       0, 3, "0,0,0", "0,0,0", 0, 0));
				Buildings.Add (bt);
			}

			//3-Gold Mine
			{
				BuildingType bt = new BuildingType(3, "Gold Mine", 3, 3, TargetType.Resource, "1,2,3,4,5,6,6,6,6,7", "Info", "The Gold Mine produces gold. Upgrade it to boost its production and gold storage capacity.");
				bt.Add(new BuildingDef ( 400, "0,150,0",        10, 1, " 500,0,0", "200,0,0", 0, 0));
				bt.Add(new BuildingDef ( 440, "0,300,0",        60, 1, "1000,0,0", "400,0,0", 0, 0));
				bt.Add(new BuildingDef ( 480, "0,700,0",       900, 2, "1500,0,0", "600,0,0", 0, 0));
				Buildings.Add (bt);
			}

			//4-Elixir Collector
			{
				BuildingType bt = new BuildingType(4, "Elixir Collector", 3, 3, TargetType.Resource, "1,2,3,4,5,6,6,6,6,7", "Info", "Elixir is pumped from the Ley Lines coursing underneath your village. Upgrade your Elixir Collectors to maximize elixir production.");
				bt.Add(new BuildingDef ( 400, "150,0,0",       10, 1, "0,   500,0", "0,200,0", 0, 0));
				bt.Add(new BuildingDef ( 440, "300,0,0",       60, 1, "0,  1000,0", "0,400,0", 0, 0));
				bt.Add(new BuildingDef ( 480, "700,0,0",      900, 2, "0,  1500,0", "0,600,0", 0, 0));
				Buildings.Add (bt);
			}

			//5-Gold Storage
			{
				BuildingType bt = new BuildingType(5, "Gold Storage", 3, 3, TargetType.Resource, "1,1,2,2,2,2,2,3,4,4", "Info", "All your precious gold is stored here. Don't let sneaky goblins anywhere near! Upgrade the storage to increase its capacity and durability against attack.");
				bt.Add(new BuildingDef ( 400, "0,300,0",       10, 1, "1000,0,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef ( 600, "0,750,0",     1800, 2, "3000,0,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef ( 800, "0,1500,0",     600, 2, "6000,0,0", "0,0,0", 0, 0));
				Buildings.Add (bt);
			}
			
			//6-Elixir Storage
			{
				BuildingType bt = new BuildingType(6, "Elixir Storage", 3, 3, TargetType.Resource, "1,1,2,2,2,2,2,3,4,4", "Info", "These storages contain the elixir pumped from underground. Upgrade them to increase the maximum amount of elixir you can store.");
				bt.Add(new BuildingDef ( 400, "300,0,0",       10, 1, "0,1000,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef ( 600, "750,0,0",     1800, 2, "0,3000,0", "0,0,0", 0, 0));
				bt.Add(new BuildingDef ( 800, "1500,0,0",    3600, 2, "0,6000,0", "0,0,0", 0, 0));
				Buildings.Add (bt);
			}

			//7-Barracks
			{
				BuildingType bt = new BuildingType(7, "Barrack", 3, 3, TargetType.Building, "1,2,2,3,3,3,4,4,4,4", "Info", "The Barracks allow you to train troops to attack your enemies. Upgrade the Barracks to unlock advanced units that can win epic battles.");
				bt.Add(new BuildingDef ( 250, "0,200,0",       10, 1, "0,0,0", "0,0,0", 8, 0));
				Buildings.Add (bt);
			}

			//8-Army Camp
			{
				BuildingType bt = new BuildingType(8, "Army Camp", 4, 4, TargetType.Building, "1,1,2,2,3,3,4,4,4,4", "Info", "Your troops are stationed in Army Camps. Build more camps and upgrade them to muster a powerful army.");
				bt.Add(new BuildingDef ( 250, "0,250,0",      300, 1, "0,0,0", "0,0,0", 0, 20));
				Buildings.Add (bt);
			}


			// unit definiron for training
			//0-Barbarian
			{
				ArmyType at = new ArmyType(0, "Barbarian", FavoriteTarget.None, DamageType.SingleTarget, MoveType.Ground, 1, 20, 16.0f, 1.0f, 1, 0.4f, "", "");
				at.Add (new ArmyDef (  8,  45,  25,       0, 0,       0));
				at.Add (new ArmyDef ( 11,  54,  40,   50000, 1,   21600));//6hr
				Armies.Add (at);
			}
			
			//1-Archer
			{
				ArmyType at = new ArmyType(1, "Archer", FavoriteTarget.None, DamageType.SingleTarget, MoveType.Ground, 1, 25, 24.0f, 1.0f, 2, 3.5f, "", "");
				at.Add (new ArmyDef (  7,  20,  50,       0, 0,       0));
				at.Add (new ArmyDef (  9,  23,  80,   50000, 1,   43200));//12hr
				Armies.Add (at);
			}

			//2-Minion
			{
				ArmyType at = new ArmyType(1, "Minion", FavoriteTarget.None, DamageType.SingleTarget, MoveType.Air, 2, 45, 32.0f, 1.0f, 1, 2.75f, "", "");
				at.Add (new ArmyDef (  35,  55,  6,       0, 0,       0));
				at.Add (new ArmyDef (  38,  60,  7,   10000, 5,   43200));//12hr
				Armies.Add (at);
			}
			Save ();


*/
			strDataBasePath += "Assets/_Res/Tables";

			
			#if UNITY_EDITOR
			Tables.LoadTdBuildingArtModelConf(AssetDatabase
				.LoadAssetAtPath<TextAsset>(strDataBasePath + "/td_building_art_model.json.txt").text);
			
			Tables.LoadTdBuildingConf(AssetDatabase
				.LoadAssetAtPath<TextAsset>(strDataBasePath + "/td_building.json.txt").text);
			#endif

		}
		
		void Start () {

			OnLoadComplete();
		}
		
		void Update () {
		
		}

		public void Save() {


			string xmlFilePath = BEUtil.pathForDocumentsFile(dbFilename);
			Debug.Log ("TBDatabase.Save() "+xmlFilePath);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<Database><name>wrench</name></Database>");
			{
				xmlDocument.DocumentElement.RemoveAll();
								
				XmlElement buildingRoot = xmlDocument.CreateElement("Building"); 
				xmlDocument.DocumentElement.AppendChild (buildingRoot);
				foreach(BuildingType bt in Buildings) {
					bt.Save (xmlDocument, buildingRoot);
				}
			
				// ####### Encrypt the XML ####### // If you want to view the original xml file, turn of this piece of code and press play.
				if (xmlDocument.DocumentElement.ChildNodes.Count >= 1) {
					xmlDocument.Save (xmlFilePath);
				}
				// ###############################
			}
		}

		private String GetIconName(String IconName)
		{
			if (IconName.Contains("[") && IconName.Contains("]"))
			{
				var split = IconName.IndexOf('[');
				var mainStr = IconName.Substring(0, split);
				var str = IconName.Substring(split+1, IconName.Length - split-2);
				var strs = str.Split(',');
				return mainStr + strs[0] + ".png";
			}
			return "Assets/_Res/Building/" + IconName + ".png";
		}
		
		private void OnLoadComplete()
		{
			foreach (var item in Tables.TdBuildingConf.Records)
			{
				// 对list格式的Buildings添加元素
				Buildings.Add(new BuildingType()
				{
					Category = TargetType.Building,
					Defs = new List<BuildingDef>(),
					Desc = string.Empty,
					ID = (int) item.ID,
					Info = item.Des,
					MaxCount = new[] {9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999},
					Name = item.Model.EditorIcon,
					displayName = item.Name,
					TileX = item.Model.Size,
					TileZ = item.Model.Size,
#if UNITY_EDITOR
					Icon = AssetDatabase.LoadAssetAtPath<Sprite>(GetIconName(item.Model.EditorIcon)),
#endif
					//TB = item.Tb,
					Scale = new Vector3(item.Model.EditorScale, item.Model.EditorScale, item.Model.EditorScale),
					Main_type = item.MainType,
					Pos = new Vector3(item.Model.EditorPos[0], item.Model.EditorPos[1], item.Model.EditorPos[2])
				});
				
				// 对Dictionary格式的BuildingsDic添加条目
				BuildingsDic.Add((int) item.ID, new BuildingType()
				{
					Category = TargetType.Building,
					Defs = new List<BuildingDef>(),
					Desc = string.Empty,
					ID = (int) item.ID,
					Info = item.Des,
					MaxCount = new[] {9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999},
					Name = item.Model.EditorIcon,
					displayName = item.Name,
					TileX = item.Model.Size,
					TileZ = item.Model.Size,
#if UNITY_EDITOR
					Icon = AssetDatabase.LoadAssetAtPath<Sprite>(GetIconName(item.Model.EditorIcon)),
#endif
					//TB = item.Tb,
					Scale = new Vector3(item.Model.EditorScale, item.Model.EditorScale, item.Model.EditorScale),
					Main_type = item.MainType,
					Pos = new Vector3(item.Model.EditorPos[0], item.Model.EditorPos[1], item.Model.EditorPos[2])
				});
				
			}

			Buildings.Sort(new BuildingsCompare());
			BEGround.instance.Awake();

		}

		/*public void OnLoadComplete(Object obj, bool bOk)
		{
			if (bOk)
			{
				TextAsset text = (TextAsset)obj;
				Tables.LoadBuildingTdConf(text.text);		
				foreach (var item in Tables.BuildingTdConf.Records)
				{
					Buildings.Add (new BuildingType()
					{
						Category = TargetType.Building,
						Defs = new List<BuildingDef>(),
						Desc = string.Empty,
						ID = (int) item.ID,
						Info = item.Des,
						MaxCount = new []{999,999,999,999,999,999,999,999,999,999},
						Name = item.IconName,
						TileX = item.Size,
						TileZ = item.Size,
						Icon = Resources.Load<Sprite>("Icons/Building/"+item.IconName),
						TB = (int)item.Tb,
					});
				}
				Buildings.Sort(new BuildingsCompare());
				BEGround.instance.Awake();
			}
		}*/
		public class BuildingsCompare : IComparer<BuildingType>
		{
			public int Compare(BuildingType x, BuildingType y)
			{
				return BuildCompare(x, y);
			}
		}
		
		static int BuildCompare(BuildingType x, BuildingType y)
		{
			if (x.ID > y.ID)
			{
				return 1;
			}
			else if (x.ID == y.ID)
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}

		public void Load() {

			//Debug.Log ("TBDatabase.Load()");
			
			//TextAsset textAsset = new TextAsset();
			//textAsset = (TextAsset)Resources.Load(dbFilename, typeof(TextAsset));

			TextAsset textAsset = (TextAsset) Resources.Load("Database");  
			XmlDocument xmlDocument = new XmlDocument ();
			xmlDocument.LoadXml ( textAsset.text );

			//string xmlFilePath = BEUtil.pathForDocumentsFile(dbFilename);
			//XmlDocument xmlDocument = new XmlDocument();
			//xmlDocument.Load(xmlFilePath);

			Buildings.Clear();
			BuildingsDic.Clear();
			Armies.Clear();
			
			if(xmlDocument != null) {
				XmlElement element = xmlDocument.DocumentElement;
				XmlNodeList list = element.ChildNodes;
				foreach(XmlElement ele in list) {
					if(ele.Name == "Building")		{	
						XmlNodeList list2 = ele.ChildNodes;
						foreach(XmlElement ele2 in list2) {
							if(ele2.Name == "BuildingType")
							{
								Buildings.Add(new BuildingType(ele2));
								BuildingsDic.Add(int.Parse(ele2.GetAttribute("ID")), 
									new BuildingType(ele2));
							}
							
						}
					}
					else {}
				}
			}
		}

		public static int			GetGemBySec(int sec) {
			if(sec <= 0) 			return 0;
			else if(sec <= 60)		return 1;
			else if(sec <= 3600)	return (int)((float)(sec-60)   *(float)(20-1)    /(float)(3600-60)) + 1;
			else if(sec <= 86400)	return (int)((float)(sec-3600) *(float)(260-20)  /(float)(86400-3600)) + 20;
			else 					return (int)((float)(sec-86400)*(float)(1000-260)/(float)(604800-86400)) + 260;
		}

		public static int			GetPayCount() 						{ return instance.Pays.Count; }
		public static PayDef		GetPayDef(int id) 					{ return (id >= 0) ? instance.Pays[id] : null; }
		public static Sprite		GetPayDefIcon(int id) 				{ return (id >= 0) ? instance.Pays[id].Icon : null; }

		public static AudioClip 	GetAudio(int id) 					{ return instance.audioClip[id]; }
		public static int			GetInAppItemCount() 				{ return instance.InApps.Count; }
		public static InAppItem		GetInAppItem(int id) 				{ return instance.InApps[id]; }
		public static int			GetLevel(int expTotal) { 
			for(int Level=1 ; Level < MAX_LEVEL ; ++Level) {
				if((instance.LevelExpTotal[Level] <= expTotal) && (expTotal < instance.LevelExpTotal[Level+1]))
					return Level;
			}
			return -1;
		}


		public static int			GetLevelExp(int level) 				{ return instance.LevelExp[level]; }
		public static int			GetLevelExpTotal(int level) 		{ return instance.LevelExpTotal[level]; }
		public static int			GetBuildingCount() 					{ return instance.Buildings.Count; }
		public static string		GetBuildingName(int type) 			{ return instance.Buildings[type].Name; }
		public static string		GetBuildingDisplayName(int type) 	{ return instance.Buildings[type].displayName; }
		public static BuildingType	GetBuildingType(int type) 			{ return instance.Buildings[type]; }

		public static string GetBuildingNameByID(int id)
		{
			return instance.BuildingsDic[id].Name;
		}
		public static string GetBuildingDisplayNameByID(int id)
		{
			return instance.BuildingsDic[id].displayName;
		}
		public static BuildingType GetBuildingTypeByID(int id)
		{
			return instance.BuildingsDic[id]; 
		}
		
		public static BuildingDef 	GetBuildingDef(int type, int Level) { return (Level > 0) ? instance.Buildings[type].Defs[Level-1] : null; }
		public static int			GetArmyTypeCount() 					{ return instance.Armies.Count; }
		public static ArmyType		GetArmyType(int type) 				{ return (type < instance.Armies.Count) ? instance.Armies[type] : null; }
		public static ArmyDef 		GetArmyDef(int type, int Level) 	{ return (Level > 0) ? instance.Armies[type].Defs[Level-1] : null; }
	}

	
}
