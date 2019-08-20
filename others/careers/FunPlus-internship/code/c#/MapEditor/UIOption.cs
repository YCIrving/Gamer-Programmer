using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIOption
///   Description:    class for option
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIOption : UIDialogBase {
		
		private static UIOption instance;
		
		public 	Toggle 		uiMusicToggle;
		public 	Toggle 		uiSoundToggle;

		void Awake () {
			instance=this;
			gameObject.SetActive(false);
		}
		
		void Start () {
		}
		
		public override void Update () {
			if (Input.GetKeyDown(KeyCode.Escape)) { 
				_Hide();
			}
		}
		
		void OnEnable(){
			uiMusicToggle.isOn = (BESetting.MusicVolume != 0) ? false : true;
			uiSoundToggle.isOn = (BESetting.SoundVolume != 0) ? false : true;
		}

		// when use clicked music button
		public void MusicToggled(bool value) {
			BEAudioManager.SoundPlay(6);
			// toggle music value and save
			BESetting.MusicVolume = value ? 0 : 100;
			BESetting.Save();

			// play or stop music
			if(value) 	{
				BEAudioManager.instance.MusicStop();
			}
			else {
				if(!BEAudioManager.instance.MusicIsPlaying())
					BEAudioManager.instance.MusicPlay();
			}
		}

		// when user clicked sfx button
		public void SoundToggled(bool value) {
			BEAudioManager.SoundPlay(6);
			//toggle sound value and save
			BESetting.SoundVolume = value ? 0 : 100;
			BESetting.Save();
			//Debug.Log ("SoundVolume:"+BEUtil.instance.SoundVolume.ToString ());
		}
		
		void _Show () {
			Time.timeScale = 0;
			ShowProcess();
		}
		
		public static void Show() { instance._Show(); }
		public static void Hide() { instance._Hide(); }

	}
	
}