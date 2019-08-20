using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogTraining
///   Description:    class for unit training
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class UIDialogTraining : UIDialogBase {
		
		public static UIDialogTraining instance;

		private Building 		building = null;
		private BuildingType 	bt = null;
		private BuildingDef 	bd = null;

		public	Text 				TrainingInfo;
		public	Text 				CapacityInfo;
		public	Text 				TimeLeft;
		public	Text 				GemCount;
		public	GameObject 			goAllCampFull;
		public	GameObject 			prefabUnitItem;
		public	GameObject 			prefabUnitQueItem;
		public	RectTransform 		rtUnitQueList;
		public	RectTransform 		rtUnitList;
		public	RectTransform 		rtUnitInfo;
		public 	UIDialogUnitInfo	scriptUnitInfo;

		public 	List<UIUnitQueItem>	queItems = new List<UIUnitQueItem>();
		public 	bool 				TrainingEnabled = true;

		void Awake () {
			instance=this;
		}
		
		void Start () {
			gameObject.SetActive(false);
		}

		public override void Update () {

			base.Update();

			UpdateUI();
		}
		
		public void UpdateUI() {

			int UnitGenTimeLeftTotal = (int)building.UnitGenTimeLeftTotal();

			TrainingInfo.text = "Train Troops "+building.UnitGenHousingSpaceTotal+"/"+bd.TrainingQueueMax.ToString ();
			CapacityInfo.text = "Troop capacity after training: "+BEGround.instance.TroopHousingSpaceTotal.ToString("#,##0")+" / "+BEGround.instance.TroopCapacityTotal.ToString("#,##0");
			TimeLeft.text = BENumber.SecToString(UnitGenTimeLeftTotal);
			GemCount.text = TBDatabase.GetGemBySec(UnitGenTimeLeftTotal).ToString ("#,##0");

			TrainingEnabled = (building.UnitGenHousingSpaceTotal < bd.TrainingQueueMax) ? true : false;

			Building buildingArmyCamp = BEGround.instance.FindCampWithSpace(1);
			if(buildingArmyCamp == null) {
				if(!goAllCampFull.activeInHierarchy) {
					goAllCampFull.SetActive(true);
				}
			}
			else {
				if(goAllCampFull.activeInHierarchy) {
					goAllCampFull.SetActive(false);
				}
			}
		}

		public void UnitInfo(int unitID) {
			//Debug.Log ("UnitInfo "+unitID.ToString());
			UIDialogUnitInfo.Show(unitID);
		}
		
		public void UnitGenAdd(int unitID) {
			//Debug.Log ("UnitCreate "+unitID.ToString());
			GenQueItem item = building.UnitGenAdd(unitID, 1);
			if(item == null) return;

			// search unit que list with given unit id
			int idx = queItems.FindIndex(x => x.item.unitID==unitID);
			if(idx == -1) {
				GameObject go = (GameObject)Instantiate(prefabUnitQueItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (rtUnitQueList);
				go.transform.localScale = Vector3.one;
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(queItems.Count*-100, 0);

				UIUnitQueItem script = go.GetComponent<UIUnitQueItem>();
				script.Init(this, item);
				queItems.Add (script);
			}

			BEGround.scene.Save();
		}

		public void UnitGenRemove(GenQueItem item) {
			item.CountDelta(-1);
			BEGround.scene.Save();
		}

		public void UnitGenRemoveByBuilding(Building scriptBarrack, int unitID) {
			
			if(building != scriptBarrack) return;
			
			for(int i=0 ; i < queItems.Count ; ++i) {
				
				UIUnitQueItem uiItem = queItems[i];
				if(uiItem.item.unitID != unitID) continue;

				// reposition unit gen items
				for(int j=i+1 ; j < queItems.Count ; ++j) {
					RectTransform rt = queItems[j].gameObject.GetComponent<RectTransform>();
					rt.anchoredPosition = new Vector2((j-1)*-100, 0);
				}
				
				//Debug.Log ("queItems.RemoveAt "+i.ToString()+"unitID:"+uiItem.item.unitID.ToString());
				queItems.RemoveAt(i);
				Destroy (uiItem.gameObject);
				return;
			}
		}
		
		public void OnButtonInstant() {
			int UnitGenTimeLeftTotal = (int)building.UnitGenTimeLeftTotal();
			int GemCount = TBDatabase.GetGemBySec(UnitGenTimeLeftTotal);

			SceneTown.Gem.ChangeDelta(-GemCount);
			building.UnitGenUpdate(UnitGenTimeLeftTotal+10);
			_Hide();
		}
		
		public void OnButtonOk() {
			_Hide();
		}

		public void _Show(Building script) {
			
			building = script;
			
			queItems.Clear();
			bd = null;
			bt = TBDatabase.GetBuildingType(building.Type);
			bd = bt.GetDefine(building.Level);
			
			// delete old items of each content
			for(int j=rtUnitQueList.childCount-1;j>=0;j--){
				Destroy (rtUnitQueList.GetChild(j).gameObject);
			}
			for(int i=0 ; i < building.queUnitGen.Count ; ++i) {
				GenQueItem item = building.queUnitGen[i];
				
				GameObject go = (GameObject)Instantiate(prefabUnitQueItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (rtUnitQueList);
				go.transform.localScale = Vector3.one;
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(i*-100, 0);
				
				UIUnitQueItem newItem = go.GetComponent<UIUnitQueItem>();
				newItem.Init(this, item);
				queItems.Add (newItem);
				//Debug.Log ("queItems.Add "+queItems.Count.ToString()+"unitID:"+item.unitID.ToString());
			}
			
			// delete old items of each content
			for(int j=rtUnitList.childCount-1;j>=0;j--){
				Destroy (rtUnitList.GetChild(j).gameObject);
			}
			int sz = TBDatabase.GetArmyTypeCount();
			for(int i=0 ; i < sz ; ++i) {
				int col = i/2;
				int row = i%2;
				
				GameObject go = (GameObject)Instantiate(prefabUnitItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (rtUnitList);
				go.transform.localScale = Vector3.one;
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(col*160, row*-160);
				
				UIUnitItem newItem = go.GetComponent<UIUnitItem>();
				newItem.Init(this, i);
			}
			rtUnitList.sizeDelta = new Vector3(160*((sz+1)/2), 310);

			UIDialogUnitInfo.Show(-1);
			ShowProcess();
		}
		
		public static void Show(Building script) 	{ instance._Show(script); }
		public static void Hide() 					{ instance._Hide(); }
	}
}