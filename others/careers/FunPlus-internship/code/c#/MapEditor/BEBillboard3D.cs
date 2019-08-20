using UnityEngine;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEBillboard3D
///   Description:    rotate transform to facing camera
///   Usage :		  just add this script to any game object
///                   to revise with position, set UsePosition value to true 
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class BEBillboard3D : MonoBehaviour {

		public bool UsePosition = false;
		Camera cam;
			
		void Start() {
			cam = Camera.main;
		}

		void Update() {

			if(UsePosition) {
				Vector3 vDir = cam.transform.position - transform.position;
				vDir.Normalize();
				transform.rotation = Quaternion.LookRotation(-vDir);
			}
			else {
				transform.rotation = cam.transform.rotation;
			}
		}
	}

}