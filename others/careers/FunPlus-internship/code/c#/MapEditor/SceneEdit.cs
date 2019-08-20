using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CocView;
using Newtonsoft.Json;
using Wod.ThirdParty.Util;
using UnityEditor;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          SceneEdit
///   Description:    main class of town edit mode
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-02-15)
///-----------------------------------------------------------------------------------------
namespace BE
{
    public class SceneEdit : SceneBase, BECameraRTSListner
    {
        public static SceneEdit instance;

        public BEGround ground = null;
        public bool bEraseMode = false;
        public bool bAddMode = false;
        public bool bMultiChooseMode = false;
        public bool bEnterMultiChooseModeByClick = false;
        public BuildingType AddModelBuildingType = null;
        public Text textMapInfo;
        public Text textEraseMode;
        public Text textAddMode;
        public Text textMultiChooseMode;
        public Text textName;
        public Text textInfo;
        public Transform trButtonRemove;
        public Transform trButtonSelectLongestWall;
        public GameObject prefabEditItem;
        public GameObject content;

        private List<UIEditItem> Items = new List<UIEditItem>();

        private Building MouseClickedBuilding = null;
        private bool bBuildingSelected = true;

        public string Filename = "";
        private DateTime SavedTime;
        private int ExpTotal = 0;
        public double Gem = 0;
        public double Gold = 0;
        public double Elixir = 0;
        private double Shield = 0;
        public int TownHallLevel = 1;

        public Building lastSelectedWall = null;
        [HideInInspector] public bool ItemCountByTownHallLevel = true;

        private string strMapsPath = "";
//        private List<int> UnShowModel = new List<int>();

        private float workTime;

        public void UpdateUI()
        {
            textMapInfo.text = "name:<color=yellow>" + Filename + "</color> - town level:<color=yellow>" +
                               TownHallLevel + "</color> - gold:<color=yellow>" +
                               Gold.ToString("#,##0") + "</color> - elixir:<color=yellow>" +
                               Elixir.ToString("#,##0") + "</color>";
        }

        void Awake()
        {
            instance = this;
            Init(SceneType.Edit);

            // Get Maps Folder Path
            strMapsPath = Application.dataPath;
            strMapsPath = strMapsPath.Substring(0, strMapsPath.LastIndexOf('/'));
            strMapsPath += "/Assets/_Res/Map";
            /*for (int i = 7; i < 14; i++)
            {
                UnShowModel.Add(i);
            }*/

            // 定时器自动保存
        }

        void Start()
        {
            // delete file 
            //File.Delete(BEUtil.pathForDocumentsFile(".xml"));

            BECameraRTS.instance.Listner = this;
            textEraseMode.text = bEraseMode ? "Erase Mode \nOn" : "EraseMode \nOff";
            textAddMode.text = bAddMode ? "Add Mode \n On" : "AddMode \nOff";
            textMultiChooseMode.text = bMultiChooseMode ? "Multi-Choose \n On" : "Multi-Choose \nOff";

            UpdateUI();

            DeckReset();

            // fill Deck
/*			DeckItemAdd(0,1,1);
			DeckItemAdd(1,1,2);
			DeckItemAdd(2,1,20);
			DeckItemAdd(2,2,10);
			DeckItemAdd(2,3,10);
			DeckItemAdd(3,1,4);
			DeckItemAdd(3,2,3);
			DeckItemAdd(4,1,4);
			DeckItemAdd(5,1,4);
			DeckItemAdd(6,1,4);
			DeckItemAdd(7,1,4);*/
            // resize content size to fit item count
            content.GetComponent<RectTransform>().sizeDelta = new Vector3(132 * Items.Count, 146);
            workTime = Time.time;
        }

        public override void Update()
        {
            bool bBuildingSelectedNew = (BEGround.buildingSelected != null) ? true : false;
            if (bBuildingSelected != bBuildingSelectedNew)
            {
                bBuildingSelected = bBuildingSelectedNew;

                trButtonRemove.gameObject.SetActive(bBuildingSelected);
                textInfo.color = bBuildingSelected ? new Color(1, 1, 1, 0) : Color.white;

            }

            if (bBuildingSelected &&
                BEGround.buildingSelected.bt.Main_type == 1 && BEGround.buildingSelected.Landed)
            {
                if (!trButtonSelectLongestWall.gameObject.activeSelf)
                trButtonSelectLongestWall.gameObject.SetActive(true);
            }
            else
            {
                if(trButtonSelectLongestWall.gameObject.activeSelf)
                    trButtonSelectLongestWall.gameObject.SetActive(false);
            }
                
            
            // 关闭自动保存功能
            // autoSaveMap();
            
            DeleteBuildByInputKey();
        }

        public void DeleteBuildByInputKey()
        {
            if (Input.GetKey(KeyCode.Delete))
            {
                OnButtonDelete();
            }
        }

