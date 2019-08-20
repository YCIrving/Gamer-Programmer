using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogTraining
///   Description:    class for unit training
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class UIDialogBattleResult : UIDialogBase {
		
		private static UIDialogBattleResult instance;
		
		public	GameObject 		prefUIBattleRewardItem;
		public	Text 			textTitle;
		public	Text 			textInfo;
		public	Transform 		trRootReward;
		public	Text 			textNoReward;
		public	Transform 		trRootCasualties;
		public	Text 			textNoCasualties;

		private bool			GameWin = false;

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

			textTitle.text = GameWin ? "Victory" : "Defeat";
			textTitle.color = GameWin ? Color.white : (Color)new Color32(255,138,98,255);

			for(int i=trRootReward.childCount-1;i>=0;i--){
				Destroy (trRootReward.GetChild(i).gameObject);
			}
			for(int i=trRootCasualties.childCount-1;i>=0;i--){
				Destroy (trRootCasualties.GetChild(i).gameObject);
			}

			float fDelay = 1.0f;
			for(int i=0 ; i < 2 ; ++i) {
				GameObject go = (GameObject)Instantiate(prefUIBattleRewardItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (trRootReward);
				go.transform.Find ("Icon").GetComponent<Image>().sprite = TBDatabase.GetPayDefIcon(i);

				if(i == 0) 	go.transform.Find ("Text").GetComponent<Text>().text = SceneBattle.instance.PlunderGold.ToString();
				else  		go.transform.Find ("Text").GetComponent<Text>().text = SceneBattle.instance.PlunderElixir.ToString();

				go.GetComponent<CanvasGroup>().alpha = 0;
				{ BETween bt = BETween.scale(go, 0.3f, new Vector3(1.4f,1.4f,1.4f), new Vector3(1,1,1)); bt.delay = fDelay; bt.method = BETweenMethod.easeOutBack; }
				{ BETween bt = BETween.alpha(go, 0.3f, 0.0f, 1.0f); bt.delay = fDelay; bt.method = BETweenMethod.easeOutBack; }

				fDelay += 0.8f;
			}

			int CasualtiesCountTotal = 0;
			int UnitTypeCount = SceneBattle.instance.Units.Count;
			for(int i=0 ; i < UnitTypeCount ; ++i) {

				BattleUnit bu = SceneBattle.instance.Units[i];
				int CasualtiesCount = SceneBattle.instance.Casualties[i];

				if(CasualtiesCount == 0) continue;

				CasualtiesCountTotal += CasualtiesCount;

				GameObject go = (GameObject)Instantiate(prefUIBattleRewardItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (trRootCasualties);
				ArmyType at = TBDatabase.GetArmyType(bu.ID);
				go.transform.Find ("Icon").GetComponent<Image>().sprite = at.Icon;
				go.transform.Find ("Text").GetComponent<Text>().text = CasualtiesCount.ToString();
				
				go.GetComponent<CanvasGroup>().alpha = 0;
				{ BETween bt = BETween.scale(go, 0.3f, new Vector3(1.4f,1.4f,1.4f), new Vector3(1,1,1)); bt.delay = fDelay; bt.method = BETweenMethod.easeOutBack; }
				{ BETween bt = BETween.alpha(go, 0.3f, 0.0f, 1.0f); bt.delay = fDelay; bt.method = BETweenMethod.easeOutBack; }
				
				fDelay += 0.8f;
			}

			if(CasualtiesCountTotal != 0)
				textNoCasualties.text = "";

			textNoReward.gameObject.SetActive (false);
		}
		
		public void OnButtonOk() {
			BEAudioManager.SoundPlay(6);
			//UIDialogBattleResult.Hide();
			if(SetModal)
				UIDialogMessage.isModalShow = false;

			//Application.LoadLevel("Town");
			SceneManager.LoadScene("Town");
		}
		
		public void _Show(bool _GameWin) {
			GameWin = _GameWin;
			Reset();
			ShowProcess();
		}
		
		public static void Show(bool _GameWin) 	{ instance._Show(_GameWin); }
		public static void Hide() 	{ instance._Hide(); }
	}
}