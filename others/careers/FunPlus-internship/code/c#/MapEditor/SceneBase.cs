using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;  

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          SceneBase
///   Description:    main class of town edit mode
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class DataOverScene {
		static public string BaseName = "";
	}


	// resource price to build buildings
	public enum SceneType {
		None	= -1, 	
		Town	= 0, 	
		World	= 1,
		Battle	= 2,
		Edit	= 3,	
		Test	= 4,
		Max		= 5,
	}
	

	public class SceneBase : MonoBehaviour {

		[HideInInspector]
		public 	SceneType	sceneType = SceneType.None;
		[HideInInspector]
		public 	bool 		InLoading = false;
		public	int			UpdateCount = 0;

		// related to save and load gamedata with xml format
		public	bool		UseEncryption = false;
		public	bool		bFirstRun = false;
		public	string 		configFilename = "Config.xml";
		public	int 		ConfigVersion = 1;
		

		public virtual void Update () {
		
			// if user pressed escape key, show quit messagebox
			if (!UIDialogMessage.IsShow() && !UIDialogMessage.isModalShow && Input.GetKeyDown(KeyCode.Escape)) { 
				UIDialogMessage.Show("Do you want to quit this program?", "Yes,No", "Quit?", null, (result) => { MessageBoxResult(result); } );
			}
			
		}

		public void Init(SceneType _sceneType) {
			sceneType = _sceneType;
			BEGround.scene = this;
		}

		public virtual void Save() {
			Debug.Log ("SceneBase::Save");
		}

		public virtual void Load() {
			Debug.Log ("SceneBase::Load");
		}

		public virtual void GainExp(int exp) {
		}

		// result of quit messagebox
		public void MessageBoxResult(int result) {
			BEAudioManager.SoundPlay(6);
			if(result == 0) {
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
				#else
				Application.Quit();
				#endif
			}
		}

	}
}