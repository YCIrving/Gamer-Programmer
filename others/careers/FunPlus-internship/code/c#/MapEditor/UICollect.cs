using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UICollect
///   Description:    class for shows gained resource count
///                   when user clicked building's collect dialog
///                   show numbers of resource count
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UICollect : MonoBehaviour {
		
		public 	CanvasGroup groupRoot;
		public 	Text 		Name;

		private Transform 	tr;
		private Vector3 	vSpeed = new Vector3(0,3,0);
		private float 		fLife = 1.0f;
		private float 		fAge;

		public 	Transform 	trObject;
		public 	Vector3 	vOffset;

		void Awake () {
			tr = transform;
		}
		
		void Start () {
			
		}
		
		void Update () {

			UpdateMovement(Time.deltaTime);
		}

		void UpdateMovement(float deltaTime) {
			fAge += deltaTime;

			// if life is over, unspawn
			if(fAge > fLife) {
				BEObjectPool.Unspawn(gameObject);
				return;
			}

			// set alpha by life ratio
			float fRatio = fAge / fLife;
			float fAlpha = Mathf.Clamp((1.0f-fRatio)*3.0f, 0.0f, 1.0f);
			groupRoot.alpha = fAlpha;

			// keep move up
			vOffset += vSpeed * deltaTime;
			Vector3 vWorldPos = trObject.position+vOffset;
			Vector3 vScreenPos = Camera.main.WorldToScreenPoint(vWorldPos);
			vScreenPos.z = 0;
			tr.position = vScreenPos;
		}

		public void Init(Transform trTarget, Vector3 offset) {
			trObject = trTarget;
			vOffset = offset;
			fAge = 0.0f;

			UpdateMovement(0.0f);
		}
	}
	
}