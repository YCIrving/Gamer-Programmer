using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogInfo
///   Description:    class for show building info
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class UIDialogInfo : UIDialogBase {

		private static UIDialogInfo instance;

		private Building 		building = null;
		private BuildingType 	bt = null;
		//private BuildingDef 	bd = null;
		//private BuildingDef 	bdNext = null;
		//private BuildingDef 	bdLast = null;

		public	Text 			textTitle;
		public	Text 			textLevel;
		public	Image 			imgIcon;
		public	ProgressInfo []	progresses;
		public	Text 			textInfo;

		void Awake () {
			instance=this;
		}
		
		void Start () {
			gameObject.SetActive(false);
		}
		
		public override void Update () {
			
			base.Update();
			
		}
		
		void Reset () {
			//bd = null;
			bt = TBDatabase.GetBuildingType(building.Type);
			//bd = bt.GetDefine(building.Level);
			//bdNext = bt.GetDefine(building.Level+1);
			//bdLast = bt.GetDefLast();
			
			textTitle.text = bt.Name;
			textLevel.text = "Level "+building.Level.ToString ();
			imgIcon.sprite = bt.Icon;


			for(int i=0 ; i < progresses.Length ; ++i) 
				progresses[i].gameObject.SetActive(true);


			// display progresses of building by building type
			textInfo.text = bt.Desc;
			// 主城
			if(bt.ID == 1) {
				// incase building si town hall, show gold capacity, elixir capacit and hitpoint
				//textInfo.text = "This is the heart of your village. Upgrading your Town Hall unlocks new defenses, buildings, traps and much more.";
				building.UIFillProgress(progresses[0], BDInfo.CapacityGold);
				building.UIFillProgress(progresses[1], BDInfo.CapacityElixir);
				building.UIFillProgress(progresses[2], BDInfo.HitPoint);
			}
			// 工人小屋
			else if(bt.ID == 2) {
				// incase building is house, only show hitpoint, and disable other progresses
				//textInfo.text = "Nothing gets done around here without Builders! You can hire more Builders to start multiple construction projects, or speed up their work by using green gems.";
				building.UIFillProgress(progresses[0], BDInfo.HitPoint);
				progresses[1].gameObject.SetActive(false);
				progresses[2].gameObject.SetActive(false);
			}
			// 城墙
			else if(bt.ID == 3) {
				//textInfo.text = "Walls are great for keeping your village safe and your enemies in the line of fire.";
				building.UIFillProgress(progresses[0], BDInfo.HitPoint);
				progresses[1].gameObject.SetActive(false);
				progresses[2].gameObject.SetActive(false);
			}
			
			else if(bt.ID == 4) {
				//if(bt.ID == 5) 	textInfo.text = "All your precious gold is stored here. Don't let sneaky goblins anywhere near! Upgrade the storage to increase its capacity and durability against attack.";
				//else  			textInfo.text = "These storages contain the elixir pumped from underground. Upgrade them to increase the maximum amount of elixir you can store.";
				building.UIFillProgress(progresses[0], BDInfo.StorageCapacity);
				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
				progresses[2].gameObject.SetActive(false);
			}
			
			else if(bt.ID== 7) {
				//textInfo.text = "The Barracks allIDow you to train troops to attack your enemies. Upgrade the Barracks to unlock advanced units that can win epic battles.";
				//"Training Capacity : 0/20";
				//"HitPoint"
				building.UIFillProgress(progresses[0], BDInfo.TrainingCapacity);
				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
				progresses[2].gameObject.SetActive(false);
			}
			else if(bt.ID == 8) {
				//textInfo.text = "Your troops are stationed in Army Camps. Build more camps and upgrade them to muster a powerful army.";
				//"Total troop Capacity : 0/20";
				//"HitPoint"
				// show troops icon (click to remove unit) "Remove Troops?"
				building.UIFillProgress(progresses[0], BDInfo.TroopCapacity);
				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
				progresses[2].gameObject.SetActive(false);
			}
			else if((bt.ID == 9) || (bt.ID == 10)) {
				//textInfo.text = "Cannons are great for point defense. Upgrade cannons to increase their firepower, but beware that your defensive turrets cannot shoot while being upgraded!";
				//"Damage per second:15";
				//"HitPoint"
				//Range : 9 Tiles
				//Damage Type: Single Target
				//Targets: Ground
				//Favorite target: Any
			}
			else if((bt.ID == 11) || (bt.ID == 12) || (bt.ID == 13)) {
				building.UIFillProgress(progresses[0], BDInfo.DamagePerSecond);
				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
				progresses[2].gameObject.SetActive(false);
			}
			
//			if(bt.TB == 0) {
//				// incase building si town hall, show gold capacity, elixir capacit and hitpoint
//				//textInfo.text = "This is the heart of your village. Upgrading your Town Hall unlocks new defenses, buildings, traps and much more.";
//				building.UIFillProgress(progresses[0], BDInfo.CapacityGold);
//				building.UIFillProgress(progresses[1], BDInfo.CapacityElixir);
//				building.UIFillProgress(progresses[2], BDInfo.HitPoint);
//			}
//			else if(bt.TB == 1) {
//				// incase building is house, only show hitpoint, and disable other progresses
//				//textInfo.text = "Nothing gets done around here without Builders! You can hire more Builders to start multiple construction projects, or speed up their work by using green gems.";
//				building.UIFillProgress(progresses[0], BDInfo.HitPoint);
//				progresses[1].gameObject.SetActive(false);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if(bt.TB == 2) {
//				//textInfo.text = "Walls are great for keeping your village safe and your enemies in the line of fire.";
//				building.UIFillProgress(progresses[0], BDInfo.HitPoint);
//				progresses[1].gameObject.SetActive(false);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if((bt.TB == 3) || (bt.TB == 4)) {
//				//if(bt.ID == 3) 	textInfo.text = "The Gold Mine produces gold. Upgrade it to boost its production and gold storage capacity.";
//				//else 			textInfo.text = "Elixir is pumped from the Ley Lines coursing underneath your village. Upgrade your Elixir Collectors to maximize elixir production.";
//				building.UIFillProgress(progresses[0], BDInfo.Capacity);
//				building.UIFillProgress(progresses[1], BDInfo.ProductionRate);
//				building.UIFillProgress(progresses[2], BDInfo.HitPoint);
//			}
//			else if((bt.TB == 5) || (bt.TB == 6)) {
//				//if(bt.ID == 5) 	textInfo.text = "All your precious gold is stored here. Don't let sneaky goblins anywhere near! Upgrade the storage to increase its capacity and durability against attack.";
//				//else  			textInfo.text = "These storages contain the elixir pumped from underground. Upgrade them to increase the maximum amount of elixir you can store.";
//				building.UIFillProgress(progresses[0], BDInfo.StorageCapacity);
//				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if(bt.TB == 7) {
//				//textInfo.text = "The Barracks allow you to train troops to attack your enemies. Upgrade the Barracks to unlock advanced units that can win epic battles.";
//				//"Training Capacity : 0/20";
//				//"HitPoint"
//				building.UIFillProgress(progresses[0], BDInfo.TrainingCapacity);
//				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if(bt.TB == 8) {
//				//textInfo.text = "Your troops are stationed in Army Camps. Build more camps and upgrade them to muster a powerful army.";
//				//"Total troop Capacity : 0/20";
//				//"HitPoint"
//				// show troops icon (click to remove unit) "Remove Troops?"
//				building.UIFillProgress(progresses[0], BDInfo.TroopCapacity);
//				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
//				progresses[2].gameObject.SetActive(false);
//			}
//			else if((bt.TB == 9) || (bt.TB == 10)) {
//				//textInfo.text = "Cannons are great for point defense. Upgrade cannons to increase their firepower, but beware that your defensive turrets cannot shoot while being upgraded!";
//				//"Damage per second:15";
//				//"HitPoint"
//				//Range : 9 Tiles
//				//Damage Type: Single Target
//				//Targets: Ground
//				//Favorite target: Any
//			}
//			else if((bt.TB == 11) || (bt.TB == 12) || (bt.TB == 13)) {
//				building.UIFillProgress(progresses[0], BDInfo.DamagePerSecond);
//				building.UIFillProgress(progresses[1], BDInfo.HitPoint);
//				progresses[2].gameObject.SetActive(false);
//			}
			else {}
		}
		
		public void OnButtonOk() {
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