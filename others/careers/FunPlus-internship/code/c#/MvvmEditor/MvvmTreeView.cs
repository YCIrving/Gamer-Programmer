using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;

internal class MvvmTreeView : TreeViewWithTreeModel<MvvmGameObject>
{
	const float kRowHeights = 20f;
	const float kToggleWidth = 18f;
	public bool showControls = true;

	static Texture2D[] _testIcons =
	{
		EditorGUIUtility.FindTexture ("Folder Icon"),
		EditorGUIUtility.FindTexture ("AudioSource Icon"),
		EditorGUIUtility.FindTexture ("Camera Icon"),
		EditorGUIUtility.FindTexture ("Windzone Icon"),
		EditorGUIUtility.FindTexture ("GameObject Icon")

	};

	// All columns
	enum MvvmColumns
	{
		// Icon,
		// Icon2,
		Enabled,
		Name,
		Tag,
		Material,
		FloatValue1
	}

	public enum SortOption
	{
		Enabled,
		Name,
		Tag,
		Material,		
		FloatValue1
	}

	// Sort options per column
	SortOption[] _sortOptions = 
	{
		SortOption.Enabled, 
		SortOption.Name, 
		SortOption.Tag, 
		SortOption.Material, 
		SortOption.FloatValue1
	};

	public static void TreeToList (TreeViewItem root, IList<TreeViewItem> result)
	{
		if (root == null)
			throw new NullReferenceException("root");
		if (result == null)
			throw new NullReferenceException("result");

		result.Clear();

		if (root.children == null)
			return;

		Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
		for (int i = root.children.Count - 1; i >= 0; i--)
			stack.Push(root.children[i]);

		while (stack.Count > 0)
		{
			TreeViewItem current = stack.Pop();
			result.Add(current);

			if (current.hasChildren && current.children[0] != null)
			{
				for (int i = current.children.Count - 1; i >= 0; i--)
				{
					stack.Push(current.children[i]);
				}
			}
		}
	}

	public MvvmTreeView (TreeViewState state, MultiColumnHeader multicolumnHeader, TreeModel<MvvmGameObject> model) : base (state, multicolumnHeader, model)
	{
		Assert.AreEqual(_sortOptions.Length , Enum.GetValues(typeof(MvvmColumns)).Length, "Ensure number of sort options are in sync with number of MvvmColumns enum values");

		// Custom setup
		rowHeight = kRowHeights;
		columnIndexForTreeFoldouts = 1;
		showAlternatingRowBackgrounds = true;
		showBorder = true;
		customFoldoutYOffset = (kRowHeights - EditorGUIUtility.singleLineHeight) * 0.5f; // center foldout in the row since we also center content. See RowGUI
		extraSpaceBeforeIconAndLabel = kToggleWidth;
		multicolumnHeader.sortingChanged += OnSortingChanged;
		
		Reload();
	}


	// Note we We only build the visible rows, only the backend has the full tree information. 
	// The treeview only creates info for the row list.
	protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
	{
		var rows = base.BuildRows (root);
		SortIfNeeded (root, rows);
		return rows;
	}

	void OnSortingChanged (MultiColumnHeader multiColumnHeader)
	{
		SortIfNeeded (rootItem, GetRows());
	}

	void SortIfNeeded (TreeViewItem root, IList<TreeViewItem> rows)
	{
		if (rows.Count <= 1)
			return;
		
		if (multiColumnHeader.sortedColumnIndex == -1)
		{
			return; // No column to sort for (just use the order the data are in)
		}
		
		// Sort the roots of the existing tree items
		SortByMultipleColumns ();
		TreeToList(root, rows);
		Repaint();
	}

	void SortByMultipleColumns ()
	{
		var sortedColumns = multiColumnHeader.state.sortedColumns;

		if (sortedColumns.Length == 0)
			return;

		var myTypes = rootItem.children.Cast<TreeViewItem<MvvmGameObject> >();
		var orderedQuery = InitialOrder (myTypes, sortedColumns);
		for (int i=1; i<sortedColumns.Length; i++)
		{
			SortOption sortOption = _sortOptions[sortedColumns[i]];
			bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);

			switch (sortOption)
			{
				case SortOption.Enabled:
					orderedQuery = orderedQuery.ThenBy(l => l.data.enabled, ascending);
					break;
				case SortOption.Name:
					orderedQuery = orderedQuery.ThenBy(l => l.data.name, ascending);
					break;
				case SortOption.Tag:
					orderedQuery = orderedQuery.ThenBy(l => (l.data.tag + l.data.id.ToString()), ascending);
					break;
				case SortOption.Material:
					orderedQuery = orderedQuery.ThenBy(l => l.data.material, ascending);
					break;
				case SortOption.FloatValue1:
					orderedQuery = orderedQuery.ThenBy(l => l.data.floatValue1, ascending);
					break;
			}
		}

