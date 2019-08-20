using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BETime
///   Description:    classes to manage time related works
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class BETime : MonoBehaviour {

		public	static BETime instance;

		public  static double 	timeAfterLastRun=0;
		public 	static float	deltaTime;
		public 	static float	PausedTime = 0.0f;
		private static DateTime	pausedTime;	
		private	static bool		bInitialized = false;


		void Awake() {
			instance=this;
		}
		
		void Start () {
			PausedTime = 0.0f;
			bInitialized = true;
		}
		
		void Update () {

			deltaTime = Time.deltaTime;
			//Debug.Log ("BETime::Update start deltaTime:"+deltaTime);

			// if puase time exist, apply to deltatime
			if(PausedTime > 0.01f) {
				deltaTime += PausedTime;
				PausedTime = 0.0f;
				//Debug.Log ("BETime::Update PausedTime:"+PausedTime);
			}

			// if time between last played time to curren time exist, apply that value
			if(timeAfterLastRun > 0.001) {
				deltaTime += (float)timeAfterLastRun;
				timeAfterLastRun = 0;
			}
			//Debug.Log ("BETime::Update end deltaTime:"+deltaTime);
		}

		// when application in lost focus and recover it,
		// check times between them, andd apply this time at Scenetown's update
		public void OnApplicationPause(bool paused) {

			//Debug.Log ("OnApplicationPause "+paused.ToString());
			if(paused) {
				pausedTime = DateTime.Now;
			} 
			else {
				if(bInitialized) {
					DateTime dtNow = DateTime.Now;	
					TimeSpan timeDelta = dtNow.Subtract(pausedTime);	
					PausedTime = (float)timeDelta.TotalSeconds;
					//Debug.Log ("PausedTime "+PausedTime.ToString());
				}
			}
		}

	}
}