        public void JsonReset(int Level)
        {
            Filename = "";
            ConfigVersion = 1;
            SavedTime = DateTime.Now;
            ExpTotal = 0;
            Gem = 0;
            Gold = 0;
            Elixir = 0;
            Shield = 0;
            TownHallLevel = Level;

            BEGround.instance.BuildingDeleteAll();
            BEUnitManager.instance.RemoveAll();
            DeckReset();

            for (int i = 0; i < TBDatabase.GetBuildingCount(); ++i)
            {
                // 根据下标，从TBDatabase中的列表Buildings，得到下标对应Building的ID
                BuildingType bt = TBDatabase.GetBuildingType(i);
                
//                if (bt.ID == 0)
//                {
//                    // add 1 Townhall
//                    DeckItemAdd(0, TownHallLevel, 1);
//                }
//                else if (bt.ID == 1)
//                {
//                    // add 2 Hut(worker)
//                    DeckItemAdd(1, 1, 5);
//                }
//                else
//                {
                // Get building's max level under current TownLevel
                int MaxLevel = 0;
                for (int j = 0; j < bt.Defs.Count; ++j)
                {
                    BuildingDef bd = bt.Defs[j];
                    if (bd.TownHallLevelRequired <= TownHallLevel)
                        MaxLevel++;
                }

                int MaxCount = bt.MaxCount[TownHallLevel - 1];

                if (MaxCount > 0)
                {
//                        if (UnShowModel.Contains(i))
//                            continue;

                    DeckItemAdd(bt.ID, MaxLevel, MaxCount);
                    LoggerHelper.Debug ("Build: i:"+i.ToString()+",MaxLevel:"+MaxLevel.ToString()+",MaxCount:"+MaxCount.ToString());
                 
                }
                
            }

            // resize content size to fit item count
            content.GetComponent<RectTransform>().sizeDelta = new Vector3(132 * Items.Count, 146);
        }
        public void Reset(int Level)
        {
            Filename = "";
            ConfigVersion = 1;
            SavedTime = DateTime.Now;
            ExpTotal = 0;
            Gem = 0;
            Gold = 0;
            Elixir = 0;
            Shield = 0;
            TownHallLevel = Level;

            BEGround.instance.BuildingDeleteAll();
            BEUnitManager.instance.RemoveAll();
            DeckReset();

            for (int i = 0; i < TBDatabase.GetBuildingCount(); ++i)
            {
                if (i == 0)
                {
                    // add 1 Townhall
                    DeckItemAdd(0, TownHallLevel, 1);
                }
                else if (i == 1)
                {
                    // add 2 Hut(worker)
                    DeckItemAdd(1, 1, 5);
                }
                else
                {
                    // Get building's max level under current TownLevel
                    int MaxLevel = 0;
                    BuildingType bt = TBDatabase.GetBuildingType(i);
                    for (int j = 0; j < bt.Defs.Count; ++j)
                    {
                        BuildingDef bd = bt.Defs[j];
                        if (bd.TownHallLevelRequired <= TownHallLevel)
                            MaxLevel++;
                    }

                    int MaxCount = bt.MaxCount[TownHallLevel - 1];

                    if (MaxCount > 0)
                    {
//                        if (UnShowModel.Contains(i))
//                            continue;

                        DeckItemAdd(i, MaxLevel, MaxCount);
                        LoggerHelper.Debug ("Build: i:"+i.ToString()+",MaxLevel:"+MaxLevel.ToString()+",MaxCount:"+MaxCount.ToString());
                     
                    }
                }
            }

            // resize content size to fit item count
            content.GetComponent<RectTransform>().sizeDelta = new Vector3(132 * Items.Count, 146);
        }
        public void DeckReset()
        {
            // remove previous Deck items
            for (int i = content.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }

            Items.Clear();
        }

        public UIEditItem DeckItemAdd(int id, int level, int count)
        {
            LoggerHelper.Debug("Build: id:"+id.ToString() +",level:"+level.ToString()+",count:"+count.ToString());
            GameObject go = (GameObject) Instantiate(prefabEditItem, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(content.transform);
            go.transform.localScale = Vector3.one;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(Items.Count * 132 + 66, 0);

            UIEditItem script = go.GetComponent<UIEditItem>();
            script.Init(TBDatabase.GetBuildingTypeByID(id), level, count);

            Items.Add(script);

            return script;
        }
        public UIEditItem DeckItemFind(int type, int level = -1)
        {
            for (int i = 0; i < Items.Count; ++i)
            {
                if (Items[i].IsMatch(type, level))
                {
                    return Items[i];
                }
            }

            return null;
        }

        public void DeckItemAddCount(int type, int level, int delta)
        {
            Debug.Log("DeckItemAddCount: type:" + type + " level:" + level);
            UIEditItem item = DeckItemFind(type, level);
            if (item != null)
            {
                item.Count += delta;
                Debug.Log("item.Count:" + item.Count);
                item.UpdateUI();
            }
        }

        public void DeckItemCountRestore()
        {
            for (int i = 0; i < Items.Count; ++i)
            {
                Items[i].Count = Items[i].CountStart;
                Items[i].UpdateUI();
            }
        }

        //BECameraRTSListner implement
        public void OnTouchDown(Ray ray)
        {
            RaycastHit hit;
            if (!bEraseMode && !bAddMode && Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "BuildingBase"))
            {
                MouseClickedBuilding = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
                // Debug.Log ("OnTouchDown MouseClickedBuilding:"+MouseClickedBuilding);
                // Debug.Log(MouseClickedBuilding.tilePos);
            }
            else
            {
                MouseClickedBuilding = null;
            }
        }

