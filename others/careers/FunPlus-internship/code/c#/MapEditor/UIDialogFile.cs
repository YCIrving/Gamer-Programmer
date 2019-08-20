using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.IO;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogFile
///   Description:    class for show message popup
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-03-02)
///-----------------------------------------------------------------------------------------
namespace BE {
	public class UIDialogFile : UIDialogBase {
		
		private static UIDialogFile instance;
		public  static bool isModalShow = false;
		
		public GameObject 	prefFileItem;
		public Transform 	trContent;
		
		void Awake () {
			instance=this;
			gameObject.SetActive(false);
		}
		
		void Start () {
		}
		
		public override void Update () {
			
			base.Update();
			
		}

		// create buttons by input string
		void SetUpButtons(string texts) {
			
			// delete previously created buttons
		}

		void _Show (string strPath, string strMask) {
			
			DirectoryInfo dir = new DirectoryInfo(strPath);
			FileInfo[] info = dir.GetFiles(strMask);

			for(int j=trContent.childCount-1;j>=0;j--){
				Destroy (trContent.GetChild(j).gameObject);
			}

			int i=0;
			string filenameOnly = "";
			foreach (FileInfo f in info) { 
				filenameOnly = Path.GetFileNameWithoutExtension(f.Name);
				//Debug.Log ("FileInfo:"+filenameOnly);
				GameObject newItem = Instantiate(prefFileItem) as GameObject;
				newItem.transform.SetParent(trContent, false);
				newItem.transform.localPosition = new Vector3(0,-40*i,0);
				newItem.transform.Find ("Text").GetComponent<Text>().text = filenameOnly;
				string strTemp = filenameOnly;
				newItem.GetComponent<Button>().onClick.AddListener(() => { OnSelected(strTemp); });
				i++;
			}
			RectTransform rtContent = trContent.GetComponent<RectTransform>();
			Vector2 vSize = rtContent.sizeDelta;
			vSize.y = 40 * i;
			rtContent.sizeDelta = vSize;

			ShowProcess();
		}

		public void OnSelected(string name) {
			//Debug.Log ("OnSelected: "+name);
			SceneEdit.instance.OnButtonLoadJson(name);
			_Hide();
		}
		
		void Close() {

			_Hide();
		}
		
		public static void Show(string strPath, string strMask) { instance._Show(strPath, strMask); }
		public static void Hide() 	{ instance.Close(); }

	}
}