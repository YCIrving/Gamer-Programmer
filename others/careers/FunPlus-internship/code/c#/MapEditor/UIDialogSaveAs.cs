using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.IO;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogNew
///   Description:    class for show message popup
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-03-02)
///-----------------------------------------------------------------------------------------
namespace BE {
	public class UIDialogSaveAs : UIDialogBase {
		
		private static UIDialogSaveAs instance;
		public  static bool isModalShow = false;
		private bool 		bModeNew = false;

		public  Text 		textTitle;
		public  InputField	IFMapName;

		void Awake () {
			instance=this;
			gameObject.SetActive(false);
		}
		
		void Start () {
		}
		
		public override void Update () {
			
			base.Update();
			
		}
		
		void _Show (bool bNew=true) {

			bModeNew = bNew;
			if(bModeNew) {
				textTitle.text = "Save As Map";
				IFMapName.text = "";
			
			}
			
			ShowProcess();
		}

		public void OnInputName(string strValue) {
			Debug.Log ("OnInputName:"+strValue);
		}
		
		public void OnOk() {
			Debug.Log ("UIDialogSaveAs::OnOk"+name);
			SceneEdit.instance.Filename = IFMapName.text;
			SceneEdit.instance.OnButtonSaveJson();
			_Hide();
		}
		
		void Close() {
			
			_Hide();
		}
		
		public static void Show(bool bNew=true) { instance._Show(bNew); }
		public static void Hide() 	{ instance.Close(); }
		
	}
}