using System;
using System.Collections.Generic;
using TextureArrayInspector;
using TMPro;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.UIElements;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

class MvvmWindow : EditorWindow
{
	// [NonSerialized] private bool _initialized=false;
	[SerializeField] TreeViewState _treeViewState; // Serialized in the window layout file so it survives assembly reloading
	[SerializeField] MultiColumnHeaderState _multiColumnHeaderState;
	private SearchField _searchField;
	private MvvmTreeView _treeView;
	private GameObject _openedPrefab;
	private MvvmPrefab _mvvmPrefab;
	
	static bool _mvvmWindowFlag = true;
	
	int IDCounter = 0;

	[MenuItem("Window/Mvvm Treeview")]
	public static MvvmWindow GetWindow ()
	{
		var window = GetWindow<MvvmWindow>();
		window.titleContent = new GUIContent("Mvvm Window");
		window.Focus();
		window.Repaint();
		return window;
	}
	
	[MenuItem("Window/Mvvm Treeview", true)]
	static bool ValidateMvvmWindow()
	{
//		EditorUtility.DisplayDialog("Coming soon...", "", "OK");
		return _mvvmWindowFlag;
	}

//	[OnOpenAsset]
//	public static bool OnOpenAsset (int instanceID, int line)
//	{
//		var myTreeAsset = EditorUtility.InstanceIDToObject (instanceID) as MyTreeAsset;
//		if (myTreeAsset != null)
//		{
//			var window = GetWindow ();
//			window.SetTreeAsset(myTreeAsset);
//			return true;
//		}
//		return false; // we did not handle the open
//	}

	IList<MvvmGameObject> GetMvvmTreeElements ()
	{
		if (_mvvmPrefab != null && _mvvmPrefab.treeElements != null && _mvvmPrefab.treeElements.Count > 0)
			return _mvvmPrefab.treeElements;
		else
		{
			return null;
		}
	}
	
	void SetMvvmTreeElements (GameObject openedPrefab)
	{
		_mvvmPrefab = ScriptableObject.CreateInstance<MvvmPrefab>();
		var gameObjectTreeElements = new List<MvvmGameObject>();
		IDCounter = 0;
			
		var root = new MvvmGameObject("Root", -1, IDCounter);
		gameObjectTreeElements.Add(root);
		
		// 加入根节点后，排序功能会失效
		root = new MvvmGameObject(_openedPrefab, 0, ++IDCounter);
		gameObjectTreeElements.Add(root);

		foreach (Transform child in _openedPrefab.transform)
		{
			AddChildrenRecursive(root, child, gameObjectTreeElements);
		}
		_mvvmPrefab.treeElements = gameObjectTreeElements;
	}

	void AddChildrenRecursive(TreeElement parentElement, Transform child, List<MvvmGameObject> gameObjectTreeElements)
	{

		var childAdded = new MvvmGameObject(child.gameObject, parentElement.depth + 1, ++IDCounter);
		gameObjectTreeElements.Add(childAdded);

		foreach (Transform grandChild in child.transform)
		{
			AddChildrenRecursive(childAdded, grandChild, gameObjectTreeElements);
		}
	}
	
	

	Rect multiColumnTreeViewRect
	{
		get { return new Rect(20, 40, position.width-40, position.height-60); }
	}

	Rect toolbarRect
	{
		get { return new Rect (20f, 20f, position.width-40f, 20f); }
	}

	Rect bottomToolbarRect
	{
		get { return new Rect(20f, position.height - 18f, position.width - 40f, 16f); }
	}

	public MvvmTreeView treeView
	{
		get { return _treeView; }
	}

	void Init ()
	{
//		if (!_initialized)
//		{
			// Check if it already exists (deserialized from window layout file or scriptable object)
			if (_treeViewState == null)
				_treeViewState = new TreeViewState();

			bool firstInit = _multiColumnHeaderState == null;
			var headerState = MvvmTreeView.CreateDefaultMultiColumnHeaderState(multiColumnTreeViewRect.width);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(_multiColumnHeaderState, headerState))
				MultiColumnHeaderState.OverwriteSerializedFields(_multiColumnHeaderState, headerState);
			_multiColumnHeaderState = headerState;

			var multiColumnHeader = new MvvmMultiColumnHeader(headerState);
			if (firstInit)
				multiColumnHeader.ResizeToFit();

			var treeModel = new TreeModel<MvvmGameObject>(GetMvvmTreeElements());

			_treeView = new MvvmTreeView(_treeViewState, multiColumnHeader, treeModel);

			_searchField = new SearchField();
			_searchField.downOrUpArrowKeyPressed += _treeView.SetFocusAndEnsureSelectedItem;

//			_initialized = true;
//		}

	}


