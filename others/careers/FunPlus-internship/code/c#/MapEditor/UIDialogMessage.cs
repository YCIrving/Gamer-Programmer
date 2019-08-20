using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogMessage
///   Description:    class for show message popup
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	public class UIDialogMessage : UIDialogBase {

		private static UIDialogMessage instance;
		public  static bool isModalShow = false;

		public Text 			Title;
		public Button 			ButtonExit;
		public Image 			Icon;
		public Text 			Message;
		public Button 			Button;
		public RectTransform 	trDialog;
		public RectTransform 	trTitlebar;
		public RectTransform 	trMessages;
		public RectTransform 	trButtons;

		public RectOffset  		Border;
		public float 			Spacing;
		public Vector2  		IconSize;
		
		public bool				Initialized = false;
		public bool				InHiding = false;
		int 					ButtonCount;
		int 					result = -1;
		Action<int> 			onFinish = null;

		void Awake () {
			instance=this;
			SetModal = false;
			gameObject.SetActive(false);
		}
		
		void Start () {
		}
		
		public override void Update () {

			base.Update();
			
			if(Initialized || InHiding) return;
			Initialized = true;
			//Debug.Log ("UIDialogMessage::Update Initialized true");
			
			SetUpShape();
		}
		
		// change dialog size by button count & message box size
		void SetUpShape() {
			float MessageHeight = Message.GetComponent<RectTransform>().sizeDelta.y;
			if(MessageHeight < 120.0f) MessageHeight = 120.0f;
			
			trMessages.sizeDelta = new Vector2(trMessages.sizeDelta.x, MessageHeight);
			
			float DialogHeight = 0.0f;
			if(string.Compare(Title.text, "") != 0) DialogHeight += trTitlebar.sizeDelta.y + Spacing;
			if(trButtons != null) DialogHeight += trButtons.sizeDelta.y + Spacing;
			DialogHeight += MessageHeight;
			DialogHeight += Border.top + Border.bottom;
			trDialog.sizeDelta = new Vector2(trDialog.sizeDelta.x, DialogHeight);
			
			if(string.Compare(Title.text, "") != 0) {
				trMessages.anchoredPosition = new Vector3(0.0f, 29.0f, 0.0f);
			}
		}
		
		void SetUpData(string message, string title, Sprite spr) {
			
			Message.text = message;
			
			if (title != null) {
				Title.text = title;
				trTitlebar.gameObject.SetActive(true);
			}
			else {
				trTitlebar.gameObject.SetActive(false);
				Title.text = "";
			}
			
			if (spr != null) {
				Icon.sprite = spr;
				Icon.gameObject.SetActive(true);
			}
			else {
				Icon.gameObject.SetActive(false);
				RectTransform MessageIn = Message.GetComponent<RectTransform>();
				MessageIn.sizeDelta = new Vector2(trDialog.sizeDelta.x - 20.0f, MessageIn.sizeDelta.y);
				MessageIn.anchoredPosition = new Vector3(0,0,0);
			}
		}
		
		// create buttons by input string
		void SetUpButtons(string texts) {

			// delete previously created buttons
			for(int j=trButtons.transform.childCount-1;j>=0;j--){
				if(trButtons.transform.GetChild(j).gameObject != Button.gameObject)
					Destroy (trButtons.transform.GetChild(j).gameObject);
			}
			// tokenize string by , 
			string [] textsub = texts.Split(',');
			ButtonCount = textsub.Length;
			var button = Button.gameObject;
			for(int i=0 ; i < ButtonCount ; ++i) {
				int iTemp = i;
				// first button is already exist
				// fill value for that
				if(i == 0) {
					//Text txt = button.transform.Find ("Text").GetComponent<Text>();
					button.transform.Find ("Text").GetComponent<Text>().text = textsub[0];
					button.GetComponent<Button>().onClick.AddListener(() => { result = iTemp; Hide(); });
				}
				else {
					// after 2nd button, instantiate button and fill data
					var buttonNew = Instantiate(button) as GameObject;
					buttonNew.transform.SetParent(button.transform.parent, false);
					buttonNew.transform.Find ("Text").GetComponent<Text>().text = textsub[i];
					buttonNew.GetComponent<Button>().onClick.AddListener(() => { result = iTemp; Hide(); });
				}
			}
		}
		
		void _Show (string message, string buttons, string title, Sprite spr, Action<int> onFinished) {

			onFinish = onFinished;
			SetUpButtons(buttons);
			SetUpData(message, title, spr);
			Initialized = false;
			InHiding = false;
			result = -1;
			ShowProcess();
		}

		void Close() {
			Initialized = false; 
			InHiding = true; 
			if (onFinish != null)
				onFinish(result);

			_Hide();
		}

		public static void Show(string message, string buttons, string title = null, Sprite spr = null, Action<int> onFinished = null) { instance._Show(message, buttons, title, spr, onFinished); }
		public static void Hide() 	{ instance.Close(); }
		public static bool IsShow() { return instance.Initialized ? true : false; }

	}
}