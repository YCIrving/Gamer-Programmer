using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEObjectPool
///   Description:    The cost of instantiate & destory is too high,
///                   we instantiate many object at the start time,
///                   and activate or deactivate object at runtime 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-08-30)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class BEObjectPool : MonoBehaviour {

		private static BEObjectPool 	_instance;
		public 	static BEObjectPool 	instance {
			get {
				if (!_instance) {
					_instance = GameObject.FindObjectOfType(typeof(BEObjectPool)) as BEObjectPool;
					if (!_instance) {
						GameObject container = new GameObject("ObjectPool");
						_instance = container.AddComponent(typeof(BEObjectPool)) as BEObjectPool;
						DontDestroyOnLoad(_instance);
					}
				}
				
				return _instance;
			}
		}

		[System.Serializable]
		public class PoolItem {
			public int 			size;
			public GameObject 	prefab;
		}

		Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
		Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
		
		public PoolItem[] Pools;
		
		void Awake() {
			CreateStartupPools();
		}

		void Start() {
		}
		
		public static void CreateStartupPools() {
			var pools = instance.Pools;
			if (pools != null && pools.Length > 0)
				for (int i = 0; i < pools.Length; ++i)
					CreatePool(pools[i].prefab, pools[i].size);
		}
		public static void CreatePool(GameObject prefab, int initialPoolSize) {
			if (prefab != null && !instance.pooledObjects.ContainsKey(prefab)) {
				var list = new List<GameObject>();
				instance.pooledObjects.Add(prefab, list);
				
				if (initialPoolSize > 0) {
					bool active = prefab.activeSelf;
					prefab.SetActive(false);
					Transform trParent = instance.transform;
					while (list.Count < initialPoolSize) {
						var obj = (GameObject)Object.Instantiate(prefab);
						obj.transform.parent = trParent;
						list.Add(obj);
					}
					prefab.SetActive(active);
				}
			}
		}
		public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position) {
			return Spawn(prefab, parent, position, Quaternion.identity);
		}
		public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) {
			return Spawn(prefab, null, position, rotation);
		}
		public static GameObject Spawn(GameObject prefab, Transform parent) {
			return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
		}
		public static GameObject Spawn(GameObject prefab, Vector3 position) {
			return Spawn(prefab, null, position, Quaternion.identity);
		}
		public static GameObject Spawn(GameObject prefab) {
			return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
		}
		public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation) {
			List<GameObject> list;
			Transform tr;
			GameObject obj;
			if (instance.pooledObjects.TryGetValue(prefab, out list)) {
				obj = null;
				if (list.Count > 0) {
					while (obj == null && list.Count > 0) {
						obj = list[0];
						list.RemoveAt(0);
					}
					if (obj != null) {
						tr = obj.transform;
						tr.parent = parent;
						tr.localPosition = position;
						tr.localRotation = rotation;
						obj.SetActive(true);
						instance.spawnedObjects.Add(obj, prefab);
						return obj;
					}
				}
				obj = (GameObject)Object.Instantiate(prefab);
				tr = obj.transform;
				tr.parent = parent;
				tr.localPosition = position;
				tr.localRotation = rotation;
				instance.spawnedObjects.Add(obj, prefab);
				return obj;
			}
			else {
				obj = (GameObject)Object.Instantiate(prefab);
				tr = obj.GetComponent<Transform>();
				tr.parent = parent;
				tr.localPosition = position;
				tr.localRotation = rotation;
				return obj;
			}
		}
		public static void Unspawn(GameObject obj) {
			GameObject prefab;
			if (instance.spawnedObjects.TryGetValue(obj, out prefab))
				Unspawn(obj, prefab);
			else
				Object.Destroy(obj);
		}
		static void Unspawn(GameObject obj, GameObject prefab) {
			instance.pooledObjects[prefab].Add(obj);
			instance.spawnedObjects.Remove(obj);
			obj.transform.parent = instance.transform;
			// set deavtivated object's position to far position
			obj.transform.position = new Vector3(10000,10000,10000);
			obj.SetActive(false);
		}

	}

}
