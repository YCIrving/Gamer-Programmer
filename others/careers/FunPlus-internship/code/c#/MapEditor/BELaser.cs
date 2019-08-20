using UnityEngine;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BELaser
///   Description:    implement laser effect
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-08-30)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class BELaser : MonoBehaviour {

		private Transform 		tr;
		private RaycastHit 		hitInfo;
		private LineRenderer [] lineR;
		public  Vector2 		uvAnimationRate = new Vector2(1.0f, 0.0f);
		private Vector2 		uvOffset = Vector2.zero;
		public  float 			distMax = 200.0f;
		public  bool 			autoFade = false;			// set auto fade 
		public  float 			autoFadeTime = 1.0f;		// fade time
		public	Color			laserColor=Color.red;		// laser color

		void Awake(){
			tr = transform;
			lineR = new LineRenderer [2];
			lineR[0]=transform.Find ("Line").GetComponent<LineRenderer>();
			lineR[1]=transform.Find ("Line2").GetComponent<LineRenderer>();
		}

		void Update() {
			Physics.Raycast(tr.position, tr.forward, out hitInfo);
			float fTexScale = 0.01f;
			// Cast a ray to find out the end point of the laser
			if (hitInfo.transform) {
				for(int i=0 ; i < lineR.Length ; ++i) {
					lineR[i].SetPosition (1, (hitInfo.distance * Vector3.forward) * 1.0f);
					lineR[i].GetComponent<Renderer>().material.mainTextureScale = new Vector2(fTexScale * (hitInfo.distance),1.0f);
				}
			}
			else {
				for(int i=0 ; i < lineR.Length ; ++i) {
					lineR[i].SetPosition (1, (distMax * Vector3.forward));
					lineR[i].GetComponent<Renderer>().material.mainTextureScale = new Vector2(fTexScale * distMax,1.0f);	
				}
			}

			uvOffset -= (uvAnimationRate * Time.deltaTime);
			for(int i=0 ; i < lineR.Length ; ++i) {
				lineR[i].materials[0].SetTextureOffset("_MainTex", uvOffset);
			}
		}

		void OnEnable(){
			StartCoroutine(Fade());
		}
		
		IEnumerator Fade(){
			float duration=0.0f;
			/*lineR[0].materials[0].SetColor("_TintColor", new Color(laserColor.r, laserColor.g, laserColor.b, .5f));
			lineR[1].materials[0].SetColor("_TintColor", new Color(1, 1, 1, .5f));*/

			while(duration<1.0f){
				/*lineR[0].materials[0].SetColor("_TintColor", new Color(laserColor.r, laserColor.g, laserColor.b, (1f-duration)/2));
				lineR[1].materials[0].SetColor("_TintColor", new Color(1, 1, 1, (1f-duration)/2));*/

				duration+=Time.fixedDeltaTime*4.0f;
				yield return new WaitForSeconds(Time.fixedDeltaTime);
			}

			// unspawn to object pool
			BEObjectPool.Unspawn(gameObject);
		}
	}

}