//	void OnSelectionChange ()
//	{
//		if (!m_Initialized)
//			return;
//
//		var myTreeAsset = Selection.activeObject as MyTreeAsset;
//		if (myTreeAsset != null && myTreeAsset != m_MyTreeAsset)
//		{
//			m_MyTreeAsset = myTreeAsset;
//			m_TreeView.treeModel.SetData (GetData ());
//			m_TreeView.Reload ();
//		}
//	}


	void OnGUI ()
	{
		
		
		EditorGUILayout.BeginHorizontal();
		ObjectField();
		if (GUI.changed && _openedPrefab != null)
		{
			var window = GetWindow ();
			window.SetMvvmTreeElements(_openedPrefab);
			Init();
		}

//		if (_openedPrefab!=null && !_initialized)
//		{
//			// 用于类型过滤
//			foreach (var go in _prefabOpened.GetComponents(typeof(Component)))
//			{
//				Debug.Log(go.GetType());
//			}
//			
//		}

		if (_openedPrefab != null)
		{
			SearchBar (toolbarRect);
            DoTreeView (multiColumnTreeViewRect);
            BottomToolBar (bottomToolbarRect);
		}

	 	// var c = _root.GetComponent<TextMeshProUGUI>()
	    
	    EditorGUILayout.EndHorizontal();

		
	}

	void ObjectField()
	{
		 _openedPrefab = (GameObject)EditorGUILayout.ObjectField(_openedPrefab, typeof(GameObject), false);
	}
	
	void SearchBar (Rect rect)
	{
		if (_openedPrefab != null)
		{
			treeView.searchString = _searchField.OnGUI (rect, treeView.searchString);
		}
	}

	void DoTreeView (Rect rect)
	{
		_treeView.OnGUI(rect);
			
	}

	void BottomToolBar (Rect rect)
	{
		GUILayout.BeginArea (rect);

		using (new EditorGUILayout.HorizontalScope ())
		{

			var style = "miniButton";
			if (GUILayout.Button("Expand All", style))
			{
				treeView.ExpandAll ();
			}

			if (GUILayout.Button("Collapse All", style))
			{
				treeView.CollapseAll ();
			}
			
			

			GUILayout.FlexibleSpace();

			GUILayout.Label (_openedPrefab != null ? AssetDatabase.GetAssetPath (_openedPrefab) : string.Empty);

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button("Set sorting", style))
			{
				var myColumnHeader = (MvvmMultiColumnHeader)treeView.multiColumnHeader;
				myColumnHeader.SetSortingColumns (new int[] {4, 3, 2}, new[] {true, false, true});
				myColumnHeader.mode = MvvmMultiColumnHeader.Mode.LargeHeader;
			}


			GUILayout.Label ("Header: ", "minilabel");
			if (GUILayout.Button("Large", style))
			{
				var myColumnHeader = (MvvmMultiColumnHeader) treeView.multiColumnHeader;
				myColumnHeader.mode = MvvmMultiColumnHeader.Mode.LargeHeader;
			}
			if (GUILayout.Button("Default", style))
			{
				var myColumnHeader = (MvvmMultiColumnHeader)treeView.multiColumnHeader;
				myColumnHeader.mode = MvvmMultiColumnHeader.Mode.DefaultHeader;
			}
			if (GUILayout.Button("No sort", style))
			{
				var myColumnHeader = (MvvmMultiColumnHeader)treeView.multiColumnHeader;
				myColumnHeader.mode = MvvmMultiColumnHeader.Mode.MinimumHeaderWithoutSorting;
			}

			GUILayout.Space (10);
			
			if (GUILayout.Button("values <-> controls", style))
			{
				treeView.showControls = !treeView.showControls;
			}
		}

		GUILayout.EndArea();
	}
}


internal class MvvmMultiColumnHeader : MultiColumnHeader
{
	Mode _mode;

	public enum Mode
	{
		LargeHeader,
		DefaultHeader,
		MinimumHeaderWithoutSorting
	}

	public MvvmMultiColumnHeader(MultiColumnHeaderState state)
		: base(state)
	{
		mode = Mode.DefaultHeader;
	}

	public Mode mode
	{
		get
		{
			return _mode;
		}
		set
		{
			_mode = value;
			switch (_mode)
			{
				case Mode.LargeHeader:
					canSort = true;
					height = 37f;
					break;
				case Mode.DefaultHeader:
					canSort = true;
					height = DefaultGUI.defaultHeight;
					break;
				case Mode.MinimumHeaderWithoutSorting:
					canSort = false;
					height = DefaultGUI.minimumHeight;
					break;
			}
		}
	}

	protected override void ColumnHeaderGUI (MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
	{
		// Default column header gui
		base.ColumnHeaderGUI(column, headerRect, columnIndex);

		// Add additional info for large header
		if (mode == Mode.LargeHeader)
		{
			// Show example overlay stuff on some of the columns
			if (columnIndex > 2)
			{
				headerRect.xMax -= 3f;
				var oldAlignment = EditorStyles.largeLabel.alignment;
				EditorStyles.largeLabel.alignment = TextAnchor.UpperRight;
				GUI.Label(headerRect, 36 + columnIndex + "%", EditorStyles.largeLabel);
				EditorStyles.largeLabel.alignment = oldAlignment;
			}
		}
	}
}


// 序列化存储，勿删
//class A : ScriptableObject
//{
//	[SerializeField] public int a;
//}
