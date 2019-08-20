using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using conf;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIEditItem
///   Description:    building item in edit mode
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-02-21)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIEditItem : MonoBehaviour {

		private BuildingType 	bt = null;
		//private BuildingDef 	bd = null;
		public  int			 	LevelMax;
		public  int			 	Level;
		public  int			 	CountStart;
		public  int			 	Count;
		public string 			DisplayName;

		
		public 	Image 		Background;
		public 	Image 		Icon;
		public 	Text 		textDisplayName;
		public 	Text 		textCount;
		public 	Text 		textLevel;

		void Start () {
		
		}
		
		void Update () {
		
		}

		// initialize with building
		public void Init(BuildingType _bt, int _Level, int _Count) {
			bt = _bt;
			//bd = bt.Defs[0];
			DisplayName = bt.displayName;
			LevelMax = Level = _Level;
			CountStart = Count = _Count;
			Icon.sprite = bt.Icon;
			//textLevel.text = "Level "+Level.ToString ();
			//Debug.Log ("UIEditItem:Init BuildingType:"+bt.ID);
			UpdateUI();
		}
		
		public bool IsMatch(int type, int level=-1) {
			//Debug.Log ("id:"+bt.ID+" level:"+Level+"script id:"+script.bt.ID+"script level:"+script.Level);
			if(level == -1)	return (type == bt.ID) ? true : false;
			else  			return ((type == bt.ID) && (level == Level)) ? true : false;
		}

		public void UpdateUI() {
			textLevel.text = "Level "+Level.ToString ();
			textCount.text = (Count != 0) ? "x "+Count.ToString () : "";
			textDisplayName.text = DisplayName;
			Background.color = (Count != 0) ? Color.green : Color.gray;
			
		}

		public void OnButtonLevelClicked() {
			if(bt == null) return;
			if(Count <= 0) return;
			if(!SceneEdit.instance.ItemCountByTownHallLevel) return;

			Level++;
			if(Level > LevelMax)
				Level = 1;

			UpdateUI();
		}

		public void OnClicked() {
			if(bt == null) return;
			if(Count <= 0) return;
			if (SceneEdit.instance.bEraseMode) return;

			if (SceneEdit.instance.bAddMode)
			{

				SceneEdit.instance.AddModelBuildingType = bt;
			}
			else
			{
				if((BEGround.buildingSelected != null) && !BEGround.buildingSelected.Landed && !BEGround.buildingSelected.Landable)
    				return;
    
    			// add building. if buildtime is zero, then create level1, else not, create o level and upgrade start
    			// Building script = BEGround.instance.BuildingAdd (bt.ID, Level);
    			// 表要求从1开始
    			Building script = BEGround.instance.BuildingAdd(bt.ID, Level);
    			if(script != null) {
    
    				script.Move(Vector3.zero);
    
    				// if the building created was wall,
    				// check last selected wall's neighbor
    				// and change position of newly created wall
    				if((script.bt.Main_type == 1) && (SceneEdit.instance.lastSelectedWall != null)) {
    
    					Building building = SceneEdit.instance.lastSelectedWall;
    
    					// choose which direction 
    					Building buildingNeighbor = null;
    					Building buildingBlock = null;
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
    							buildingNeighbor = BEGround.instance.GetBuilding((int)building.tilePos.x + NeighborX, 
    								(int)building.tilePos.y+NeighborZ);
    							
    							buildingBlock = BEGround.instance.GetBuilding((int) building.tilePos.x - NeighborX,
    								(int) building.tilePos.y - NeighborZ);
    							// if wall finded
    							//if((buildingNeighbor != null) && (buildingNeighbor.Type == 2)) {
    							if( (buildingBlock == null) && (buildingNeighbor != null) && (buildingNeighbor.Type == 3)) {
    								bFind = true;
    								break;
    							}
    						}
    						
    						if(bFind) break;
    					}
    					//Debug.Log ("wall NeighborX:"+NeighborX.ToString ()+ "NeighborZ:"+NeighborZ.ToString ());
    
    					if (bFind)
    					{
    						// set inverse direction
                            tilePos.y = building.tilePos.y - (float) NeighborZ;
                            tilePos.x = building.tilePos.x - (float) NeighborX;
    //                        if(NeighborX == 0)	tilePos.y -= (float)NeighborZ;
    //                        else 				tilePos.x -= (float)NeighborX;
                            
                            //Debug.Log ("wall tilePos New:"+tilePos.ToString ());
                            script.Move((int)tilePos.x, (int)tilePos.y);
    					}
    					else
    					{
    						BEGround.instance.MoveToVacantTilePos(script);
    					}
    					SceneEdit.instance.lastSelectedWall = script;
    
    				}
    				else { 
    					BEGround.instance.MoveToVacantTilePos(script);
    				}
    
    				script.CheckLandable();
    				BEGround.instance.BuildingSelect(script);
    				script.Land(true,false);
    				script.Land(false,false);
    
    				Count--;
    				UpdateUI();
    			}			
			}

		}
	}
}