        public void OnTouchUp(Ray ray)
        {
        }


//        private Ray _ray;
//        private Vector3 hitPoint;
        
        public void OnTouch(Ray ray)
        {
//            _ray = ray;
            
            
//            Debug.Log ("OnTouch MouseClickedBuilding:"+MouseClickedBuilding);
//            Debug.Log ("OnTouch BEGround.buildingSelected:"+BEGround.buildingSelected);
//            Debug.Log(AddModelBuildingType.displayName);
            RaycastHit hit;
            // 如果当前没有建筑物被选中
            if (BEGround.buildingSelected == null)
            {
                if (bAddMode)
                {
                    if (AddModelBuildingType != null)
                    {
                        float enter;
                        BEGround.instance.xzPlane.Raycast(ray, out enter);
                        Vector3 vTarget = ray.GetPoint(enter);
                        Vector2 tileSize = new Vector2(AddModelBuildingType.TileX, AddModelBuildingType.TileZ);
                        Vector2 tilePos = BEGround.instance.GetTilePosInAddMode(vTarget, tileSize);
                        bool IsVacant = BEGround.instance.IsVacant(tilePos, tileSize);
                        if (IsVacant)
                        {
                            Building script = BEGround.instance.BuildingAdd(AddModelBuildingType.ID, 1);
                            script.Move((int)tilePos.x, (int)tilePos.y);
                            // BEGround.instance.MoveToVacantTilePos(script);
                            script.CheckLandable();
                            BEGround.instance.BuildingSelect(script);
                            script.Land(true,false);
                            script.Land(false,false);
                            
                            BEGround.instance.BuildingLandUnselect();
                        }
                    }
                    else Debug.Log("Please click the building type.");
                }
                else 
                {
                    // 如果点击到了BuildingBase
                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "BuildingBase")
                    {
                        // hitPoint = hit.point;
                        Building buildingNew = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
    
                        if (bEraseMode)
                        {
                            if (ItemCountByTownHallLevel) DeckItemAddCount(buildingNew.bt.ID, -1, 1);
                            else DeckItemAddCount(buildingNew.bt.ID, buildingNew.Level, 1);
    
                            BEGround.instance.BuildingDelete(buildingNew);
                        }
                        else if (bMultiChooseMode)
                        {
                            BEGround.instance.BuildingMultiSelect(buildingNew);
                        }
                        else
                        {
                            BEGround.instance.BuildingSelect(buildingNew);
    
                            // remember last selected wall building
                            if (buildingNew.bt.Main_type == 1)
                                lastSelectedWall = buildingNew;
                        }                    
                    }
                    // 用于在复选模式中，点击空地，清空所有选中物体，退出多选模式
                    else
                    {
                        if (bMultiChooseMode)
                        {
                            foreach (Building bd in BEGround.buildingMultiSelected)
                            {
                                BEGround.instance.BuildingLandUnselectWithArg(bd);
                            }
                            BEGround.buildingMultiSelected.Clear();
                            if (!bEnterMultiChooseModeByClick)
                                GameObject.Find("ToggleMultiChooseMode").GetComponent<Toggle>().isOn = false;

                        }
                    }
                }
            }
            else
            {
                if (MouseClickedBuilding != BEGround.buildingSelected)
                    BEGround.instance.BuildingLandUnselect();

                if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "BuildingBase"))
                {
                    Building buildingNew = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
                    BEGround.instance.BuildingSelect(buildingNew);

                    // remember last selected wall building
                    if (buildingNew.bt.Main_type == 1)
                        lastSelectedWall = buildingNew;
                }
            }
//            Debug.Log ("OnTouch MouseClickedBuilding:"+MouseClickedBuilding);
//            Debug.Log ("OnTouch BEGround.buildingSelected:"+BEGround.buildingSelected);
        }

