using UnityEngine;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEMuzzleFlash
///   Description:    set random scale * rotation to gameobject
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-08-30)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class BEMuzzleFlash : MonoBehaviour {

		Transform 	tr;

		void Awake () {
			tr = transform;
		}
		
		void Update () {
			if(Time.timeScale > 0.01f) {
				tr.localScale = Vector3.one * Random.Range(0.5f,1.5f);
				tr.localEulerAngles = new Vector3(0,0,Random.Range(0.0f,90.0f));
			}
		}
	}

}