using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogUnitInfo
///   Description:    class for unit info
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	// what kind of building info
	public enum UnitInfo {
		None				= -1,
		DamagePerSecond		= 0, 	
		HitPoint			= 1, 	
		TrainingCost		= 2, 	
	}

	public class UIDialogUnitInfo : MonoBehaviour {
		
		private static UIDialogUnitInfo instance;
		
		private int 			unitID = -1;
		private ArmyType 		at = null;
		//private ArmyDef 		ad = null;

		public	UIDialogTraining	scriptUITraining;	
		public	Image 				Dialog;
		public	Text 				textTitle;
		public	Text 				textLevel;
		public	Image 				imgIcon;
		public	ProgressInfo []		progresses;
		public	GameObject 			goInfo;
		public	Text 				textInfo;
		public	Text 				textFavoriteTarget;
		public	Text 				textDamageType;
		public	Text 				textTargets;
		public	Text 				textHousingSpace;
		public	Text 				textTrainingTime;
		public	Text 				textMovementSpeed;

		void Awake () {
			instance=this;
		}
		
		void Start () {

		}
		
		void Update () {
			
		}
		
		void Reset () {
			if(unitID == -1) return;

			at = TBDatabase.GetArmyType(unitID);
			//ad = (at != null) ? at.GetDefine(1) : null;

			if(at == null) return;

			textTitle.text = at.Name;
			int Level = 1;
			textLevel.text = "Level "+Level.ToString ();
			imgIcon.sprite = at.Icon;

			for(int i=0 ; i < progresses.Length ; ++i) 
				progresses[i].gameObject.SetActive(true);

			// display progresses of building by building type
			textInfo.text = at.Desc;
			at.UIFillProgress(progresses[0], UnitInfo.DamagePerSecond, Level);
			at.UIFillProgress(progresses[1], UnitInfo.HitPoint, Level);
			at.UIFillProgress(progresses[2], UnitInfo.TrainingCost, Level);

			textFavoriteTarget.text = at.eFavoriteTarget.ToString();
			textDamageType.text = at.eDamageType.ToString ();
			textTargets.text = at.eTargets.ToString ();
			textHousingSpace.text = at.HousingSpace.ToString ();
			textTrainingTime.text = BENumber.SecToString(at.TrainingTime);
			textMovementSpeed.text = at.MoveSpeed.ToString ();

		}
		
		public void OnButtonBack() {
			scriptUITraining.UnitInfo(-1);
		}
		
		public void OnButtonExit() {
			scriptUITraining._Hide();
		}
		
		public void _Show(int _unitID) {
			
			unitID = _unitID;
			Reset();
			Dialog.GetComponent<RectTransform>().anchoredPosition = (unitID == -1) ? new Vector2(0,-720) : new Vector2(0,0);
		}

		public void _Hide() {
		}
		
		public static void Show(int _unitID) 	{ instance._Show(_unitID); }
		public static void Hide() 				{ instance._Hide(); }
	}
}