//        private void OnDrawGizmos()
//        {
//            Gizmos.color = Color.red;
//            //Gizmos.DrawRay(_ray);
//            Gizmos.DrawLine(_ray.origin, hitPoint);
//        }

        public void OnDragStart(Ray ray)
        {
            if (bMultiChooseMode)
            {
                if ((BEGround.buildingMultiSelected.Any()) &&
                    (BEGround.buildingMultiSelected.Contains(MouseClickedBuilding)))
                {
                    BECameraRTS.instance.camPanningUse = false;
                    BECameraRTS.instance.InertiaUse = false;
                    BETween.alpha(ground.gameObject, 0.1f, 0.0f, 0.3f);
                }
            }
            else
            {
                if ((BEGround.buildingSelected != null) && (MouseClickedBuilding == BEGround.buildingSelected))
                {
                    BECameraRTS.instance.camPanningUse = false;
                    BECameraRTS.instance.InertiaUse = false;
    
                    //Debug.Log ("OnDragStart MouseClickedBuilding:"+MouseClickedBuilding);
                    //Debug.Log ("OnDragStart BEGround.buildingSelected:"+BEGround.buildingSelected);
                    if (MouseClickedBuilding == BEGround.buildingSelected)
                    {
                        BETween.alpha(ground.gameObject, 0.1f, 0.0f, 0.3f);
                        //Debug.Log ("ground alpha to 0.1");
                    }
                }
            }

        }

        public void OnDrag(Ray ray)
        {
            if (bEraseMode)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "BuildingBase"))
                {
                    // 固定摄像机
                    BECameraRTS.instance.camPanningUse = false;
                    BECameraRTS.instance.InertiaUse = false;
                    
                    Building buildingNew = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
                    if (ItemCountByTownHallLevel) DeckItemAddCount(buildingNew.bt.ID, -1, 1);
                    else DeckItemAddCount(buildingNew.bt.ID, buildingNew.Level, 1);

                    BEGround.instance.BuildingDelete(buildingNew);
                    
                }
                else
                {
                    
                }
            }
            else if (bAddMode)
            {
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit) && AddModelBuildingType != null)
                {
                    BECameraRTS.instance.camPanningUse = false;
                    BECameraRTS.instance.InertiaUse = false;
                    float enter;
                    BEGround.instance.xzPlane.Raycast(ray, out enter);
                    Vector3 vTarget = ray.GetPoint(enter);
                    Vector2 tileSize = new Vector2(AddModelBuildingType.TileX, AddModelBuildingType.TileZ);
                    Vector2 tilePos = BEGround.instance.GetTilePosInAddMode(vTarget, tileSize);
                    bool IsVacant = BEGround.instance.IsVacant(tilePos, tileSize);
                    if (IsVacant)
                    {
                        Building script = BEGround.instance.BuildingAdd(AddModelBuildingType.ID, 1);
                        script.Move((int)tilePos.x, (int)tilePos.y);
                        // BEGround.instance.MoveToVacantTilePos(script);
                        script.CheckLandable();
                        BEGround.instance.BuildingSelect(script);
                        script.Land(true,false);
                        script.Land(false,false);
                            
                        BEGround.instance.BuildingLandUnselect();
                    }
                        
                        
//                    Debug.Log(vTarget);
//                        Debug.Log(tileSize);
//                        Debug.Log(tilePos);
                }
            }
            else if (bMultiChooseMode)
            {
                if ((BEGround.buildingMultiSelected.Any()) && (BEGround.buildingMultiSelected.Contains(MouseClickedBuilding) ))
                {
                    BECameraRTS.instance.camPanningUse = false;
                    BECameraRTS.instance.InertiaUse = false;
   
                    float enter = 0.0f;
                    BEGround.instance.xzPlane.Raycast(ray, out enter);
                    
                    // 统一使用世界坐标
                    Vector3 mouseClickedBuildingPos = BEGround.instance.TilePosToWorldPos(MouseClickedBuilding.tilePos, 
                        MouseClickedBuilding.tileSize);
                    
                    List<Vector3> targetPosList = new List<Vector3>();
                    foreach (Building bd in BEGround.buildingMultiSelected)
                    {
                        Vector3 relativePos = BEGround.instance.TilePosToWorldPos(bd.tilePos, bd.tileSize) - mouseClickedBuildingPos;
                        Vector3 targetPos = ray.GetPoint(enter);
                        targetPos.x += relativePos.x;
                        targetPos.z += relativePos.z;
                        
                        // 合法性检查
                        if (targetPos.x <= (BEGround.instance.GridSize.x-bd.tileSize.x + 1) * 0.5f &&
                            targetPos.x >= (BEGround.instance.GridSize.x-bd.tileSize.x) * -0.5f &&
                            targetPos.z <= (BEGround.instance.GridSize.y-bd.tileSize.y + 1) * 0.5f &&
                            targetPos.z >= (BEGround.instance.GridSize.y-bd.tileSize.y) * -0.5f)
                            targetPosList.Add(targetPos);
                        else break;
                    }

                    if (targetPosList.Count != BEGround.buildingMultiSelected.Count) return;
                    for (int i = 0; i < targetPosList.Count; i++)
                    {
                        BEGround.buildingMultiSelected[i].Move(targetPosList[i]);
                        BEGround.buildingMultiSelected[i].CheckLandable();
                        Color gridColor = BEGround.buildingMultiSelected[i].Landable ? Color.green : Color.red;
                        BEUtil.SetObjectColor(BEGround.buildingMultiSelected[i].goGrid, gridColor);
                    }
                    
                } 
            }            
            else
            {
               if ((BEGround.buildingSelected != null) && (MouseClickedBuilding == BEGround.buildingSelected))
               {
                   BECameraRTS.instance.camPanningUse = false;
                   BECameraRTS.instance.InertiaUse = false;
   
                   float enter = 0.0f;
                   BEGround.instance.xzPlane.Raycast(ray, out enter);
                   BEGround.buildingSelected.Move(ray.GetPoint(enter));
                   BEGround.buildingSelected.CheckLandable();
                   Color gridColor = BEGround.buildingSelected.Landable ? Color.green : Color.red;
                   BEUtil.SetObjectColor(BEGround.buildingSelected.goGrid, gridColor);
                   
   
   
                   // remember last selected wall building
                   if (BEGround.buildingSelected.bt.Main_type == 1)
                       lastSelectedWall = BEGround.buildingSelected;
               } 
            }

        }

        public void OnDragEnd(Ray ray)
        {
            if (bEraseMode || bAddMode)
            {
                BECameraRTS.instance.camPanningUse = true;
                BECameraRTS.instance.InertiaUse = true;
            }
            else if (bMultiChooseMode)
            {
                if (BEGround.buildingMultiSelected.Any())
                {
                    BECameraRTS.instance.camPanningUse = true;
                    BECameraRTS.instance.InertiaUse = true;
                    
                    if (BEGround.buildingMultiSelected.Contains(MouseClickedBuilding))
                    {
                        BETween.alpha(ground.gameObject, 0.1f, 0.3f, 0.0f);
                        BEGround.instance.BuildingMultiLand();
                    }
                }
            }
            else
            {
                if (BEGround.buildingSelected != null)
                {
                    BECameraRTS.instance.camPanningUse = true;
                    BECameraRTS.instance.InertiaUse = true;
    
                    //Debug.Log ("OnDragEnd MouseClickedBuilding:"+MouseClickedBuilding);
                    //Debug.Log ("OnDragEnd BEGround.buildingSelected:"+BEGround.buildingSelected);
                    if (MouseClickedBuilding == BEGround.buildingSelected)
                    {
                        BETween.alpha(ground.gameObject, 0.1f, 0.3f, 0.0f);
    
                        if (BEGround.buildingSelected.Landable && BEGround.buildingSelected.OnceLanded)
                            BEGround.instance.BuildingLand(); //BuildingLandUnselect();
                    }
                }
    
                MouseClickedBuilding = null;
            }

        }

        public void OnLongPress(Ray ray)
        {
            //Debug.Log ("OnLongPress MouseClickedBuilding:"+MouseClickedBuilding);
            RaycastHit hit;
            if (BEGround.buildingSelected == null && !bEraseMode)
            {
                if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "BuildingBase"))
                {
                    Building buildingNew = BEGround.instance.BuildingFromObject(hit.collider.gameObject);
                    BEGround.instance.BuildingSelect(buildingNew);

                    // remember last selected wall building
                    if (buildingNew.bt.Main_type == 1)
                        lastSelectedWall = buildingNew;
                }
            }
        }

        public void OnMouseWheel(float value)
        {
        }

        //SceneBase functions
        public override void GainExp(int exp)
        {
        }

        public override void Save()
        {
        }

        public override void Load()
        {
        }

        //ui event functions
        public void OnButtonNew()
        {
            UIDialogNew.Show(true);
        }

        public void OnButtonEdit()
        {
            UIDialogNew.Show(false);
        }

        public void OnButtonOpen()
        {
            UIDialogFile.Show(strMapsPath, "*.txt");
        }

        public void OnButtonPlay()
        {
            SceneManager.LoadScene("CocScene");
        }

        public void OnButtonSaveJson()
        {
//			if(BEGround.buildingSelected != null) {
//				UIDialogMessage.Show("All buildings must be landed before saving", "Ok", "Warning");
//				return;
//			}
//			if(BEGround.buildingSelected != null) {
//				UIDialogMessage.Show("All buildings must be landed before saving", "Ok", "Warning");
//				return;
//			}
            if (Filename.Length == 0)
            {
                UIDialogMessage.Show("Create map before Saving", "Ok", "Warning");
                return;
            }
            
            Transform trDecoRoot = BEGround.instance.trDecoRoot;
            List<MapItem> items = new List<MapItem>();

            // first save town hall
            // because the townhall's level limits other building's count and max level. 
            foreach (Transform child in trDecoRoot)
            {
                Building script = child.gameObject.GetComponent<Building>();
                if (!script.Landed)
                {
                    UIDialogMessage.Show("Floating building(s) detected", "Ok", "Warning");
                    return;
                }
            }

            //flowField.Save("test");
            if (!Filename.EndsWith(".json"))
                Filename += ".json";
            string jsonFilePath = strMapsPath + "/" + Filename + ".txt"; //BEUtil.pathForDocumentsFile(Filename+".xml");

            Stage stage = new Stage();
            stage.StageName = new MapConfig();
            stage.StageName.Size = 100;

            
            // StageName = 关卡id


            // first save town hall
            // because the townhall's level limits other building's count and max level. 
            foreach (Transform child in trDecoRoot)
            {
                Building script = child.gameObject.GetComponent<Building>();
                if (script != null)
                {  
                    //script.Save (xmlDocument);
                    items.Add(new MapItem
                    {
                        ID = script.Type,
                        X = (int) script.tilePos.x,
                        Y = (int) script.tilePos.y
                    });
                }
            }

            stage.StageName.Building = items.ToArray();
            var json = JsonConvert.SerializeObject(stage);
            File.WriteAllText(jsonFilePath, json);


            UIDialogMessage.Show("Mapname " + Filename + ".txt Saved", "Ok", "Save");
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public void OnButtonSaveAsJson()
        {
//			if(BEGround.buildingSelected != null) {
//				UIDialogMessage.Show("All buildings must be landed before saving", "Ok", "Warning");
//				return;
//			}
//			if(BEGround.buildingSelected != null) {
//				UIDialogMessage.Show("All buildings must be landed before saving", "Ok", "Warning");
//				return;
//			}
            if (Filename.Length == 0)
            {
                UIDialogMessage.Show("Create map before Saving", "Ok", "Warning");
                return;
            }

            UIDialogSaveAs.Show(true);
        }

        public void OnButtonSave()
        {
//			if(BEGround.buildingSelected != null) {
//				UIDialogMessage.Show("All buildings must be landed before saving", "Ok", "Warning");
//				return;
//			}
//			if(BEGround.buildingSelected != null) {
//				UIDialogMessage.Show("All buildings must be landed before saving", "Ok", "Warning");
//				return;
//			}
            if (Filename.Length == 0)
            {
                UIDialogMessage.Show("Create map before Saving", "Ok", "Warning");
                return;
            }

            //flowField.Save("test");
            string xmlFilePath = strMapsPath + "/" + Filename + ".xml"; //BEUtil.pathForDocumentsFile(Filename+".xml");
            XmlDocument xmlDocument = new XmlDocument();
            // StageName = 关卡id
            xmlDocument.LoadXml("<StageName><name>wrench</name></StageName>");
            {
                xmlDocument.DocumentElement.RemoveAll();
                Transform trDecoRoot = BEGround.instance.trDecoRoot;

                // first save town hall
                // because the townhall's level limits other building's count and max level. 
                foreach (Transform child in trDecoRoot)
                {
                    Building script = child.gameObject.GetComponent<Building>();
                    if ((script != null) && (script.bt.ID == 0))
                    {
                        script.Save(xmlDocument);
                    }
                }

                //second save buildings except townhall
                foreach (Transform child in trDecoRoot)
                {
                    Building script = child.gameObject.GetComponent<Building>();
                    if ((script != null) && (script.bt.ID != 0))
                    {
                        script.Save(xmlDocument);
                    }
                }

                // ####### Encrypt the XML ####### 
                // If you want to view the original xml file, turn of this piece of code and press play.
                if (xmlDocument.DocumentElement.ChildNodes.Count >= 1)
                {
                    //if(UseEncryption) {
                    //	string data = BEUtil.Encrypt (xmlDocument.DocumentElement.InnerXml);
                    //	xmlDocument.DocumentElement.RemoveAll ();
                    //	xmlDocument.DocumentElement.InnerText = data;
                    //}
                    xmlDocument.Save(xmlFilePath);
                }

                // ###############################
            }

            UIDialogMessage.Show("Mapname " + Filename + ".xml Saved", "Ok", "Save");

#if UNITY_EDITOR
            //UnityEditor.AssetDatabase.Refresh ();
#endif

            LoadXmlFile(xmlFilePath);
        }

        public void OnButtonLoadJson(string filename)
        {
            string jsonFilePath = strMapsPath + "/" + filename + ".txt";

            if (!File.Exists(jsonFilePath))
            {
                UIDialogMessage.Show(filename + ".txt Not found", "Ok", "Error");
                return;
            }

            BEGround.instance.BuildingDeleteAll();
            BEUnitManager.instance.RemoveAll();
            DeckReset();
            Filename = filename;
            InLoading = true;

            var json = File.ReadAllText(jsonFilePath);
            var stage = JsonConvert.DeserializeObject<Stage>(json);

            foreach (var building in stage.StageName.Building)
            {
                //Building script = BEGround.instance.BuildingAdd(building.ID, 1);
                Building script = BEGround.instance.BuildingAdd(building.ID, 1);
                script.LoadJson(building);
            }

            // int BuildingTypeCount = TBDatabase.instance.Buildings.Count;
            int BuildingTypeCount = TBDatabase.GetBuildingCount();
            for (int i = 0; i < BuildingTypeCount; ++i)
            {
                BuildingType bt = TBDatabase.GetBuildingType(i);
//                if (bt.ID == 0)
//                {
//                    DeckItemAdd(i, 1, 1);
//                }
//                else
//                {
                int MaxLevel = bt.GetMaxLevleByTownLevel(TownHallLevel);
                int MaxCount = bt.MaxCount[TownHallLevel];
//                if (i == 1) MaxCount = 5;

//                    if (UnShowModel.Contains(i)) //暂不展示3d模型
//                        continue;

                DeckItemAdd(bt.ID, MaxLevel, MaxCount);
//                }
            }


            InLoading = false;

            //set resource's capacity
//            BEGround.instance.CapacityCheck();
            UpdateUI();
            // resize content size to fit item count
            content.GetComponent<RectTransform>().sizeDelta = new Vector3(132 * Items.Count, 146);
        }

        /*public void OnButtonLoad(string filename)
        {
            string xmlFilePath = strMapsPath + "/" + filename + ".xml";

            if (!File.Exists(xmlFilePath))
            {
                UIDialogMessage.Show(filename + ".xml Not found", "Ok", "Error");
                return;
            }

            BEGround.instance.BuildingDeleteAll();
            BEUnitManager.instance.RemoveAll();
            DeckReset();
            Filename = filename;
            InLoading = true;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);

            // ####### Encrypt the XML ####### 
            // If the Xml is encrypted, so this piece of code decrypt it.
            if (xmlDocument.DocumentElement.ChildNodes.Count <= 1)
            {
                //if(UseEncryption) {
                //	string data = BEUtil.Decrypt(xmlDocument.DocumentElement.InnerText);
                //	xmlDocument.DocumentElement.InnerXml = data;
                //}
            }
            //################################


            if (xmlDocument != null)
            {
                XmlElement element = xmlDocument.DocumentElement;
                XmlNodeList list = element.ChildNodes;
                foreach (XmlElement ele in list)
                {
                    if (ele.Name == "Building")
                    {
                        int Type = int.Parse(ele.GetAttribute("ID"));
                        //int Level = int.Parse(ele.GetAttribute("Level"));	
                        //Debug.Log ("Building Type:"+Type.ToString()+" Level:"+Level.ToString());
                        int Level = 1;
                        Building script = BEGround.instance.BuildingAdd(Type, Level);
                        script.Load(ele);


                        if (Type == 0)
                        {
                            TownHallLevel = Level;

                            if (ItemCountByTownHallLevel)
                            {
                                int BuildingTypeCount = TBDatabase.instance.Buildings.Count;
                                for (int i = 0; i < BuildingTypeCount; ++i)
                                {
                                    if (i == 0)
                                    {
                                        DeckItemAdd(i, Level, 1);
                                    }
                                    else
                                    {
                                        BuildingType bt = TBDatabase.GetBuildingType(i);
                                        int MaxLevel = bt.GetMaxLevleByTownLevel(TownHallLevel);
                                        int MaxCount = bt.MaxCount[TownHallLevel];
                                        if (i == 1) MaxCount = 5;

                                        if (UnShowModel.Contains(i)) //暂不展示3d模型
                                            continue;

                                        DeckItemAdd(i, MaxLevel, MaxCount);
                                    }
                                }
                            }
                        }

                        UIEditItem item = null;
                        if (ItemCountByTownHallLevel)
                        {
                            item = DeckItemFind(Type, -1);
                            if (item != null)
                            {
                                item.Count -= 1;
                                item.UpdateUI();
                            }
                        }
                        else
                        {
                            item = DeckItemFind(Type, Level);
                            if (item == null)
                            {
                                item = DeckItemAdd(Type, Level, 1);
                            }
                            else
                            {
                                item.CountStart++;
                            }

                            item.Count = 0;
                            item.UpdateUI();
                        }
                    }
                    else
                    {
                    }
                }
            }

            InLoading = false;

            //set resource's capacity
            BEGround.instance.CapacityCheck();
            UpdateUI();
            // resize content size to fit item count
            content.GetComponent<RectTransform>().sizeDelta = new Vector3(132 * Items.Count, 146);
        }*/

        public void OnButtonCancel()
        {
            //flowField.Save("test");
        }
     

        public void OnButtonRemoveAll()
        {
            BEGround.instance.BuildingDeleteAll();
            BEUnitManager.instance.RemoveAll();
            DeckItemCountRestore();
        }
        
        public void OnButtonMultiChooseMode(bool value)
        {
            bMultiChooseMode = value;
            bEnterMultiChooseModeByClick = value;
            textMultiChooseMode.text = bMultiChooseMode ? "Multi-Choose \nOn" : "Multi-Choose \nOff";
            BEGround.instance.BuildingLandUnselect();
            foreach (Building bd in BEGround.buildingMultiSelected)
            {
                BEGround.instance.BuildingLandUnselectWithArg(bd);
            }
            BEGround.buildingMultiSelected.Clear();
        }

        public void OnButtonEraseMode(bool value)
        {
            bEraseMode = value;
            textEraseMode.text = bEraseMode ? "Erase Mode \nOn" : "Erase Mode \nOff";
            BEGround.instance.BuildingLandUnselect();
        }

        public void OnButtonAddMode(bool value)
        {
            bAddMode = value;
            textAddMode.text = bAddMode ? "Add Mode \nOn" : "Add Mode \nOff";
            BEGround.instance.BuildingLandUnselect();
            AddModelBuildingType = null;
        }

        public void OnButtonDelete()
        {
            if (BEGround.buildingSelected != null)
            {
                DeckItemAddCount(BEGround.buildingSelected.bt.ID, BEGround.buildingSelected.Level, 1);
                BEGround.instance.BuildingDelete(BEGround.buildingSelected);
            }
        }
        
        public void OnButtonSelectLongestWall()
        {
            Building originalBuilding = BEGround.buildingSelected;
            
            Toggle t = GameObject.Find("ToggleMultiChooseMode").GetComponent<Toggle>();
            t.isOn = true;
            bEnterMultiChooseModeByClick = false;
//            OnButtonMultiChooseMode(true);
            List <Building> wallsInX = new List<Building>();
            List <Building > wallsInY = new List<Building>();
            wallsInX.Add(originalBuilding);
            wallsInY.Add(originalBuilding);
            // 遍历x方向
            for (int x = (int)originalBuilding.tilePos.x - 1; x >= 0; x--)
            {
                Building temp = BEGround.instance.GetBuilding(x, (int) originalBuilding.tilePos.y);
                if (temp && temp.bt.Main_type == 1)
                {
                    wallsInX.Add(temp);
                }
                else
                {
                    break;
                }
            }
            for (int x = (int)originalBuilding.tilePos.x + 1; x <= BEGround.instance.GridSize.x; x++)
            {
                Building temp = BEGround.instance.GetBuilding(x, (int) originalBuilding.tilePos.y);
                if (temp && temp.bt.Main_type == 1)
                {
                    wallsInX.Add(temp);
                }
                else
                {
                    break;
                }
            }
            
            // 遍历y方向
            for (int y = (int)originalBuilding.tilePos.y - 1; y >= 0; y--)
            {
                Building temp = BEGround.instance.GetBuilding((int) originalBuilding.tilePos.x, y);
                if (temp && temp.bt.Main_type == 1)
                {
                    wallsInY.Add(temp);
                }
                else
                {
                    break;
                }
            }
            for (int y = (int)originalBuilding.tilePos.y + 1; y <= BEGround.instance.GridSize.y; y++)
            {
                Building temp = BEGround.instance.GetBuilding((int) originalBuilding.tilePos.x, y);
                if (temp && temp.bt.Main_type == 1)
                {
                    wallsInY.Add(temp);
                }
                else
                {
                    break;
                }
            }

            wallsInX = wallsInX.Count >= wallsInY.Count ? wallsInX : wallsInY;
            foreach (Building bd in wallsInX)
            {
                BEGround.instance.BuildingMultiSelect(bd);
            }

        }

        public void LoadXmlFile(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            StringBuilder sbJSON = new StringBuilder();
            sbJSON.Append("{");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
            sbJSON.Append("}");

            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            fileName = fileName.Substring(0, fileName.IndexOf("."));
            string destPath = Application.dataPath + "/_Res/Map/" + fileName + ".json.txt";

            FileStream fs = new FileStream(destPath, FileMode.Create);
            if (fs != null)
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(sbJSON.ToString());
                sw.Flush();
                sw.Close();
                fs.Close();
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }
        }

        private void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            if (showNodeName)
                sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
            sbJSON.Append("{");

            SortedList childNodeNames = new SortedList();

            if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    StoreChildNode(childNodeNames, attr.Name, attr.InnerText);

            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode is XmlText)
                    StoreChildNode(childNodeNames, "value", cnode.InnerText);
                else if (cnode is XmlElement)
                    StoreChildNode(childNodeNames, cnode.Name, cnode);
            }

            foreach (string childname in childNodeNames.Keys)
            {
                ArrayList alChild = (ArrayList) childNodeNames[childname];
                if (alChild.Count == 1)
                    OutputNode(childname, alChild[0], sbJSON, true);
                else
                {
                    sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                    foreach (object Child in alChild)
                        OutputNode(childname, Child, sbJSON, false);
                    sbJSON.Remove(sbJSON.Length - 2, 2);
                    sbJSON.Append(" ], ");
                }
            }

            sbJSON.Remove(sbJSON.Length - 2, 2);
            sbJSON.Append(" }");
        }

        private void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
        {
            if (nodeValue is XmlElement)
            {
                XmlNode cnode = (XmlNode) nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    XmlNodeList children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText) (children[0])).InnerText;
                }
            }

            object oValuesAL = childNodeNames[nodeName];
            ArrayList ValuesAL;
            if (oValuesAL == null)
            {
                ValuesAL = new ArrayList();
                childNodeNames[nodeName] = ValuesAL;
            }
            else
                ValuesAL = (ArrayList) oValuesAL;

            ValuesAL.Add(nodeValue);
        }

        private void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                string sChild = (string) alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
                XmlToJSONnode(sbJSON, (XmlElement) alChild, showNodeName);

            sbJSON.Append(", ");
        }

        private string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int) ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }

                sbOut.Append(ch);
            }

            return sbOut.ToString();
        }

        private void StoreChildNode(IDictionary childNodeNames, string nodeName, object nodeValue)
        {
            ArrayList list2;
            if (nodeValue is XmlElement)
            {
                XmlNode node = (XmlNode) nodeValue;
                if (node.Attributes.Count == 0)
                {
                    XmlNodeList childNodes = node.ChildNodes;
                    if (childNodes.Count == 0)
                    {
                        nodeValue = null;
                    }
                    else if ((childNodes.Count == 1) && (childNodes[0] is XmlText))
                    {
                        nodeValue = childNodes[0].InnerText;
                    }
                }
            }

            object obj2 = childNodeNames[nodeName];
            if (obj2 == null)
            {
                list2 = new ArrayList();
                childNodeNames[nodeName] = list2;
            }
            else
            {
                list2 = (ArrayList) obj2;
            }

            list2.Add(nodeValue);
        }


        const float MAX_SET_TIME = 5 * 60;

        void autoSaveMap()
        {
            if (Filename.Length == 0)
                return;
            if ((Time.time - workTime) > MAX_SET_TIME)
            {
                LoggerHelper.Debug("Map save:"+Filename+" is saved.");
                OnButtonSaveJson();
                workTime = Time.time;
            }
        }

        private uint m_leaveTime;

        void OnApplicationFocus(bool isFocus)
        {
            if (isFocus)
            {
                if (m_leaveTime == 0)
                    return;

                uint curTime = GetUnixTime() - m_leaveTime;
                if (curTime > MAX_SET_TIME)
                {
                    OnButtonSaveJson();
                    LoggerHelper.Debug("application Focus Map save:"+Filename+" is saved.");
                }

                m_leaveTime = 0;
            }
            else
            {
                m_leaveTime = GetUnixTime();
            }
        }

        private uint GetUnixTime()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            uint time = (uint) (DateTime.Now - startTime).TotalSeconds;
            return time;
        }
    }
}