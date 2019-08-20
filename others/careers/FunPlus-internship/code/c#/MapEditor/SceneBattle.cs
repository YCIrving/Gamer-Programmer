using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;  

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          SceneBattle
///   Description:    main class of town 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-02-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class BattleButton {
		public  int			ID = -1;
		public	Transform	tr = null;
		public	Image		Border = null;
		public	Image		Icon = null;
		public  Text		Info = null;
		public	Image		Cooltime = null;
		public 	BETween		bt = null;

		public	bool		UseCooltime = true;
		public	int			UseCount = 0;
		public	bool		Used = false;
		public  float		CooltimePeriod = 3.0f;
		public  float		timeAfterUse = 0.0f;

		public BattleButton(int _ID, GameObject go, bool _UseCooltime) {
			ID = _ID;
			tr = go.transform;
			Border = go.GetComponent<Image>();
			Border.color = new Color(1,1,1,0);
			Icon = tr.Find ("Background").Find ("Icon").GetComponent<Image>();
			Info = tr.Find ("Background").Find ("Text").GetComponent<Text>();
			if(tr.Find ("Background").Find ("CoolTime") != null)
				Cooltime = tr.Find ("Background").Find ("CoolTime").GetComponent<Image>();

			UseCooltime = _UseCooltime;
		}
		
		public void SelectToggle() {
			if(bt != null) {
				bt.Clear();
				bt = null;
				tr.localScale = Vector3.one;
				Border.color = new Color(1,1,1,0);
			}
			else {
				bt = BETween.scale(tr.gameObject, 0.5f, new Vector3(1.0f,1.0f,1.0f), new Vector3(1.1f,1.1f,1.1f));
				bt.loopStyle = BETweenLoop.pingpong;
				bt.loopCount = 100000;
				bt.method = BETweenMethod.easeOut;
				Border.color = new Color(1,1,1,1);
			}
		}

		public virtual bool Use() {
			return false;
		}

		public virtual void UpdateUI() {
		}

		public void Update(float deltaTime) {
			if(!UseCooltime) return;
			
			if(Used) {
				timeAfterUse += deltaTime;
				if(timeAfterUse > CooltimePeriod) {
					timeAfterUse = 0.0f;
					Used = false;
				}
				
				Cooltime.fillAmount = Used ? (CooltimePeriod-timeAfterUse)/CooltimePeriod : 0.0f;
			}
		}
	}

	public class BattleUnit : BattleButton {
		public  int			CountMax;
		public  int			Count;

		public BattleUnit(int _ID, GameObject go, int _Count) : base(_ID, go, false) {
			CountMax = Count = _Count;

			UpdateUI();

			ArmyType at = TBDatabase.GetArmyType(_ID);
			Icon.sprite = at.Icon;
		}

		public bool Commit() {
			if(Count <= 0) return false;
			Count--;
			UpdateUI();

			return true;
		}

		public override void UpdateUI() {
			Info.text = (Count > 0) ? "x"+Count.ToString () : "";
			// turn grayscale if Count == 0
		}
	}
	
	public class BattleItem : BattleButton {
		public  int			Price=6;

		public BattleItem(int _ID, GameObject go, bool _UseCooltime) : base(_ID, go, _UseCooltime) {
			UpdateUI();

			if(_ID == 0) 		Icon.sprite = Resources.Load<Sprite>("Icons/UI/Bomb");
			else if(_ID == 1) 	Icon.sprite = Resources.Load<Sprite>("Icons/UI/MultiMissle");
			else {}
		}

		public override bool Use() {
			if(!Used && SceneBattle.instance.EnergyDelta(-Price)) {

				UseCount++;
				Used = true;
				Price += 4;		// increase price
				UpdateUI();
				return true;
			}
			else {
				return false;
			}
		}
		
		public override void UpdateUI() {
			Info.text = (Price > 0) ? Price.ToString () : "";
			// turn grayscale if Count == 0
		}
	}
	
	public class SceneBattle : SceneBase, BECameraRTSListner {
		
		public	static SceneBattle instance;

		public 	GameObject 	prefMissle=null;
		public 	GameObject 	prefSmallMissle=null;
		public 	GameObject 	prefUIRewardItem=null;
		public 	GameObject 	prefUIBattleUnit=null;
		public 	GameObject 	prefUIBattleItem=null;
		public 	BEGround 	ground=null;
		public  Text		textEnemyLevel;
		public  Text		textEnemyName;
		public  GameObject	goReward;
		public  Text		textInfo;
		public  Text		textTime;
		public  GameObject	goButtonRetreat;
		public  Text		textButtonRetreat;
		public  GameObject	goLootAvailable;
		public  GameObject	goLootGained;
		public  Text		textEnergy;
		public  RectTransform	trRootEnergy;
		public	Transform 		trUnitRoot = null;
		public  Transform		trRootRewardList;
		public  Transform		trRootButtonUnit;
		public  Transform		trRootButtonItem;

		private bool		GameWin = false;
		private bool		InGame = false;
		private bool		GameEnd = false;
		//private bool		GameCanceled = false;
		private float 		timeStart = 30.0f; // time before game start
		private float 		timeEnd = 240.0f;  // actual battle period
		public 	List<BattleUnit> 	Units = new List<BattleUnit>();
		public 	List<BattleItem> 	Items = new List<BattleItem>();
		private List<BattleButton> 	Buttons = new List<BattleButton>();

		private int				Energy = 30;	// set initial Energy value to 30
		private int				UnitSelected = -1;
		private int				ItemSelected = -1;
		private BattleButton	BBSelected = null;

		// for commit troops to ground
		private bool		CommitEnabled = false;
		private float		CommitPeriod = 0.25f;
		private float		CommitAge = 0.0f;

		public 	List<Building>	Buildings = new List<Building>();
		public  BENumber	Gold;
		public  BENumber	Elixir;
		public  BENumber	PlunderGold;
		public  BENumber	PlunderElixir;
		public 	int []		Casualties = null;

		private Vector3 	vPosClicked = Vector3.zero;
		private int 		ButtonRetreatState = 0;

		private	string		strMapsPath = "";

		void Awake () {
			instance=this;
			Init(SceneType.Battle);

			Gold = new BENumber(BENumber.IncType.VALUE, 0, 200000, 10000);
			Gold.AddUIText(BEUtil.GetObject("PanelOverlay/LootAvailable/LabelGold").GetComponent<Text>());

			Elixir = new BENumber(BENumber.IncType.VALUE, 0, 300000, 10000);
			Elixir.AddUIText(BEUtil.GetObject("PanelOverlay/LootAvailable/LabelElixir").GetComponent<Text>());

			PlunderGold = new BENumber(BENumber.IncType.VALUE, 0, 200000, 0);
			PlunderGold.AddUIText(BEUtil.GetObject("PanelOverlay/LootGained/LabelGold").GetComponent<Text>());
			
			PlunderElixir = new BENumber(BENumber.IncType.VALUE, 0, 300000, 0);
			PlunderElixir.AddUIText(BEUtil.GetObject("PanelOverlay/LootGained/LabelElixir").GetComponent<Text>());
			
			//Set BENumber to TBDatabase's PayDef
			TBDatabase.GetPayDef((int)PayType.Gold).Number = Gold;
			TBDatabase.GetPayDef((int)PayType.Elixir).Number = Elixir;

			// Get Maps Folder Path
			strMapsPath = Application.dataPath;
			strMapsPath = strMapsPath.Substring(0, strMapsPath.LastIndexOf( '/' ) );
			strMapsPath += "/Assets/NoBuild/Resources/Maps";
		}
		
		void Start () {

			Time.timeScale = 1;
			BECameraRTS.instance.Listner = this;

			goLootAvailable.SetActive(false);
			goLootGained.SetActive(false);

			// load game data from xml file
			configFilename = DataOverScene.BaseName;
			textEnemyName.text = configFilename;
			Load ();
			
			//set resource's capacity
//			BEGround.instance.CapacityCheck();

			// add unit to commit in game
			for(int i=trRootButtonUnit.childCount-1;i>=0;i--){
				Destroy (trRootButtonUnit.GetChild(i).gameObject);
			}
			LoadArmy();
			//AddButtonUnit(0,10);
			//AddButtonUnit(1,10);
			//AddButtonUnit(2,10);
			// first unit is selected at starting time
			OnButtonUnit(0); 

			// add items which used in game
			for(int i=trRootButtonItem.childCount-1;i>=0;i--){
				Destroy (trRootButtonItem.GetChild(i).gameObject);
			}
			AddButtonItem(0,true);
			AddButtonItem(1,true);

			EnergyDelta(0);

			if(Items.Count > 4)
				trRootEnergy.anchoredPosition = new Vector2(-74,146+114);
			
			// initial Cam position & zoom setting
			BECameraRTS.instance.SetCameraPosition(Vector3.zero);
			BECameraRTS.instance.SetCameraZoom(50);
		}

		public override void Update () {
			
			base.Update();

			// get delta time from BETime
			float deltaTime = BETime.deltaTime;
			
			// to avoid initial update delay,
			// call this cam animation  in 2nd update frame
			UpdateCount++;
			if(UpdateCount == 2) {
				float zoomTarget = 40.0f;
				BETween bt = BETween.position(BECameraRTS.instance.trCamera.gameObject, 1.0f, new Vector3(0,0,-zoomTarget));
				bt.method = BETweenMethod.easeOut;
				bt.onFinish = () => { BECameraRTS.instance.SetCameraZoom(zoomTarget); };
			}

			Gold.Update();
			Elixir.Update();
			PlunderGold.Update();
			PlunderElixir.Update();

			//Debug.Log ("PlunderGold:"+PlunderGold.Target()+" PlunderElixir:"+PlunderElixir.Target());
			// if game is not ended
			if(!GameEnd) {

				// if game is not even started yet
				if(!InGame) {
					timeStart -= deltaTime;

					if(timeStart < 0.0f) 	{ GameStart(); }
					else 					{ textTime.text = BENumber.SecToString((int)timeStart); }
				}
				// if game is inprogress
				else {
					timeEnd -= deltaTime;

					if(timeEnd < 0.0f) 		{ GameEndProcess(); }
					else 					{ textTime.text = BENumber.SecToString((int)timeEnd); }

					int UnDeployedUnitCount = 0;
					for(int i=0 ; i < Units.Count ; ++i) {
						UnDeployedUnitCount += Units[i].Count;
					}
					
					if((UnDeployedUnitCount == 0) && (BEUnitManager.instance.GetLiveUnitCount() == 0)) {
						GameEndProcess();
					}
				}
			}
			else {

			}

			for(int i=0 ; i < Buttons.Count ; ++i) {
				Buttons[i].Update(deltaTime);
			}

			// if user keep pressing unit button
			if(CommitEnabled) {
				CommitAge += Time.deltaTime;

				// add unit periodically
				while(CommitAge >= CommitPeriod) {
					CommitAge -= CommitPeriod;
					EnemyAdd();
				}
				//Debug.Log ("CommitAge "+CommitAge.ToString ());
			}
		}

		public bool EnergyDelta(int value) {
			if(value < 0) {
				if(Energy+value >= 0) {
					Energy += value;
					textEnergy.text = Energy.ToString();
					return true;
				}
				else {
					return false;
				}
			}
			else {
				Energy += value;
				textEnergy.text = Energy.ToString();
				return true;
			}
		}

		public void GameStart() {
			if(InGame) return;

			InGame = true;
			textInfo.text = "Battle ends in:";
			textButtonRetreat.text = "Retreat";
			goReward.SetActive(false);
			goLootAvailable.SetActive(true);
			goLootGained.SetActive(true);
		}

		public void GameEndProcess() {
			if(!InGame) return;
			if(GameEnd) return;

			if(Buildings.Count == 0) {
				GameWin = true;
			}
			else {
				GameWin = false;
			}

			GameEnd = true; 
			StartCoroutine(GameEndShow(1.0f));
		}

		void AddRewardItem(int ID, double Value) {
			GameObject go = (GameObject)Instantiate(prefUIRewardItem, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (trRootRewardList);
			go.transform.localScale = Vector3.one;

			go.transform.Find ("Icon").GetComponent<Image>().sprite = TBDatabase.GetPayDefIcon(ID);
			go.transform.Find ("Text").GetComponent<Text>().text = Value.ToString ("#,##0");
		}

		void AddButtonUnit(int ID, int Count) {
			GameObject go = (GameObject)Instantiate(prefUIBattleUnit, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (trRootButtonUnit);
			go.transform.localScale = Vector3.one;
			
			int tmpInt = Units.Count;
			go.transform.Find ("Background").GetComponent<Button>().onClick.AddListener(() => { OnButtonUnit(tmpInt); });
			
			BattleUnit newItem = new BattleUnit(ID, go, Count);
			Units.Add (newItem);
			Buttons.Add (newItem);
			go.GetComponent<RectTransform>().anchoredPosition = new Vector2(114*(Units.Count-1)+53, 53) + ((Units.Count > 4) ? new Vector2(-114*4,114) : Vector2.zero);
		}
		
		void AddButtonItem(int ID, bool UseCooltime) {
			GameObject go = (GameObject)Instantiate(prefUIBattleItem, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (trRootButtonItem);
			go.transform.localScale = Vector3.one;
			
			int tmpInt = Items.Count;
			go.transform.Find ("Background").GetComponent<Button>().onClick.AddListener(() => { OnButtonItem(tmpInt); });
			
			BattleItem newItem = new BattleItem(ID, go, UseCooltime);
			Items.Add (newItem);
			Buttons.Add (newItem);
			go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-114*(Items.Count-1)-53, +53) + ((Items.Count > 4) ? new Vector2(114*4,114) : Vector2.zero);
		}
		
		public void EnemyAdd() {

			if(UnitSelected == -1) return;

			BattleUnit bu = Units[UnitSelected];
			if(bu.Commit()) {

				Vector3 vPosCreate = vPosClicked + new Vector3(UnityEngine.Random.Range (-0.1f,0.1f),0,UnityEngine.Random.Range (-0.1f,0.1f));
				BEUnit newUnit = BEUnitManager.instance.Add(UnitSelected, bu.ID, 1, vPosCreate, Quaternion.identity);
				newUnit.SetState(UnitState.Chase, null, Vector3.zero);

				if(!InGame)
					GameStart();
			}

			if(bu.Count <= 0) {
				UnitSelected = -1;
				bu.SelectToggle();
				BBSelected = null;
			}
		}

		public void MissleFire(Vector3 _vTarget, GameObject prefab) {
			Vector3 vStart = _vTarget + new Vector3(1,1,-1) * 32.0f;
			Vector3 vDir = (_vTarget - vStart);
			vDir.Normalize();
			Quaternion qRot = Quaternion.LookRotation(vDir);
			GameObject go = BEObjectPool.Spawn (prefab, vStart, qRot);
			BEBullet bullet = (BEBullet)go.GetComponent("BEBullet");
			if(bullet != null) {
				// set enemy tag to bullet
				bullet.EnemyTag = "Building";
				bullet.Init (null);
			}
		}

		public IEnumerator SmallMissleFire(Vector3 _vTarget, float RandomRange, int Count, float fDelay) {
			if(fDelay > 0.0001f) yield return new WaitForSeconds(fDelay);

			while(Count > 0) { 
				Vector3 vTarget = _vTarget + new Vector3(UnityEngine.Random.Range(-RandomRange,RandomRange),
				                                         0,
				                                         UnityEngine.Random.Range(-RandomRange,RandomRange));
				MissleFire(vTarget, prefSmallMissle);
				Count--;

				yield return new WaitForSeconds(0.2f); 
			}
		}

		//GUI Input
		public void OnButtonRetreat() {
			BEAudioManager.SoundPlay(6);

			if(InGame) {
				if(ButtonRetreatState == 0) {
					textButtonRetreat.text = "Confirm?";
					ButtonRetreatState = 1;
				}
				else {
					GameEndProcess();
				}
			}
			else {
				//Application.LoadLevel("Town");
				SceneManager.LoadScene("Town");
			}
		}
		
		public void OnButtonUnit(int id) {

			//Debug.Log ("OnButtonUnit:"+id);
			BEAudioManager.SoundPlay(6);

			if(ItemSelected != -1)
				ItemSelected = -1;
			if(BBSelected != null) 
				BBSelected.SelectToggle();

			//Debug.Log ("OnButtonUnit id:"+id+" Count:"+Units[id].Count);
			if((id != -1) && (Units[id].Count > 0)) {
				UnitSelected = id;

				BBSelected = Units[UnitSelected];
				BBSelected.SelectToggle();
			}
		}
		
		public void OnButtonItem(int id) {
			//Debug.Log ("OnButtonItem:"+id);
			BEAudioManager.SoundPlay(6);

			if(UnitSelected != -1)
				UnitSelected = -1;

			if(BBSelected != null) 
				BBSelected.SelectToggle();
			
			ItemSelected = id;
			
			if(ItemSelected != -1) {
				BBSelected = Items[ItemSelected];
				BBSelected.SelectToggle();
			}
		}
		
		//BECameraRTSListner implement
		public void OnTouchDown(Ray ray) { 
			float enter;
			BEGround.instance.xzPlane.Raycast(ray, out enter);
			vPosClicked = ray.GetPoint(enter);

		}
		public void OnTouchUp(Ray ray) {
			if(CommitEnabled) {
				CommitEnabled = false;
				BECameraRTS.instance.camPanningUse = true;
				BECameraRTS.instance.InertiaUse = true;
			}
		}
		public void OnTouch(Ray ray) {

			float enter;
			BEGround.instance.xzPlane.Raycast(ray, out enter);
			vPosClicked = ray.GetPoint(enter);

			if((UnitSelected != -1) && !CommitEnabled) {
				EnemyAdd();
			}
			if(ItemSelected != -1) {
				if(Items[ItemSelected].Use()) {

					if(ItemSelected == 0) {
						// Big Missle Fire
						MissleFire(vPosClicked, prefMissle);
					}
					else if(ItemSelected == 1) {
						// Small Missle Fire
						StartCoroutine(SmallMissleFire(vPosClicked, 3.0f, 12, 0.0f));
						GameStart();
					}
					else {}
				}
			}
		}
		public void OnDragStart(Ray ray) {}
		public void OnDrag(Ray ray) {}
		public void OnDragEnd(Ray ray) {}
		public void OnLongPress(Ray ray) {
			
			if((UnitSelected != -1)&& !CommitEnabled) {
				CommitEnabled = true;
				CommitAge = CommitPeriod;
				BECameraRTS.instance.camPanningUse = false;
				BECameraRTS.instance.InertiaUse = false;
			}
		}
		public void OnMouseWheel(float value) {}

		//SceneBase functions
		// add exp
		public override void GainExp(int exp) {}
		public override void Save() {}
		public override void Load() {

			TextAsset textAsset = (TextAsset) Resources.Load("Maps/"+configFilename);  
			//TextAsset textAsset = Resources.Load("Maps/"+configFilename, typeof(TextAsset)) as TextAsset;

			//Debug.Log ("SceneTown::Load");
			//string xmlFilePath = strMapsPath+"/"+configFilename+".dat";//BEUtil.pathForDocumentsFile(configFilename);
			//if(!File.Exists(xmlFilePath))
			//	return;

			InLoading = true;
			XmlDocument xmlDocument = new XmlDocument();
			//xmlDocument.Load(xmlFilePath);
			xmlDocument.LoadXml ( textAsset.text );

			// ####### Encrypt the XML ####### 
			// If the Xml is encrypted, so this piece of code decrypt it.
			if (xmlDocument.DocumentElement.ChildNodes.Count <= 1) {
				if(UseEncryption) {
					string data = BEUtil.Decrypt(xmlDocument.DocumentElement.InnerText);
					xmlDocument.DocumentElement.InnerXml = data;
				}
			}
			//################################
			
			
			if(xmlDocument != null) {
				XmlElement element = xmlDocument.DocumentElement;
				XmlNodeList list = element.ChildNodes;
				foreach(XmlElement ele in list) {
					if(ele.Name == "ConfigVersion")			{	ConfigVersion 	= int.Parse(ele.GetAttribute("value"));	 		}
/*					else if(ele.Name == "Time")				{	
						DateTime dtNow = DateTime.Now;	
						DateTime dtSaved = DateTime.Parse(ele.GetAttribute("value"));	 
						//Debug.Log ("dtNow:"+dtNow.ToString());
						//Debug.Log ("dtSaved:"+dtSaved.ToString());
						TimeSpan timeDelta = dtNow.Subtract(dtSaved);	
						//Debug.Log ("TimeSpan:"+timeDelta.ToString());
						BETime.timeAfterLastRun = timeDelta.TotalSeconds;
					}
*/					//else if(ele.Name == "ExpTotal")			{	ExpTotal = int.Parse(ele.GetAttribute("value"));	 			}
					//else if(ele.Name == "Gem")				{	Gem.ChangeTo(double.Parse(ele.GetAttribute("value")));	 		}
					else if(ele.Name == "Gold")				{	Gold.ChangeTo(double.Parse(ele.GetAttribute("value")));	 		}
					else if(ele.Name == "Elixir")			{	Elixir.ChangeTo(double.Parse(ele.GetAttribute("value")));	 	}
					//else if(ele.Name == "Shield")			{	Shield.ChangeTo(double.Parse(ele.GetAttribute("value")));	 	}
					else if(ele.Name == "Building")			{	
						int Type = int.Parse(ele.GetAttribute("Type"));	 		
						int Level = int.Parse(ele.GetAttribute("Level"));	
						//Debug.Log ("Building Type:"+Type.ToString()+" Level:"+Level.ToString());
						Building script = BEGround.instance.BuildingAdd(Type, Level);
						script.Load (ele);

						if(script.bt.Category == TargetType.Deco) {
							script.goCenter.GetComponent<BoxCollider>().enabled = false;
						}

						if((script.bt.Category != TargetType.Deco) && (script.bt.Category != TargetType.Wall))
							Buildings.Add (script);

						//Set Town Level text
						if(Type == 0) {
							textEnemyLevel.text = Level.ToString ();
						}
					}
					else {}
				}
			}
			
			InLoading = false;

			//set resource's capacity
//			BEGround.instance.CapacityCheck();
			AddRewardItem((int)PayType.Gold, Gold.Target()); 
			AddRewardItem((int)PayType.Elixir, Elixir.Target()); 
		}


		public void UpdateTownData(double PlunderGoldAdd, double PlunderElixirAdd) {

			string 	TownDataFilename = "Config.xml";
			string xmlFilePath = BEUtil.pathForDocumentsFile(TownDataFilename);
			if(!File.Exists(xmlFilePath))
				return;
			
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xmlFilePath);
			
			// ####### Encrypt the XML ####### 
			// If the Xml is encrypted, so this piece of code decrypt it.
			if (xmlDocument.DocumentElement.ChildNodes.Count <= 1) {
				if(UseEncryption) {
					string data = BEUtil.Decrypt(xmlDocument.DocumentElement.InnerText);
					xmlDocument.DocumentElement.InnerXml = data;
				}
			}
			//################################


			int [] RemoveUnitCount = new int[TBDatabase.GetArmyTypeCount()];
			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				RemoveUnitCount[i] = Casualties[i];
			}

			
			if(xmlDocument != null) {
				XmlElement element = xmlDocument.DocumentElement;
				XmlNodeList list = element.ChildNodes;
				foreach(XmlElement ele in list) {
					if(ele.Name == "Gold") {	
						double dValue = double.Parse(ele.GetAttribute("value")) + PlunderGoldAdd;	
						ele.SetAttribute("value", dValue.ToString());
					}
					else if(ele.Name == "Elixir") {	
						double dValue = double.Parse(ele.GetAttribute("value")) + PlunderElixirAdd;	
						ele.SetAttribute("value", dValue.ToString());
					}
					else if(ele.Name == "Building")	{	
						int Type = int.Parse(ele.GetAttribute("Type"));	
						if(Type == 8) {
							XmlNodeList list2 = ele.ChildNodes;
							foreach(XmlElement ele2 in list2) {
								if(ele2.Name == "Unit") {	
									string strUnits = ele2.GetAttribute("value");
									string [] strUnitsSub = strUnits.Split(',');
									int [] UnitsSub = new int[strUnitsSub.Length];
									for(int i=0 ; i < strUnitsSub.Length ; ++i) {
										UnitsSub[i] = int.Parse (strUnitsSub[i]);

										if(RemoveUnitCount[i] > 0) {
											if(UnitsSub[i] >= RemoveUnitCount[i]) {
												UnitsSub[i] -= RemoveUnitCount[i];
												RemoveUnitCount[i] = 0;
											}
											else {
												RemoveUnitCount[i] = RemoveUnitCount[i] - UnitsSub[i];
												UnitsSub[i] = 0;
											}
										}
									}

									strUnits = "";
									for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
										strUnits += UnitsSub[i].ToString ();
										
										if(i != TBDatabase.GetArmyTypeCount()-1)
											strUnits += ",";
									}

									ele2.SetAttribute("value", strUnits);
									//Debug.Log ("Unit "+strUnits);
								}
							}
						}
					}
					else {}
				}
			}

			// ####### Encrypt the XML ####### 
			// If you want to view the original xml file, turn of this piece of code and press play.
			if (xmlDocument.DocumentElement.ChildNodes.Count >= 1) {
				if(UseEncryption) {
					string data = BEUtil.Encrypt (xmlDocument.DocumentElement.InnerXml);
					xmlDocument.DocumentElement.RemoveAll ();
					xmlDocument.DocumentElement.InnerText = data;
				}
				xmlDocument.Save (xmlFilePath);
			}
			// ###############################
		}

		public void LoadArmy() {
			
			string 	TownDataFilename = "Config.xml";
			string xmlFilePath = BEUtil.pathForDocumentsFile(TownDataFilename);
			if(!File.Exists(xmlFilePath))
				return;
			
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xmlFilePath);
			
			// ####### Encrypt the XML ####### 
			// If the Xml is encrypted, so this piece of code decrypt it.
			if (xmlDocument.DocumentElement.ChildNodes.Count <= 1) {
				if(UseEncryption) {
					string data = BEUtil.Decrypt(xmlDocument.DocumentElement.InnerText);
					xmlDocument.DocumentElement.InnerXml = data;
				}
			}
			//################################
			
			
			int [] UnitCount = new int[TBDatabase.GetArmyTypeCount()];
			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				UnitCount[i] = 0;
			}
			
			
			if(xmlDocument != null) {
				XmlElement element = xmlDocument.DocumentElement;
				XmlNodeList list = element.ChildNodes;
				foreach(XmlElement ele in list) {
					if(ele.Name == "Building")	{	
						int Type = int.Parse(ele.GetAttribute("Type"));	
						if(Type == 8) {
							XmlNodeList list2 = ele.ChildNodes;
							foreach(XmlElement ele2 in list2) {
								if(ele2.Name == "Unit") {	
									string strUnits = ele2.GetAttribute("value");
									string [] strUnitsSub = strUnits.Split(',');
									for(int i=0 ; i < strUnitsSub.Length ; ++i) {
										UnitCount[i] += int.Parse (strUnitsSub[i]);
									}
								}
							}
						}
					}
					else {}
				}
			}


			// Add Army
			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				AddButtonUnit(i,UnitCount[i]);
			}
		}

		public void BuildingDestroyed(Building script) {

			int idx = Buildings.FindIndex(x => x==script);
			if(idx != -1) {

				// if Townhall is destroyed
				if(script.bt.ID == 0) {
				}

				Buildings.RemoveAt(idx);

				if(InGame && !GameEnd && (Buildings.Count == 0)) {
					GameEndProcess();
				}
			}
		}
	
		public IEnumerator GameEndShow(float fDelay) {
			textInfo.text = "";
			textTime.text = "";
			goButtonRetreat.SetActive(false);

			if(fDelay > 0.0001f) yield return new WaitForSeconds(fDelay);

			//Calc casualties count
			Casualties = new int[TBDatabase.GetArmyTypeCount()];
			for(int i=0 ; i < TBDatabase.GetArmyTypeCount() ; ++i) {
				int UnitLiveCount = BEUnitManager.instance.UnitGroups[i].Count;
				Casualties[i] = Units[i].CountMax - Units[i].Count - UnitLiveCount;
			}

			if(GameWin) {
				UpdateTownData(PlunderGold.Target(), PlunderElixir.Target());
			}
			else {
				UpdateTownData(0, 0);
			}

			UIDialogBattleResult.Show(GameWin);
		}
	}

}
