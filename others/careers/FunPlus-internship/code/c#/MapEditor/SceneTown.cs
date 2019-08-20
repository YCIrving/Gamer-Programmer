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
///   Class:          SceneTown
///   Description:    main class of town 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class SceneTown : SceneBase, BECameraRTSListner {

		public	static SceneTown instance;

		public  Text			textLevel;
		private Text			HouseInfo = null;
		private Building		MouseClickedBuilding = null;
		public 	BEGround 		ground=null;

		public  static 	BENumber	Exp;
		public  static 	BENumber	Gold;
		public  static 	BENumber	Elixir;
		public  static 	BENumber	Gem;
		public  static 	BENumber	Shield;
		private static 	int 		Level = 0;
		private static 	int 		ExpTotal = 0;

		private bool	LongPressed = false;


		void Awake () {
			instance=this;
			Init(SceneType.Town);

			// initialize BENumber class and set ui element 
			Exp = new BENumber(BENumber.IncType.VALUE, 0, 100000000, 0);
			Exp.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelExp/Fill").GetComponent<Image>());

			Gold = new BENumber(BENumber.IncType.VALUE, 0, 200000, 10000); // initial gold count is 1000
			Gold.AddUIText(BEUtil.GetObject("PanelOverlay/LabelGold/Text").GetComponent<Text>());
			Gold.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelGold/Fill").GetComponent<Image>());

			Elixir = new BENumber(BENumber.IncType.VALUE, 0, 300000, 10000); // initial elixir count is 1000	
			Elixir.AddUIText(BEUtil.GetObject("PanelOverlay/LabelElixir/Text").GetComponent<Text>());
			Elixir.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelElixir/Fill").GetComponent<Image>());

			Gem = new BENumber(BENumber.IncType.VALUE, 0, 100000000, 1000);	// initial gem count is 100	0	
			Gem.AddUIText(BEUtil.GetObject("PanelOverlay/LabelGem/Text").GetComponent<Text>());

			HouseInfo = BEUtil.GetObject("PanelOverlay/LabelHouse/Text").GetComponent<Text>();

			Shield = new BENumber(BENumber.IncType.TIME, 0, 100000000, 0);			
			Shield.AddUIText(BEUtil.GetObject("PanelOverlay/LabelShield/Text").GetComponent<Text>());

			//Set BENumber to TBDatabase's PayDef
			TBDatabase.GetPayDef((int)PayType.Gold).Number = Gold;
			TBDatabase.GetPayDef((int)PayType.Elixir).Number = Elixir;
			TBDatabase.GetPayDef((int)PayType.Gem).Number = Gem;
		}

		void Start () {

			Time.timeScale = 1;
			BECameraRTS.instance.Listner = this;

			// load game data from xml file
			Load ();

			// level & exp ui update
			UpdateExp();

			// create workers by hut count
			// 工人小屋ID固定为1
			int HutCount = BEGround.instance.GetBuildingCount(1);
			BEWorkerManager.instance.CreateWorker(HutCount);
			BEGround.instance.SetWorkingBuildingWorker();

			// initial Cam position & zoom setting
			BECameraRTS.instance.SetCameraPosition(Vector3.zero);
			BECameraRTS.instance.SetCameraZoom(50);

			//UpdateCount++;
		}

		public override void Update () {

			//Debug.Log ("SceneTown::Update start UpdateCount:"+UpdateCount);
			base.Update();

			// get delta time from BETime
			float deltaTime = BETime.deltaTime;

			// to avoid initial update delay,
			// call this cam animation  in 2nd update frame
			UpdateCount++;
			if(UpdateCount == 2) {
				float zoomTarget = 30.0f;
				BETween bt = BETween.position(BECameraRTS.instance.trCamera.gameObject, 1.0f, new Vector3(0,0,-zoomTarget));
				bt.method = BETweenMethod.easeOut;
				bt.onFinish = () => { BECameraRTS.instance.SetCameraZoom(zoomTarget); };
			}

			// if user pressed escape key, show quit messagebox
			if (!UIDialogMessage.IsShow() && !UIDialogMessage.isModalShow && Input.GetKeyDown(KeyCode.Escape)) { 
				UIDialogMessage.Show("Do you want to quit this program?", "Yes,No", "Quit?", null, (result) => { MessageBoxResult(result); } );
			}

			Exp.Update();
			Gold.Update();
			Elixir.Update();
			Gem.Update();
			Shield.ChangeTo (Shield.Target() - (double)deltaTime);
			Shield.Update();
			HouseInfo.text = BEWorkerManager.instance.GetAvailableWorkerCount().ToString () +"/"+BEGround.instance.GetBuildingCount(1).ToString ();
			//Debug.Log ("SceneTown::Update end UpdateCount:"+UpdateCount);
		}

		//pause
		public void OnButtonAttack() {
			BEAudioManager.SoundPlay(6);

			// Set Target Mpa name
			DataOverScene.BaseName = "Dolphin";
			//Application.LoadLevel("Battle");
			SceneManager.LoadScene("Battle");
		}
		
		// user clicked shop button
		public void OnButtonShop() {
			BEAudioManager.SoundPlay(6);
			UIShop.Show(ShopType.Normal);
		}
		
		// user clicked gem button
		public void OnButtonGemShop() {
			BEAudioManager.SoundPlay(6);
			UIShop.Show(ShopType.InApp);
		}
		
		// user clicked house button
		public void OnButtonHouse() {
			BEAudioManager.SoundPlay(6);
			UIShop.Show(ShopType.House);
		}
		
		// user clicked option button
		public void OnButtonOption() {
			BEAudioManager.SoundPlay(6);
			UIOption.Show();
		}
		
		//BECameraRTSListner implement
		public void OnTouchDown(Ray ray) {
			LongPressed = false;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "Building")) {
				MouseClickedBuilding = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
				//Debug.Log ("OnTouchDown MouseClickedBuilding:"+MouseClickedBuilding);
			}
			else {
				MouseClickedBuilding = null;
			}
		}
		
		public void OnTouchUp(Ray ray) {

			//Debug.Log ("OnTouchUp MouseClickedBuilding:"+MouseClickedBuilding);
			//Debug.Log ("OnTouchUp BEGround.buildingSelected:"+BEGround.buildingSelected);
		}

		public void OnTouch(Ray ray) {
			//Debug.Log ("OnTouch MouseClickedBuilding:"+MouseClickedBuilding);
			//Debug.Log ("OnTouch BEGround.buildingSelected:"+BEGround.buildingSelected);
			RaycastHit hit;
			if(BEGround.buildingSelected == null) {
				
				if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "Building")) {
					Building buildingNew = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
					
					if(!buildingNew.HasCompletedWork() && !LongPressed)
						BEGround.instance.BuildingSelect(buildingNew);
				}
			}
			else {
				
				if((MouseClickedBuilding != BEGround.buildingSelected) && BEGround.buildingSelected.OnceLanded)
					BEGround.instance.BuildingLandUnselect();
				
				if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "Building")) {
					Building buildingNew = BEGround.instance.BuildingFromObject(hit.collider.gameObject);

					if(!buildingNew.HasCompletedWork())
						BEGround.instance.BuildingSelect(buildingNew);
				}
			}
		}

		public void OnDragStart(Ray ray) {
			
			if((BEGround.buildingSelected != null) && (MouseClickedBuilding == BEGround.buildingSelected)) {
				BECameraRTS.instance.camPanningUse = false;
				BECameraRTS.instance.InertiaUse = false;
				
				//Debug.Log ("OnDragStart MouseClickedBuilding:"+MouseClickedBuilding);
				//Debug.Log ("OnDragStart BEGround.buildingSelected:"+BEGround.buildingSelected);
				if(MouseClickedBuilding == BEGround.buildingSelected) {
					BETween.alpha(ground.gameObject, 0.1f, 0.0f, 0.3f);
				}
			}
		}
		
		public void OnDrag(Ray ray) {
			
			if((BEGround.buildingSelected != null) && (MouseClickedBuilding == BEGround.buildingSelected) && (BEGround.buildingSelected.bt.Category != TargetType.Deco )) {
				//BECameraRTS.instance.camPanningUse = false;
				//BECameraRTS.instance.InertiaUse = false;
				
				float enter=0.0f;
				BEGround.instance.xzPlane.Raycast(ray, out enter);
				BEGround.buildingSelected.Move (ray.GetPoint(enter));
			}
		}
		
		public void OnDragEnd(Ray ray) {
			
			if(BEGround.buildingSelected != null) {
				BECameraRTS.instance.camPanningUse = true;
				BECameraRTS.instance.InertiaUse = true;
				
				//Debug.Log ("OnDragEnd MouseClickedBuilding:"+MouseClickedBuilding);
				//Debug.Log ("OnDragEnd BEGround.buildingSelected:"+BEGround.buildingSelected);
				if((MouseClickedBuilding != null) && (MouseClickedBuilding == BEGround.buildingSelected)) {
					BETween.alpha(ground.gameObject, 0.1f, 0.3f, 0.0f);
				
					if(BEGround.buildingSelected.Landable && BEGround.buildingSelected.OnceLanded)
						BEGround.instance.BuildingLand();//BuildingLandUnselect();
				}
			}
			
			MouseClickedBuilding = null;
		}
		
		public void OnLongPress(Ray ray) {
			LongPressed = true;
			//Debug.Log ("OnLongPress MouseClickedBuilding:"+MouseClickedBuilding);
			RaycastHit hit;
			if(BEGround.buildingSelected == null) {
				if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "Building")) {
					Building buildingNew = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
					if(!buildingNew.HasCompletedWork())
						BEGround.instance.BuildingSelect(buildingNew);
				}
			}
		}

		public void OnMouseWheel(float value) {}


		public void UpdateExp() {
			int NewLevel = TBDatabase.GetLevel(ExpTotal);
			int LevelExpToGet = TBDatabase.GetLevelExp(NewLevel);
			int LevelExpStart = TBDatabase.GetLevelExpTotal(NewLevel);
			
			Exp.MaxSet(LevelExpToGet);
			int ExpLeft = ExpTotal - LevelExpStart;
			Exp.ChangeTo(ExpLeft);
			
			// if level up occured
			if((NewLevel > Level) && (Level != 0)) {
				// show levelup notify here
			}
			Level = NewLevel;
			textLevel.text = NewLevel.ToString ();
		}

		//SceneBase functions
		// add exp
		public override void GainExp(int exp) {
			//Debug.Log ("SceneTown::GainExp"+exp);

			ExpTotal += exp;
			UpdateExp();
			
			// save game data
			Save ();
		}
		
		// Do not save town when script quit.
		// save when action is occured
		// (for example, when building created, when start upgrade, when colled product, when training start)
		public override void Save() {

			if(InLoading) return;
			//Debug.Log ("SceneTown::Save");

			string xmlFilePath = BEUtil.pathForDocumentsFile(configFilename);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<StageName><name>wrench</name></StageName>");
			{
				xmlDocument.DocumentElement.RemoveAll();
				Transform trDecoRoot = BEGround.instance.trDecoRoot;
				//List<GameObject> goTiles=new List<GameObject>();
				foreach(Transform child in trDecoRoot) {
					Building script = child.gameObject.GetComponent<Building>();
					if(script != null) {
						script.Save (xmlDocument);
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
		}

		public override void Load() {

			//Debug.Log ("SceneTown::Load");
			string xmlFilePath = BEUtil.pathForDocumentsFile(configFilename);
			if(!File.Exists(xmlFilePath)) {

				//if user new to this game add initial building
				BEGround.instance.BuildingAdd(0,1,Vector3.zero);		// add town hall at map center
				BEGround.instance.BuildingAdd(1,1,new Vector3(4,0,0));	// add hut 

				Save();
				bFirstRun = true;

				//set resource's capacity
//				BEGround.instance.CapacityCheck();
				return;
			}
			else {
				bFirstRun = false;
			}

			InLoading = true;
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

			if(xmlDocument != null) {
				XmlElement element = xmlDocument.DocumentElement;
				XmlNodeList list = element.ChildNodes;
				foreach(XmlElement ele in list) {
				if(ele.Name == "Building")			{	
						int Type = int.Parse(ele.GetAttribute("Type"));	 		
						int Level = int.Parse(ele.GetAttribute("Level"));	
						//Debug.Log ("Building Type:"+Type.ToString()+" Level:"+Level.ToString());
						Building script = BEGround.instance.BuildingAdd(Type, Level);
						script.Load (ele);
					}
					else {}
				}
			}

			InLoading = false;
			
			//set resource's capacity
//			BEGround.instance.CapacityCheck();
		}
	}

}
