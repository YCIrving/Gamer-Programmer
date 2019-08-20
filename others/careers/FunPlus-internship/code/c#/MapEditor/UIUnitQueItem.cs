using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIUnitQueItem
///   Description:    unit trainig que item in training dialog
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIUnitQueItem : MonoBehaviour {

		private UIDialogTraining 	uiTraining = null;
		private bool 				Initialized = false;

		[HideInInspector]
		public 	GenQueItem			item = null;

		public 	Image 		UnitIcon;
		public 	GameObject 	goProgress;
		public 	Image 		Progress;
		public 	Text 		Count;
		public 	Text 		TimeLeft;

		void Update () {

			if(!Initialized) return; 
		
			// find camp with given housing space available
			Building buildingArmyCamp = BEGround.instance.FindCampWithSpace(item.at.HousingSpace);
			if(buildingArmyCamp != null) {

				if(!goProgress.activeInHierarchy) {
					goProgress.SetActive(true);
				}

				// show progress and left time
				Progress.fillAmount = ((float)item.at.TrainingTime - item.timeLeft)/(float)item.at.TrainingTime;
				TimeLeft.text = BENumber.SecToString((int)item.timeLeft);
			}
			else {

				if(goProgress.activeInHierarchy) {
					TimeLeft.text = "Stopped!";
					goProgress.SetActive(false);
				}
			}
			Count.text = item.Count.ToString()+"x";
		}

		// if user clicked '2' button to remove unit from the que
		public void OnButtonClicked() {
			// refund half of training cost
			int ReturnCost = item.at.Defs[0].TrainingCost/2;
			SceneTown.Elixir.ChangeDelta(ReturnCost);

			// remove from training dialog
			uiTraining.UnitGenRemove(item);
		}
		
		public void Init(UIDialogTraining _uiTraining, GenQueItem _item) {
			uiTraining = _uiTraining;
			item = _item;
			Initialized = true;

			UnitIcon.sprite = item.at.Icon;
		}
	}
}