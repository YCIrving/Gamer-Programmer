using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UICommand
///   Description:    class for command of selected building
///                   when user select a building
///                   this dialog show from the bottom of the screen
///                   user can click various command buttons
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	// command types
	public enum CommandType {
		None,
		Create,
		CreateCancel,
		Info,
		Upgrade, 	
		UpgradeCancel,
		UpgradeFinish,
		Remove, 	
		RemoveCancel,
		RemoveFinish,
		Boost,	
		BoostAll,
		Training,
		Research,
	}

	// class represent each command button
	public class CommandButton {
		
		//private static Color	colorShow = Color.black;
		//private static Color	colorNotShow = new Color(0,0,0,0);
		public  GameObject	go = null;
		public  Image 		Background;
		public  Text 		Title;
		public  Image 		Icon;
		public  Text 		Price;
		public  Image 		PriceIcon;
		public  Sprite		Symbol;
		public	UnityAction call = null;
		public	bool		Disabled = false;
		public 	bool 		Locked = false;

		public 	PayType 	payType = PayType.Gold;
		public 	int 		BuyPrice = 0;
		public 	PayDef 		pd = null;

		public CommandButton(GameObject _go, Sprite _symbol, string _text, PayType _payType, int _buyPrice, UnityAction _call) {
			go = _go;
			Background = go.GetComponent<Image>();
			Title = go.transform.Find ("Title").GetComponent<Text>();
			Icon = go.transform.Find ("Icon").GetComponent<Image>();
			Price = go.transform.Find ("Price").GetComponent<Text>();
			PriceIcon = go.transform.Find ("PriceIcon").GetComponent<Image>();
			Symbol = _symbol;
			Title.text = _text;
			call = _call;
			Disabled = false;
			Locked = false;

			payType = _payType;
			pd = TBDatabase.GetPayDef ((int)payType);
			BuyPrice = _buyPrice;

			Icon.overrideSprite = Symbol;

			Price.text = (BuyPrice == 0) ? "" : BuyPrice.ToString ("#,##0");
			//Debug.Log ("_payType:"+_payType.ToString());
			PriceIcon.overrideSprite = TBDatabase.GetPayDefIcon((int)_payType);
			PriceIcon.gameObject.SetActive ((BuyPrice == 0) ? false : true);
			//PriceIcon.gameObject.SetActive (false);

			State(false);
		}
		
		public void State(bool Selected) {
			if(!Selected) {
				//Background.color = Color.white;
				//Icon.color = colorShow;
				//Icon2.color = colorNotShow;
				//Text.color = clrText;
			}
			else {
				//Background.color = Color.green;
				//Icon.color = colorNotShow;
				//Icon2.color = colorShow;
				//Text.color = clrText;
			}
		}
		
		public void Update () {
			if((pd != null) && (pd.Number != null)) {
				Price.color = ((int)pd.Number.Target() < BuyPrice) ? Color.red : Color.white;
			}
		}
		
	}

	// command dialog
	public class UICommand : MonoBehaviour {
		
		private static UICommand instance;
		public  static int 			YOffset = 0;
		

		private Building 			building = null;
		public 	GameObject 			prefButton;
		public 	Transform 			trButtonRoot;
		public  Text 				Info;
		private List<CommandButton>	Buttons=new List<CommandButton>();

		public  static bool 		Visible = false;

		void Awake () {
			instance=this;
		}
		
		void Start () {
			Visible = false;
		}
		
		void Update () {

			if(!UIDialogMessage.isModalShow) {
				if((BEGround.buildingSelected != null) && !UICommand.Visible) {
					UICommand.Show (BEGround.buildingSelected);
				}
				else if((BEGround.buildingSelected == null) && UICommand.Visible) {
					UICommand.Hide ();
				}
				else {}
			}

			if(Visible) {
				for(int i=0 ; i < Buttons.Count ; ++i)
					Buttons[i].Update ();

				if(building != null) {
					if(building.OnceLanded && !building.Landed && trButtonRoot.gameObject.activeInHierarchy)
						trButtonRoot.gameObject.SetActive(false);

					if(building.Landed && !trButtonRoot.gameObject.activeInHierarchy)
						trButtonRoot.gameObject.SetActive(true);

				}
			}
		}
		
		public void Reset () {
			for(int i=0 ; i < Buttons.Count ; ++i) {
				Destroy (Buttons[i].go);
			}
			Buttons.Clear();
		}

		// add button to command dialog
		public CommandButton AddButton(Sprite symbol, string text, PayType _payType, int buyPrice, CommandType ct) {
			GameObject go = (GameObject)Instantiate(prefButton, Vector3.zero, Quaternion.identity);
			go.transform.SetParent(trButtonRoot);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			
			CommandButton newButton = new CommandButton(go, symbol, text, _payType, buyPrice, null);
			Buttons.Add(newButton);
			
			CommandType tempCT = ct;
			Button button = go.GetComponent<Button>();
			button.onClick.RemoveAllListeners(); 
			button.onClick.AddListener(() => { ButtonClicked(tempCT); });
			
			return newButton;
		}

		// set buttons position to align center
		// and add move animation
		public void ButtonReposition() {
			int Count = Buttons.Count;
			float Offset = (Count-1)*120.0f*-0.5f;
			for(int i=0 ; i < Count ; ++i) {
				Vector3 vEnd = new Vector3(Offset, 0, 0);
				Vector3 vStart = new Vector3(Offset, -180, 0);
				Offset+= 120.0f;

				BETween.position(Buttons[i].go, 0.2f, vStart, vEnd).delay = 0.1f*(float)i;
			}
		}

		public void MessageBoxResultUpgradeCancel(int result) {
			BEAudioManager.SoundPlay(6);
			if(result == 0) {
				building.UpgradeCancel();
				Hide();
			}
		}

		public void MessageBoxResultRemoveCancel(int result) {
			BEAudioManager.SoundPlay(6);
			if(result == 0) {
				building.RemoveCancel();
				Hide();
			}
		}
		
		// when button clicked
		public void ButtonClicked(CommandType ct) {
			//Debug.Log ("ButtonClicked "+ct.ToString());
			BEAudioManager.SoundPlay(6);

			// if newly created building is selected
			// only perform create and cancel
			if(building.OnceLanded == false) {

				// if user clicked 'create' button
				if(ct == CommandType.Create) {

					// building can land
					if(building.Landable) {

						// hide command dialog
						Hide();
						// land building and unselect
						BEGround.instance.BuildingLandUnselect();

						// decrease build price of the building
						BuildingDef bd = TBDatabase.GetBuildingDef(building.Type, (building.Level == 0) ? 1 : building.Level);
						building.PayforBuild(bd);
						// if building level is 0(need buildtime), upgrade to level 1 start 
						// if not, check resource capacity
						if(building.Level == 0)  	
							building.Upgrade();
//						else  						
//							BEGround.instance.CapacityCheck();
					}

					// if house is created add worker 
					if(building.Type == 1) {
						BEWorkerManager.instance.AddWorker();
					}

					SceneTown.instance.Save();

					// if wall is created,automatically next wall create for convenience
					if(building.Type == 2) {

						// check if user has enough gold and wall count is not max count
						BuildingDef bd = TBDatabase.GetBuildingDef(building.Type, 1);
						bool Available = bd.PriceInfoCheck(null);
						//int CountMax = BEGround.instance.GetBuildingCountMax(building.Type);
						int CountMax = 99999;
						int Count = BEGround.instance.GetBuildingCount(building.Type);
						if(Available && (Count < CountMax)) {

							//Debug.Log ("wall tilePos:"+building.tilePos.ToString ());
							// add another wall automatically
							Building script = BEGround.instance.BuildingAdd (2,1);
							if(script != null) {

								// choose which direction 
								Building buildingNeighbor = null;
								Vector2 tilePos = Vector2.zero;
								int NeighborX = 0;
								int NeighborZ = 0;
								bool bFind = false;
								// check prev and next tile in x,z coordination
								for(int dir=0 ; dir < 2 ; ++dir) {
									for(int value=0 ; value < 2 ; ++value) {
										if(dir==0) {
											NeighborX = 0;
											NeighborZ = ((value==0) ? -1 : 1);
										}
										else {
											NeighborX = ((value==0) ? -1 : 1);
											NeighborZ = 0;
										}
										buildingNeighbor = BEGround.instance.GetBuilding((int)building.tilePos.x+NeighborX, (int)building.tilePos.y+NeighborZ);

										// if wall finded
										if((buildingNeighbor != null) && (buildingNeighbor.Type == 2)) {
											bFind = true;
											break;
										}
									}

									if(bFind) break;
								}
								//Debug.Log ("wall NeighborX:"+NeighborX.ToString ()+ "NeighborZ:"+NeighborZ.ToString ());

								// set inverse direction
								tilePos = building.tilePos;
								if(NeighborX == 0)	tilePos.y -= (float)NeighborZ;
								else 				tilePos.x -= (float)NeighborX;

								//Debug.Log ("wall tilePos New:"+tilePos.ToString ());
								script.Move((int)tilePos.x, (int)tilePos.y);
								script.CheckLandable();
								BEGround.instance.BuildingSelect(script);
							}
						}
					}

					// error fix - if game quit while building wall sequencely, last unlanded wall is saved (without payment)
					//SceneTown.instance.Save();
					BEWorkerManager.instance.SetWorker(building);
				} 
				// if user clicked 'cancel' button
				else if(ct == CommandType.CreateCancel) {
					// hide command dialog
					Hide();
					// delete temporary created building
					BEGround.instance.BuildingDelete(BEGround.buildingSelected);
				}
				else {}
			}
			else {
				if(ct == CommandType.Info) {
					Hide();
					UIDialogInfo.Show(building);
				} 
				else if(ct == CommandType.Upgrade) {
					// check if worker available
					if(BEWorkerManager.instance.WorkerAvailable()) {
						Hide();
						UIDialogUpgradeAsk.Show(building);
					}
					else {
						UIDialogMessage.Show("All workers are working now", "Ok", "No Worker Available");
					}
				}
				else if(ct == CommandType.UpgradeCancel) {
					UIDialogMessage.Show("Cancel current upgrade?", "Yes,No", "Cancel Upgrade ?", null, (result) => { MessageBoxResultUpgradeCancel(result); } );
				}
				else if(ct == CommandType.UpgradeFinish) {

					// if instant finish button was clicked
					int GemCount = building.GetUpgradeFinishGemCount();
					// user has enough gem to finish
					if(SceneTown.Gem.Target() >= GemCount) {
						// decrease gem
						SceneTown.Gem.ChangeDelta(-GemCount);
						// complete upgrade
						building.UpgradeCompleted = true;
						Hide();
					}
					else {
						UIDialogMessage.Show("You need more gems to finish this work immediately", "Ok", "Need More Gems");
					}
				}
				else if(ct == CommandType.Remove) {
					// check if worker available
					if(BEWorkerManager.instance.WorkerAvailable()) {
						if(building.Remove()) {
							// set this building to worker
							BEWorkerManager.instance.SetWorker(building);
							Hide();
						}
					}
					else {
						UIDialogMessage.Show("All workers are working now", "Ok", "No Worker Available");
					}
				}
				else if(ct == CommandType.RemoveCancel) {
					UIDialogMessage.Show("Cancel current removing?", "Yes,No", "Cancel Remove ?", null, (result) => { MessageBoxResultRemoveCancel(result); } );
				}
				else if(ct == CommandType.RemoveFinish) {
					
					// if instant finish button was clicked
					int GemCount = building.GetRemoveFinishGemCount();
					// user has enough gem to finish
					if(SceneTown.Gem.Target() >= GemCount) {
						// decrease gem
						SceneTown.Gem.ChangeDelta(-GemCount);
						// complete upgrade
						building.RemoveCompleted = true;
						building.RemoveEnd();
					}
					else {
						UIDialogMessage.Show("You need more gems to finish this work immediately", "Ok", "Need More Gems");
					}
				}
				else if(ct == CommandType.Training) {
					Hide();
					UIDialogTraining.Show(building);
				}
				else {}
			}
		}

		public void _Show(Building script) {

			building = script;
			Reset();
			Visible = true;

			// if newly create building was selected 
			if(building.OnceLanded == false) {
				// only show 'create','cancel' button
				AddButton(Resources.Load<Sprite>("Icons/UI/CheckedMark"), "Ok", PayType.None, 0, CommandType.Create);
				AddButton(Resources.Load<Sprite>("Icons/UI/Multiplication"), "Cancel", PayType.None, 0, CommandType.CreateCancel);
			}
			else {

				if(building.bt.Category != TargetType.Deco) {
					// if building is not in creation, show 'info' button
					if(building.Level != 0)
						AddButton(Resources.Load<Sprite>("Icons/UI/Info"), "Info", PayType.None, 0, CommandType.Info);
					
					// if building can upgrade to next level
					if(building.defNext != null) {
						
						// if in upgrading
						if(building.InUpgrade) {
							if(!building.UpgradeCompleted) {
								// shows 'Cancel','Finish' buttons
								AddButton(Resources.Load<Sprite>("Icons/UI/Multiplication"), "Cancel", PayType.None, 0, CommandType.UpgradeCancel);
								int	GemCount = building.GetUpgradeFinishGemCount();
								AddButton(Resources.Load<Sprite>("Icons/UI/Gem"), "Finish\nNow", PayType.Gem, GemCount, CommandType.UpgradeFinish);
							}
						}
						else {
							
							// get price to upgrade
							PayType payType = PayType.None;
							int 	BuyPrice = 0;
							for(int i=0 ; i < (int)PayType.Max ; ++i) {
								
								if(building.defNext.BuildPrice[i] != 0) { 
									payType = (PayType)i; 	
									BuyPrice = building.defNext.BuildPrice[i]; 
									break;
								}
							}
							
							// add 'upgrade' button
							AddButton(Resources.Load<Sprite>("Icons/UI/UpArrow"), "Upgrade", payType, BuyPrice, CommandType.Upgrade);
						}
					}
					
					// is building is barrack 
					if((building.Type == 7)&& !building.InUpgrade) {
						// add'Train' button
						AddButton(Resources.Load<Sprite>("Icons/UI/Training"), "Train\nTroops", 0, 0, CommandType.Training);
					}				
				}
				else {

					if(building.InRemove) {
						if(!building.RemoveCompleted) {
							// shows 'Cancel','Finish' buttons
							AddButton(Resources.Load<Sprite>("Icons/UI/Multiplication"), "Cancel", PayType.None, 0, CommandType.RemoveCancel);
							int	GemCount = building.GetRemoveFinishGemCount();
							AddButton(Resources.Load<Sprite>("Icons/UI/Gem"), "Finish\nNow", PayType.Gem, GemCount, CommandType.RemoveFinish);
						}
					}
					else {
						
						// get price to upgrade
						PayType payType = PayType.None;
						int 	BuyPrice = 0;
						for(int i=0 ; i < (int)PayType.Max ; ++i) {
							
							if(building.def.BuildPrice[i] != 0) { 
								payType = (PayType)i; 	
								BuyPrice = building.def.BuildPrice[i]; 
								break;
							}
						}
						
						// add 'remove' button
						AddButton(Resources.Load<Sprite>("Icons/UI/Multiplication"), "Remove", payType, BuyPrice, CommandType.Remove);
					}
				}
			}
			
			ButtonReposition();

			// if normal building
			if(building.bt.Category != TargetType.Deco) 	{
				int Level = (building.Level == 0) ? 1 : building.Level;
				Info.text = TBDatabase.GetBuildingNameByID(building.Type) + "  Level "+Level.ToString ();
			}
			// if deco object
			else {
				Info.text = TBDatabase.GetBuildingNameByID(building.Type);
			}

			//BETween.anchoredPosition3D(gameObject, 0.2f, new Vector3(0,-250,0), new Vector3(0,0,0));//.method = BETweenMethod.easeOut;
			StartCoroutine(TweenMov(GetComponent<RectTransform>(), new Vector2(0,-250), new Vector2(0,YOffset), 0.2f, 0));
		}

		public void _Hide() {
			//BETween.anchoredPosition3D(gameObject, 0.2f, new Vector3(0,0,0), new Vector3(0,-250,0));//.method = BETweenMethod.easeOut;
			StartCoroutine(TweenMov(GetComponent<RectTransform>(), new Vector2(0,YOffset), new Vector2(0,-250), 0.2f, 0));
			Visible = false;
		}
		
		public static void Show(Building script) 	{ instance._Show(script); }
		public static void Hide() 					{ instance._Hide(); }

		public IEnumerator TweenMov(RectTransform tr, Vector2 Start, Vector2 End, float time, float fDelay) {
			
			if(fDelay > 0.01f)
				yield return new WaitForSeconds(fDelay);
			
			float fAge = 0.0f;
			Vector2 vPos = Start;
			bool Completed = false;
			while(true) {
				tr.anchoredPosition = vPos;
				if(Completed) {

					break;
				}
				
				yield return new WaitForSeconds(0.03f);
				
				fAge += 0.03f;
				float fRatio = fAge/time;
				vPos = Vector2.Lerp(Start, End, fRatio);
				if(fRatio > 1.0f) {
					Completed = true;
					vPos = End;
				}
			}
		}
	}
	
}
