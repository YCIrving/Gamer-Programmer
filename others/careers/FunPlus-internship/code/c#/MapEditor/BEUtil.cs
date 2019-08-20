using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;  

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEUtil
///   Description:    utility class
///   Usage :		  find gameobject with string contain hierachy
///                   GetObject("main/aaa/bbb");
/// 				  Get Reliable path
/// 				  Set Color of gameobject
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class BEUtil : MonoBehaviour {

		public	static BEUtil instance;
		
		void Awake() {
			instance=this;
		}
		
		//GmaeObject.Find("PanelMain").transform.Find("Top").transform.Find("Label") -> GetObject("PanelMain/Top/Label") 
		public static GameObject GetObject(string path) { 
			GameObject goTop = null;
			string[] PathSub = path.Split('/');
			for(int i=0 ; i < PathSub.Length ; ++i) {
				goTop = (i==0) ? GameObject.Find (PathSub[i]).gameObject : GetObject(goTop, PathSub[i]);
				if(i == (PathSub.Length-1)) {
					break; 
				}
			}
			
			return goTop;
		}
		public static GameObject GetObject(GameObject go, string path) { 
			GameObject goTop = go;
			if(!string.Equals(path,"")) {
				string[] PathSub = path.Split('/');
				for(int i=0 ; i < PathSub.Length ; ++i) {
					goTop = goTop.transform.Find(PathSub[i]).gameObject;
					if(i == (PathSub.Length-1)) {
						break; 
					}
				}
			}
			
			return goTop; 
		}

		public static void SetObjectColor(GameObject go, Color color) {
			if(go == null) return;

			if(go.GetComponent<Renderer>() != null) {
				for(int i=0 ; i < go.GetComponent<Renderer>().materials.Length ; ++i) {
					go.GetComponent<Renderer>().materials[i].color = color;
				}
			}
			else {
				for(int j=0 ; j < go.transform.childCount; ++j) {
					SetObjectColor (go.transform.GetChild(j).gameObject, color);
				}
			}
		}

		public static void SetObjectColor(GameObject go, string propertyName, Color color) {
			if(go == null) return;
			
			if(go.GetComponent<Renderer>() != null) {
				for(int i=0 ; i < go.GetComponent<Renderer>().materials.Length ; ++i) {
					go.GetComponent<Renderer>().materials[i].SetColor(propertyName, color);
				}
			}
			else {
				for(int j=0 ; j < go.transform.childCount; ++j) {
					SetObjectColor (go.transform.GetChild(j).gameObject, propertyName, color);
				}
			}
		}

		//
		public static string pathForDocumentsFile( string filename ) { 
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				//string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5 );
				//path = path.Substring( 0, path.LastIndexOf( '/' ) );
				//return Path.Combine( Path.Combine( path, "Documents" ), filename );
				return Application.persistentDataPath+"/"+filename;
			}
			else if(Application.platform == RuntimePlatform.Android) {
				string path = Application.persistentDataPath;
				path = path.Substring(0, path.LastIndexOf( '/' ) );
				return Path.Combine (path, filename);
			}
			else  {
				string path = Application.dataPath;
				path = path.Substring(0, path.LastIndexOf( '/' ) );
				return Path.Combine (path, filename);
			}
		}
		
		// 
		//
		public static string Encrypt (string toEncrypt) {
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012");
			// 256-AES key
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes (toEncrypt);
			RijndaelManaged rDel = new RijndaelManaged ();
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			// http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
			rDel.Padding = PaddingMode.PKCS7;
			// better lang support
			ICryptoTransform cTransform = rDel.CreateEncryptor ();
			byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
			return Convert.ToBase64String (resultArray, 0, resultArray.Length);
		}
		
		public static string Decrypt (string toDecrypt) {
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012");
			// AES-256 key
			byte[] toEncryptArray = Convert.FromBase64String (toDecrypt);
			RijndaelManaged rDel = new RijndaelManaged ();
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			// http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
			rDel.Padding = PaddingMode.PKCS7;
			// better lang support
			ICryptoTransform cTransform = rDel.CreateDecryptor ();
			byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
			return UTF8Encoding.UTF8.GetString (resultArray);
		}

	}
}
