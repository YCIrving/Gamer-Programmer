using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIInfo
///   Description:    class for diaplay building info & progress
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIInfo : MonoBehaviour {
		
		public 	CanvasGroup groupRoot;
		public 	CanvasGroup groupInfo;
		public 	Text 		Name;
		public 	Text 		Level;

		public 	CanvasGroup groupProgress;
		public 	Text 		TimeLeft;
		public 	Image 		Progress;
		public 	Image 		Icon;

		public 	CanvasGroup groupCollect;
		public 	Image 		CollectDialog;
		public 	Image 		CollectIcon;

		public	Building 	building = null;

		void Update () {
			
		}

		// when user clicked collect dialog
		public void OnButtonCollect() {
			// do collect
			building.Collect();
		}
		
	}
	
}