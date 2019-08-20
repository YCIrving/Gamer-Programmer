using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogUpgradeAsk
///   Description:    class for building upgrade ask dialog
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	// what kind of building info
	public enum BDInfo {
		None				= -1,
		CapacityGold		= 0, 	
		CapacityElixir		= 1, 	
		Capacity			= 2, 	
		ProductionRate		= 3,	
		HitPoint			= 4,
		StorageCapacity		= 5,
		TroopCapacity		= 6,
		TrainingCapacity	= 7,
		DamagePerSecond		= 8,
	}

	// use this class to show progress value
	[System.Serializable]
	public class ProgressInfo {
		public	GameObject 	gameObject;
		public	Image 		imageIcon;
		public	Text 		textInfo;
		public	Image 		imageMiddle;
		public	Image 		imageFront;
	}

	public class UIDialogUpgradeAsk : UIDialogBase {

		private static UIDialogUpgradeAsk instance;

		private Building 		building = null;
		private BuildingType 	bt = null;
		//private BuildingDef 	bd = null;
		private BuildingDef 	bdNext = null;
		//private BuildingDef 	bdLast = null;
		private bool 			Available = true;
		private bool 			TownLevelOk = true;

		public	Text 			textTitle;
		//public	Text 			textLevel;
		public	Image 			imgIcon;
		public	ProgressInfo []	progresses;
		public	GameObject 		goNote;
		public	Text 			NoteInfo;
		public	GameObject 		goNormal;
		public	Text 			textBuildTime;
		public	Image 			PriceIcon;
		public	Text 			Price;
		public	Text 			PriceGem;

		private int				GemCount = 0;


		void Awake () {
			instance=this;
		}
		
		void Start () {
			gameObject.SetActive(false);
		}
		
		public override void Update () {

			base.Update();
			
			if(bdNext != null)
				Available = bdNext.PriceInfoCheck(Price);
		}

		void Reset () {
			//bd = null;

			bt = TBDatabase.GetBuildingType(building.Type);
			//bd = bt.GetDefine(building.Level);
			bdNext = bt.GetDefine(building.Level+1);
			//bdLast = bt.GetDefLast();
			
			textTitle.text = "Upgrade "+bt.Name+" to Level "+(building.Level+1).ToString ()+" ?";
			imgIcon.sprite = bt.Icon;

			for(int i=0 ; i < progresses.Length ; ++i) 
				progresses[i].gameObject.SetActive(true);
			
			// display progresses of building by building type
			
			if(bt.ID == 1) {
				// incase building si town hall, show gold capacity, elixir capacit and hitpoint
				building.UIFillProgressWithNext(progresses[0], BDInfo.CapacityGold);
				building.UIFillProgressWithNext(progresses[1], BDInfo.CapacityElixir);
				building.UIFillProgressWithNext(progresses[2], BDInfo.HitPoint);
			}
			else if(bt.ID == 2) {
				// incase building is house, only show hitpoint, and disable other progresses
				building.UIFillProgressWithNext(progresses[0], BDInfo.HitPoint);
				progresses[1].gameObject.SetActive(false);
				progresses[2].gameObject.SetActive(false);
			}
			else if(bt.ID == 3) {
				building.UIFillProgressWithNext(progresses[0], BDInfo.HitPoint);
				progresses[1].gameObject.SetActive(false);
				progresses[2].gameObject.SetActive(false);
			}
			else if((bt.ID == 4) || (bt.ID == 5)) {
				building.UIFillProgressWithNext(progresses[0], BDInfo.Capacity);
				building.UIFillProgressWithNext(progresses[1], BDInfo.ProductionRate);
				building.UIFillProgressWithNext(progresses[2], BDInfo.HitPoint);
			}
			else if((bt.ID == 6) || (bt.ID == 7)) {
				building.UIFillProgressWithNext(progresses[0], BDInfo.StorageCapacity);
				building.UIFillProgressWithNext(progresses[1], BDInfo.HitPoint);
				progresses[2].gameObject.SetActive(false);
			}
			else if(bt.ID == 8) {
				//"Training Capacity : 0/20";
				//"HitPoint"
			}
			else if(bt.ID == 9) {
				//"Total troop Capacity : 0/20";
				//"HitPoint"
				// show troops icon (click to remove unit) "Remove Troops?"
			}
			else if(bt.ID == 10) {
				//"Damage per second:15";
				//"HitPoint"
				//Range : 9 Tiles
				//Damage Type: Single Target
				//Targets: Ground
				//Favorite target: Any
			}
			else if(bt.ID == 11) {
			}
			else {}
			
//			if(bt.TB == 0) {
//				// incase building si town hall, show gold capacity, elixir capacit and hitpoint
//				building.UIFillProgressWithNext(progresses[0], BDInfo.CapacityGold);
//				building.UIFillProgressWithNext(progresses[1], BDInfo.CapacityElixir);
//				building.UIFillProgressWithNext(progresses[2], BDInfo.HitPoint);
//			}
//			else if(bt.TB == 1) {
//				// incase building is house, only show hitpoint, and disable other progresses
//				building.UIFillProgressWithNext(progresses[0], BDInfo.HitPoint);
//				progresses[1].gameObject.SetActive(false);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if(bt.TB == 2) {
//				building.UIFillProgressWithNext(progresses[0], BDInfo.HitPoint);
//				progresses[1].gameObject.SetActive(false);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if((bt.TB == 3) || (bt.TB == 4)) {
//				building.UIFillProgressWithNext(progresses[0], BDInfo.Capacity);
//				building.UIFillProgressWithNext(progresses[1], BDInfo.ProductionRate);
//				building.UIFillProgressWithNext(progresses[2], BDInfo.HitPoint);
//			}
//			else if((bt.TB == 5) || (bt.TB == 6)) {
//				building.UIFillProgressWithNext(progresses[0], BDInfo.StorageCapacity);
//				building.UIFillProgressWithNext(progresses[1], BDInfo.HitPoint);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if(bt.TB == 7) {
//				//"Training Capacity : 0/20";
//				//"HitPoint"
//			}
//			else if(bt.TB == 8) {
//				//"Total troop Capacity : 0/20";
//				//"HitPoint"
//				// show troops icon (click to remove unit) "Remove Troops?"
//			}
//			else if(bt.TB == 9) {
//				//"Damage per second:15";
//				//"HitPoint"
//				//Range : 9 Tiles
//				//Damage Type: Single Target
//				//Targets: Ground
//				//Favorite target: Any
//			}
//			else if(bt.TB == 10) {
//			}
//			else {}

			// get townhall to check upgrade requires next townhall
			Building buildingTown = BEGround.instance.Buildings[0][0];
			if(bdNext.TownHallLevelRequired > buildingTown.Level) {
				goNote.SetActive(true);
				NoteInfo.text = "To upgrade this building, you first need\n Town Hall Level "+bdNext.TownHallLevelRequired.ToString ()+"!";
				TownLevelOk = false;
				goNormal.SetActive(false);
			}
			else {
				goNote.SetActive(false);
				TownLevelOk = true;
				goNormal.SetActive(true);

				// set infos about upgrade
				bdNext.PriceInfoApply(PriceIcon, Price);
				textBuildTime.text = BENumber.SecToString(bdNext.BuildTime);
				GemCount = (bdNext.BuildTime + 1)/60; // 1 gem per minute
				PriceGem.text = GemCount.ToString ("#,##0");
				PriceGem.color = (SceneTown.Gem.Target() < GemCount) ? Color.red : Color.white;
			}
		}

		// when user clicked upgrade button
		public void OnButtonOk() {

			// if upgrade is available, then upgrade
			if(Available && TownLevelOk) {
				if(building.Upgrade()) {
					// set this building to worker
					BEWorkerManager.instance.SetWorker(building);
					BEGround.instance.BuildingSelect(null);
				}
				
				_Hide();
			}
			else {
				UIDialogMessage.Show("More Resource Required", "Ok", "Error");
			}
		}
		
		public void OnButtonInstant() {
			if(!TownLevelOk) {
				// upgrade not available
				return;
			}

			// checkuser has enough gem count
			if(SceneTown.Gem.Target() < GemCount) {
				// you need more gem
				UIDialogMessage.Show("More Gem Required", "Ok", "Error", TBDatabase.GetPayDefIcon((int)PayType.Gem));

				return;
			}

			// decrease gem count and upgrade immediately
			SceneTown.Gem.ChangeDelta(-GemCount);
			building.UpgradeEnd();
			BEGround.instance.BuildingSelect(null);

			_Hide();
		}
		
		public void _Show(Building script) {
			
			building = script;
			
			Reset();
			ShowProcess();
		}
		
		public static void Show(Building script) 	{ instance._Show(script); }
		public static void Hide() 					{ instance._Hide(); }
	}
}