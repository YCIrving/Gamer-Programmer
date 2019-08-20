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
	public class UIDialogNew : UIDialogBase {
		
		private static UIDialogNew instance;
		public  static bool isModalShow = false;
		private bool 		bModeNew = false;

		public  Text 		textTitle;
		public  InputField	IFMapName;
		public  InputField	IFMapLevel;
		public  InputField	IFMapGold;
		public  InputField	IFMapElixir;

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
				textTitle.text = "New Map";
				IFMapName.text = "";
				IFMapLevel.text = "1";
				IFMapGold.text = "0";
				IFMapElixir.text = "0";
			}
			else {
				textTitle.text = "Edit Map";
				IFMapName.text = SceneEdit.instance.Filename;
				IFMapLevel.text = SceneEdit.instance.TownHallLevel.ToString();
				IFMapGold.text = SceneEdit.instance.Gold.ToString("#,##0");
				IFMapElixir.text = SceneEdit.instance.Elixir.ToString("#,##0");
			}

			ShowProcess();
		}

		public void OnInputName(string strValue) {
			Debug.Log ("OnInputName:"+strValue);
		}
		
		public void OnInputLevel(string strValue) {
			Debug.Log ("OnInputLevel:"+strValue);
		}
		
		public void OnInputGold(string strValue) {
			Debug.Log ("OnInputGold:"+strValue);
		}
		
		public void OnInputElixir(string strValue) {
			Debug.Log ("OnInputElixir:"+strValue);
		}
		
		public void OnOk() {
			Debug.Log ("UIDialogNew::OnOk"+name);

			if(bModeNew) {
				//SceneEdit.instance.Reset(int.Parse(IFMapLevel.text));
				SceneEdit.instance.JsonReset(int.Parse(IFMapLevel.text));
			}

			SceneEdit.instance.Filename = IFMapName.text;
			SceneEdit.instance.TownHallLevel = int.Parse(IFMapLevel.text);
			SceneEdit.instance.Gold = double.Parse(IFMapGold.text);
			SceneEdit.instance.Elixir = double.Parse(IFMapElixir.text);
			SceneEdit.instance.UpdateUI();

			_Hide();
		}
		
		void Close() {
			
			_Hide();
		}
		
		public static void Show(bool bNew=true) { instance._Show(bNew); }
		public static void Hide() 	{ instance.Close(); }
		
	}
}