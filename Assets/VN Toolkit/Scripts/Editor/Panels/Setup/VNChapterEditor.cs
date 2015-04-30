using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility;
using UnityEngine.Events;
using VNToolkit.VNUtility.VNIcon;
using UnityEditor;
using System.Collections.Generic;

namespace VNToolkit.VNEditor {

	public class VNChapterEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private Texture2D chapterAddIcon;
		private Texture2D chapterMinusIcon;

		public class ChapterDataElements {
			public bool elementEnabled { get; set; }
			public List<bool> element { get; set; }
		}

		public class ChapterDataNode {
			public bool nodeEnabled { get; set; }
			public ChapterDataElements[] dataElements { get; set; }
		}

		private ChapterDataNode[] dataNodeList;
		private int dataNodeIndx;
		private int dataElementIndx;

		GUIStyle nodeStyle;

		private const int CHAPTER_ELEMENT_MAX = 10;
		private const string CHAPTER_NAME_FORMAT = "Chapter {0}";

		// Static Variables

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_CHAPTER_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_CHAPTER; }
		}

		protected override bool IsPanelFoldable {
			get { return true; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override bool IsRefreshable {
			get { return false; }
		}

		protected override bool IsScrollable {
			get { return false; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		protected override System.Action<Rect> OnPanelGUI {
			get { return ChapterWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);

			dataNodeIndx = -1;
			dataElementIndx = -1;
			AddChapterNode();

			nodeStyle = new GUIStyle(EditorStyles.miniButton);
			nodeStyle.onNormal.background = null;

			chapterAddIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_ADD);
			chapterMinusIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_MINUS);
		}

		private void ChapterWindow(Rect position) {
			EditorGUILayout.LabelField("Node: " + dataNodeIndx, EditorStyles.label);
			EditorGUILayout.LabelField("Element: " + dataElementIndx, EditorStyles.label);

			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);

			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);
			if (GUILayout.Button(chapterAddIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
				AddChapterElement();
			}

			if (GUILayout.Button(chapterMinusIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {

			}

			EditorGUILayout.EndHorizontal();

			DrawChapterNode();

			EditorGUILayout.EndHorizontal();

			DrawChapterElements();
		}

		# endregion Panel Editor Abstract

		// Nodes
		private void AddChapterNode() {
			if (dataNodeList == null) {
				dataNodeList = new ChapterDataNode[0];
			}

			ArrayUtility.Add<ChapterDataNode>(ref dataNodeList, new ChapterDataNode());
			SetChapterNodeIndex(dataNodeList.Length - 1);

			// Add first element;
			AddChapterElement();
		}

		private void DrawChapterNode() {
			if (dataNodeList == null)
				return;

			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);

			for (int i = 0; i < dataNodeList.Length; i++) {
				bool toggle = dataNodeList[i].nodeEnabled;
				string name = (i + 1).ToString();

				Color originalColor = GUI.color;
				GUI.color = (toggle) ? Color.yellow : Color.white;

				toggle = GUILayout.Toggle(
					toggle,
					name,
					nodeStyle,
					GUILayout.Width(20f),
					GUILayout.Height(20f)
				);

				GUI.color = originalColor;

				// On value changed
				if (dataNodeList[i].nodeEnabled != toggle) {
					if (toggle) {
						SetChapterNodeIndex(i);
					}
					else {
						dataNodeIndx = -1;
					}
				}

				// Always update node index with the current active node
				if (toggle && i != dataNodeIndx) {
					SetChapterNodeIndex(i);
				}

				dataNodeList[i].nodeEnabled = toggle;
			}

			EditorGUILayout.EndHorizontal();
		}

		private void SetChapterNodeIndex(int indx) {
			for (int i = 0; i < dataNodeList.Length; i++) {
				// Set the current index to active
				if (i == indx) {
					dataNodeList[i].nodeEnabled = true;
					dataNodeIndx = indx;
					continue;
				}

				dataNodeList[i].nodeEnabled = false;
			}
		}

		// Additional Node when pressint add button
		private void AddNewChapterNode() {
			if (dataNodeList == null || dataNodeList[dataNodeIndx].dataElements == null)
				return;

			if (dataNodeList[dataNodeIndx].dataElements.Length < CHAPTER_ELEMENT_MAX)
				return;

			int curNode = 0;
			for (int i = 0; i < dataNodeList.Length; i++) {
				ChapterDataElements[] elements = dataNodeList[i].dataElements;
				if (elements.Length == CHAPTER_ELEMENT_MAX) {
					curNode++;
				}
			}

			if (curNode == dataNodeList.Length) {
				AddChapterNode();
				return;
			}

			SetChapterNodeIndex(dataNodeList.Length - 1);
		}

		// Elements
		private void AddChapterElement() {
			if (dataNodeList[dataNodeIndx].dataElements == null) {
				dataNodeList[dataNodeIndx].dataElements = new ChapterDataElements[0];
			}

			ChapterDataElements[] elements = dataNodeList[dataNodeIndx].dataElements;
			if (elements.Length >= CHAPTER_ELEMENT_MAX) {
				AddNewChapterNode();
				return;
			}

			ArrayUtility.Add<ChapterDataElements>(ref elements, new ChapterDataElements());
			dataNodeList[dataNodeIndx].dataElements = elements;
			SetChapterElementIndex(elements.Length - 1);
		}

		private void DrawChapterElements() {
			if (dataNodeIndx < 0)
				return;

			if (dataNodeList == null || dataNodeList[dataNodeIndx].dataElements == null)
				return;

			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);
			ChapterDataElements[] elements = dataNodeList[dataNodeIndx].dataElements;
			for (int i = 0; i < elements.Length; i++) {
				bool toggle = elements[i].elementEnabled;
				string name = (i + 1).ToString();

				Color originalColor = GUI.color;
				GUI.color = (toggle) ? Color.yellow : Color.white;

				toggle = GUILayout.Toggle(
					toggle,
					name,
					nodeStyle,
					GUILayout.Width(25f),
					GUILayout.Height(25f)
				);

				GUI.color = originalColor;

				// On value changed
				if (elements[i].elementEnabled != toggle) {
					if (toggle) {
						SetChapterElementIndex(i);
					}
					else {
						dataElementIndx = -1;
					}
				}

				// Always update node index with the current active node
				if (toggle && i != dataElementIndx) {
					SetChapterElementIndex(i);
				}

				elements[i].elementEnabled = toggle;
			}

			dataNodeList[dataNodeIndx].dataElements = elements;
			EditorGUILayout.EndHorizontal();
		}

		private void SetChapterElementIndex(int indx) {
			if (dataNodeList == null || dataNodeList[dataNodeIndx].dataElements == null)
				return;

			ChapterDataElements[] elements = dataNodeList[dataNodeIndx].dataElements;
			for (int i = 0; i < elements.Length; i++) {
				if (i == indx) {
					elements[i].elementEnabled = true;
					dataElementIndx = indx;
					continue;
				}

				elements[i].elementEnabled = false;
			}

			dataNodeList[dataNodeIndx].dataElements = elements;
		}
	}
}