		rootItem.children = orderedQuery.Cast<TreeViewItem> ().ToList ();
	}

	IOrderedEnumerable<TreeViewItem<MvvmGameObject>> InitialOrder(IEnumerable<TreeViewItem<MvvmGameObject>> myTypes, int[] history)
	{
		SortOption sortOption = _sortOptions[history[0]];
		bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
		switch (sortOption)
		{
			case SortOption.Enabled:
				return myTypes.Order(l => l.data.enabled, ascending);
			case SortOption.Name:
				return myTypes.Order(l => l.data.name, ascending);
			case SortOption.Tag:
				return myTypes.Order(l => (l.data.tag + l.data.id.ToString()), ascending);
			case SortOption.Material:
				return myTypes.Order(l => l.data.material, ascending);
			case SortOption.FloatValue1:
				return myTypes.Order(l => l.data.floatValue1, ascending);
			default:
				Assert.IsTrue(false, "Unhandled enum");
				break;
		}
		// default
		return myTypes.Order(l => l.data.name, ascending);
	}

	// 根据名称得到图标，暂时不需要
	int GetIconIndex(TreeViewItem<MvvmGameObject> item)
	{
		return (int)(Mathf.Min(0.99f, item.data.floatValue1) * _testIcons.Length);
	}

	int GetIcon2Index (TreeViewItem<MvvmGameObject> item)
	{
		return Mathf.Min(item.data.text.Length, _testIcons.Length-1);
	}

	protected override void RowGUI (RowGUIArgs args)
	{
		var item = (TreeViewItem<MvvmGameObject>) args.item;

		for (int i = 0; i < args.GetNumVisibleColumns (); ++i)
		{
			CellGUI(args.GetCellRect(i), item, (MvvmColumns)args.GetColumn(i), ref args);
		}
	}

	void CellGUI (Rect cellRect, TreeViewItem<MvvmGameObject> item, MvvmColumns column, ref RowGUIArgs args)
	{
		// Center cell rect vertically (makes it easier to place controls, icons etc in the cells)
		CenterRectUsingSingleLineHeight(ref cellRect);

		switch (column)
		{
//			case MvvmColumns.Icon:
//				{
//					GUI.DrawTexture(cellRect, _testIcons[GetIconIndex(item)], ScaleMode.ScaleToFit);
//				}
//				break;
//			case MvvmColumns.Icon2:
//				{
//					GUI.DrawTexture(cellRect, _testIcons[GetIcon2Index(item)], ScaleMode.ScaleToFit);
//				}
//				break;
			case MvvmColumns.Enabled:
			{
				// Do toggle
				Rect toggleRect = cellRect;
				toggleRect.x += GetContentIndent(item);
				toggleRect.width = kToggleWidth;
				if (toggleRect.xMax < cellRect.xMax)
					item.data.enabled = EditorGUI.Toggle(toggleRect, item.data.enabled); // hide when outside cell rect
			}
				break;
			
			case MvvmColumns.Name:
				{
					args.rowRect = cellRect;
					base.RowGUI(args);
				}
				break;
			
			case MvvmColumns.Tag:
			{
				DefaultGUI.Label(cellRect, item.data.tag + ' ' + item.data.id, args.selected, args.focused);	
			}
				break;
			
			case MvvmColumns.Material:
			{
				item.data.material = (Material)EditorGUI.ObjectField(cellRect, GUIContent.none, item.data.material, typeof(Material), false);
			}
				break;

			case MvvmColumns.FloatValue1:
			// case MvvmColumns.Value2:
			// case MvvmColumns.Value3:
				{
					if (showControls)
					{
						cellRect.xMin += 5f; // When showing controls make some extra spacing

						if (column == MvvmColumns.FloatValue1)
							item.data.floatValue1 = EditorGUI.Slider(cellRect, GUIContent.none, item.data.floatValue1, 0f, 1f);
//						if (column == MvvmColumns.Value2)
//							item.data.material = (Material)EditorGUI.ObjectField(cellRect, GUIContent.none, item.data.material, typeof(Material), false);
//						if (column == MvvmColumns.Value3)
//							item.data.text = GUI.TextField(cellRect, item.data.text);
					}
					else
					{
						string value = "Missing";
						if (column == MvvmColumns.FloatValue1)
							value = item.data.floatValue1.ToString("f5");
//						if (column == MvvmColumns.Value2)
//							value = item.data.floatValue2.ToString("f5");
//						if (column == MvvmColumns.Value3)
//							value = item.data.floatValue3.ToString("f5");

						DefaultGUI.LabelRightAligned(cellRect, value, args.selected, args.focused);
					}
				}
				break;
		}
	}

	// Rename
	//--------

	protected override bool CanRename(TreeViewItem item)
	{
		// Only allow rename if we can show the rename overlay with a certain width (label might be clipped by other columns)
		Rect renameRect = GetRenameRect (treeViewRect, 0, item);
		return renameRect.width > 30;
	}

	protected override void RenameEnded(RenameEndedArgs args)
	{
		// Set the backend name and reload the tree to reflect the new model
		if (args.acceptedRename)
		{
			var element = treeModel.Find(args.itemID);
			element.name = args.newName;
			Reload();
		}
	}

	protected override Rect GetRenameRect (Rect rowRect, int row, TreeViewItem item)
	{
		Rect cellRect = GetCellRectForTreeFoldouts (rowRect);
		CenterRectUsingSingleLineHeight(ref cellRect);
		return base.GetRenameRect (cellRect, row, item);
	}

	// Misc
	//--------

	protected override bool CanMultiSelect (TreeViewItem item)
	{
		return true;
	}

	public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
	{
		var columns = new[] 
		{
//			new MultiColumnHeaderState.Column 
//			{
//				headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByLabel"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. "),
//				contextMenuText = "Asset",
//				headerTextAlignment = TextAlignment.Center,
//				sortedAscending = true,
//				sortingArrowAlignment = TextAlignment.Right,
//				width = 30, 
//				minWidth = 30,
//				maxWidth = 60,
//				autoResize = false,
//				allowToggleVisibility = true
//			},
//			new MultiColumnHeaderState.Column 
//			{
//				headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByType"), "Sed hendrerit mi enim, eu iaculis leo tincidunt at."),
//				contextMenuText = "Type",
//				headerTextAlignment = TextAlignment.Center,
//				sortedAscending = true,
//				sortingArrowAlignment = TextAlignment.Right,
//				width = 30, 
//				minWidth = 30,
//				maxWidth = 60,
//				autoResize = false,
//				allowToggleVisibility = true
//			},
			new MultiColumnHeaderState.Column 
			{
				headerContent = new GUIContent("Enabled"),
				headerTextAlignment = TextAlignment.Left,
				sortedAscending = true,
				sortingArrowAlignment = TextAlignment.Center,
				width = 100, 
				minWidth = 50,
				autoResize = false,
				allowToggleVisibility = false
			},
			new MultiColumnHeaderState.Column 
			{
				headerContent = new GUIContent("Name"),
				headerTextAlignment = TextAlignment.Left,
				sortedAscending = false,
				sortingArrowAlignment = TextAlignment.Center,
				width = 150, 
				minWidth = 60,
				autoResize = false,
				allowToggleVisibility = false
			},

			new MultiColumnHeaderState.Column 
			{
				headerContent = new GUIContent("Tag", "Nam at tellus ultricies ligula vehicula ornare sit amet quis metus."),
				headerTextAlignment = TextAlignment.Left,
				sortedAscending = true,
				sortingArrowAlignment = TextAlignment.Center,
				width = 70,
				minWidth = 60,
				autoResize = true
			},			
			new MultiColumnHeaderState.Column 
            {
              	headerContent = new GUIContent("Material", "Maecenas congue non tortor eget vulputate."),
              	headerTextAlignment = TextAlignment.Left,
              	sortedAscending = true,
              	sortingArrowAlignment = TextAlignment.Center,
              	width = 95,
              	minWidth = 60,
              	autoResize = true,
              	allowToggleVisibility = false
            },
			new MultiColumnHeaderState.Column 
			{
			headerContent = new GUIContent("FloatValue1", "In sed porta ante. Nunc et nulla mi."),
			headerTextAlignment = TextAlignment.Left,
			sortedAscending = true,
			sortingArrowAlignment = TextAlignment.Center,
			width = 110,
			minWidth = 60,
			autoResize = true
			}
		};

		Assert.AreEqual(columns.Length, Enum.GetValues(typeof(MvvmColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");

		var state =  new MultiColumnHeaderState(columns);
		return state;
	}
}

static class MvvmExtensionMethods
{
	public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
	{
		if (ascending)
		{
			return source.OrderBy(selector);
		}
		else
		{
			return source.OrderByDescending(selector);
		}
	}

	public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
	{
		if (ascending)
		{
			return source.ThenBy(selector);
		}
		else
		{
			return source.ThenByDescending(selector);
		}
	}
}
