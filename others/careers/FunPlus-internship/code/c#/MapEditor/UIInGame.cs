using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIInGame
///   Description:    class for create & manage ingame ui (health bar)
///   Usage :		  UIInGame.instance.AddHealthBar(prefabHealthBar, goTarget, new Vector3(0,1,0));
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-08-30)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	// struct for sort ui component by camera z distance
	public class InGameUI {
		public  Transform	trObject; 	//object transforn in world space
		public  Vector3  	vOffset;	//offset from trObject
		public  Transform	trUI;		//ui transform
		public	float		depth;		//z depth in camera space
	}
	
	public class UIInGame : MonoBehaviour {

		public static UIInGame instance;
		
		public  Transform 		InGameUIRoot = null;
		private List<InGameUI> 	InGameUIs = new List<InGameUI>();

		void Awake () {
			instance=this;
		}

		void Start () {
		
		}
		
		void Update () {
			SortInGameUI();
		}

		public void SortInGameUI() {
			for (int i = 0; i < InGameUIs.Count; i++) {
				Vector3 vWorldPos = InGameUIs[i].trObject.position+new Vector3(0,1.0f,0);
				Vector3 vScreenPos = Camera.main.WorldToScreenPoint(vWorldPos);
				vScreenPos.z = 0;
				InGameUIs[i].trUI.position = vScreenPos;
				
				float distance = (vWorldPos - Camera.main.transform.position).magnitude;
				InGameUIs[i].depth = -distance;
			}
			
			InGameUIs.Sort((x, y) => x.depth.CompareTo(y.depth));
			for (int i = 0; i < InGameUIs.Count; i++) {
				InGameUIs[i].trUI.SetSiblingIndex(i);
			}
		}
		
		// add healthbar ui 
		public GameObject AddInGameUI(GameObject prefab, Transform trObject, Vector3 vOffset) {
			
			InGameUI newDU = new InGameUI();
			newDU.trObject = trObject;
			newDU.vOffset = vOffset;
			newDU.trUI = BEObjectPool.Spawn (prefab, InGameUIRoot, new Vector3(1000,1000,1000), Quaternion.identity).transform;
			newDU.depth = 0;
			InGameUIs.Add (newDU);
			newDU.trUI.localScale = new Vector3(1,1,1);
			
			return newDU.trUI.gameObject;
		}
		
		public void RemoveInGameUI(Transform trObject) {
			int idx = InGameUIs.FindIndex(x => x.trObject==trObject);
			if(idx != -1) {
				InGameUI newDU = InGameUIs[idx];
				BEObjectPool.Unspawn (newDU.trUI.gameObject);
				InGameUIs.RemoveAt(idx);
				newDU = null;
			}
		}
	}

}
