using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility;
using UnityEngine.Events;
using VNToolkit.VNUtility.VNIcon;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

namespace VNToolkit.VNEditor {

	public class VNChapterListEditor : VNPanelAbstract {

		// Public Variables
		public VNChapterData CurrentElementData {
			get {
				if (dataNodeIndx < 0 || dataElementIndx < 0)
					return null;

				return dataNodeList[dataNodeIndx].dataElements[dataElementIndx].elementData;
			}
		}

		// Private Variables
		private Texture2D chapterAddIcon;
		private Texture2D chapterMinusIcon;

		public class ChapterDataElements {
			public bool elementEnabled { get; set; }
			public VNChapterData elementData { get; set; }
		}

		public class ChapterDataNode {
			public int selectedNodeIndx { get; set; }
			public bool nodeEnabled { get; set; }
			public ChapterDataElements[] dataElements { get; set; }
		}

		private ChapterDataNode[] dataNodeList;
		private int dataNodeIndx;
		private int dataElementIndx;
		private int chapterElementCount;

		private GUIStyle nodeStyle;
		private VNChapterInfoEditor chapterInfo;

		private const int CHAPTER_ELEMENT_MAX = 10;
		private const string CHAPTER_NAME_FORMAT = "Chapter {0}";
		private const string CHAPTER_TOTAL_FORMAT = "Total Chapters: {0}";
		private const string CHAPTER_REMOVE_DATA_FORMAT = "Are you sure you want to permanently remove Chapter {0}?";

		// Static Variables

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_CHAPTER_LIST_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_CHAPTER_LIST; }
		}

		protected override bool IsPanelFoldable {
			get { return false; }
		}

		protected override bool IsPanelFlexible {
			get { return false; }
		}

		protected override bool IsRefreshable {
			get { return false; }
		}

		protected override bool IsScrollable {
			get { return false; }
		}

		protected override float PanelWidth {
			get { return 350f; }
		}

		protected override System.Action<Rect> OnPanelGUI {
			get { return ChapterWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);

			chapterElementCount = 0;

			dataNodeIndx = -1;
			dataElementIndx = -1;
			AddChapterNode();

			nodeStyle = new GUIStyle(EditorStyles.miniButton);
			nodeStyle.onNormal.background = null;			

			chapterAddIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_ADD);
			chapterMinusIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_MINUS);
		}

		protected override void PanelOpen() {
			base.PanelOpen();

			VNDataManager.SharedInstance.LoadData();
		}

		protected override void PanelClose() {
			base.PanelClose();
		}

		protected override void PanelSave() {
			base.PanelSave();

			VNChapterData tmpData = VNDataManager.GetData<VNChapterData>(dataNodeList[dataNodeIndx].dataElements[dataElementIndx].elementData) as VNChapterData;
			tmpData.Save();
		}

		protected override void PanelLoad() {
			base.PanelLoad();

			dataNodeList[dataNodeIndx].dataElements = new ChapterDataElements[0];
			for (int i = 0; i < VNDataManager.SharedInstance.VnChapterData.Count; i++) {
				ChapterDataElements newElement = new ChapterDataElements();
				newElement.elementData = VNDataManager.SharedInstance.VnChapterData[i];

				ChapterDataElements[]  elements = dataNodeList[dataNodeIndx].dataElements;
				ArrayUtility.Add<ChapterDataElements>(ref elements, newElement);
				dataNodeList[dataNodeIndx].dataElements = elements;
			}

			SetChapterElementIndex(dataNodeList.Length - 1);
		}

		protected override void PanelClear() {
			base.PanelClear();
		}

		protected override void PanelReset() {
			base.PanelReset();
		}

		protected override void PanelRefresh() {
			base.PanelRefresh();
		}

		private void ChapterWindow(Rect position) {
			//EditorGUILayout.LabelField("Node: " + dataNodeIndx, EditorStyles.label);
			//EditorGUILayout.LabelField("Element: " + dataElementIndx, EditorStyles.label);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(string.Format(CHAPTER_TOTAL_FORMAT, chapterElementCount), EditorStyles.label);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX);

			EditorGUILayout.BeginHorizontal();

			DrawChapterElements();

			GUILayout.FlexibleSpace();
			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);
			if (GUILayout.Button(chapterAddIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
				AddChapterElement();
				VNEditorUtility.SetAllPanelStateRecursively(parent, VN_PANELSTATE.REFRESH);
			}

			if (GUILayout.Button(chapterMinusIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
				if(EditorUtility.DisplayDialog(
					"Removing Chapter Element",
					string.Format(CHAPTER_REMOVE_DATA_FORMAT, (dataElementIndx + 1)),
					"Continue",
					"Cancel")) 
				{
					RemoveChapterElement(dataElementIndx);
					dataElementIndx = -1;
					VNEditorUtility.SetAllPanelStateRecursively(parent, VN_PANELSTATE.REFRESH);
				}
				
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndHorizontal();

			DrawChapterNode();

			EditorGUILayout.EndVertical();
		}

		# endregion Panel Editor Abstract

		public void SaveAllChapters() {
			int count = 0;
			for (int i = 0; i < dataNodeList.Length; i++) {
				for (int j = 0; j < dataNodeList[i].dataElements.Length; j++) {
					if (VNDataManager.SharedInstance.VnChapterData.Contains(dataNodeList[i].dataElements[j].elementData)) {
						if (VNEditorUtility.Compare<VNChapterData>(VNDataManager.SharedInstance.VnChapterData[count], dataNodeList[i].dataElements[j].elementData)) {
							VNDataManager.SharedInstance.VnChapterData[count] = dataNodeList[i].dataElements[j].elementData;
							count++;
							continue;
						}
					}

					VNDataManager.SharedInstance.VnChapterData.Add(dataNodeList[i].dataElements[j].elementData);
				}
			}
		}

		// Nodes
		private void AddChapterNode() {
			if (dataNodeList == null) {
				dataNodeList = new ChapterDataNode[0];
			}

			ChapterDataNode dataNode = new ChapterDataNode();
			dataNode.dataElements = new ChapterDataElements[0];

			ArrayUtility.Add<ChapterDataNode>(ref dataNodeList, new ChapterDataNode());
			SetChapterNodeIndex(dataNodeList.Length - 1);

			// Add first element
			AddChapterElement();
		}

		private void DrawChapterNode() {
			if (dataNodeList == null)
				return;

			//if (dataNodeList.Length < 2)
			//    return;

			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX, GUILayout.Width(295f));

			for (int i = 0; i < dataNodeList.Length; i++) {
				bool toggle = dataNodeList[i].nodeEnabled;
				string name = string.Empty;

				Color originalColor = GUI.color;
				GUI.color = (toggle) ? Color.yellow : Color.white;

				toggle = GUILayout.Toggle(
					toggle,
					name,
					nodeStyle,
					GUILayout.Width(15f),
					GUILayout.Height(15f)
				);

				GUI.color = originalColor;

				// On value changed
				if (dataNodeList[i].nodeEnabled != toggle) {
					if (toggle) {
						SetChapterNodeIndex(i);
						VNEditorUtility.SetAllPanelStateRecursively(parent, VN_PANELSTATE.REFRESH);
					}
					else {
						dataNodeIndx = -1;
						VNEditorUtility.SetAllPanelStateRecursively(parent, VN_PANELSTATE.REFRESH);
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
					dataElementIndx = dataNodeList[dataNodeIndx].selectedNodeIndx;
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

			chapterElementCount++;

			VNChapterData newData = new VNChapterData();
			newData.chapterName = string.Format(CHAPTER_NAME_FORMAT, chapterElementCount);
			newData.DATA_ID = chapterElementCount;

			ArrayUtility.Add<ChapterDataElements>(ref elements, new ChapterDataElements() { elementData = newData });
			dataNodeList[dataNodeIndx].dataElements = elements;
			SetChapterElementIndex(elements.Length - 1);

			UpdateChapterData();
		}

		private void RemoveChapterElement(int indx) {
			if (dataNodeList == null || dataNodeList[dataNodeIndx].dataElements == null)
				return;

			// element to remove
			ChapterDataElements element = dataNodeList[dataNodeIndx].dataElements[indx];

			chapterElementCount--;
			ChapterDataElements[] elements = dataNodeList[dataNodeIndx].dataElements;
			ArrayUtility.Remove<ChapterDataElements>(ref elements, element);
			dataNodeList[dataNodeIndx].dataElements = elements;

			UpdateChapterData();
		}

		private void DrawChapterElements() {
			if (dataNodeIndx < 0)
				return;

			if (dataNodeList == null || dataNodeList[dataNodeIndx].dataElements == null)
				return;

			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX, GUILayout.Width(295f));
			ChapterDataElements[] elements = dataNodeList[dataNodeIndx].dataElements;
			for (int i = 0; i < elements.Length; i++) {
				bool toggle = elements[i].elementEnabled;
				//string name = (i + 1).ToString();
				string name = elements[i].elementData.DATA_ID.ToString();

				//Color originalColor = GUI.color;
				//GUI.color = (toggle) ? Color.yellow : Color.white;

				toggle = GUILayout.Toggle(
					toggle,
					name,
					EditorStyles.miniButton,
					GUILayout.Width(25f),
					GUILayout.Height(25f)
				);

				//GUI.color = originalColor;

				// On value changed
				if (elements[i].elementEnabled != toggle) {
					if (toggle) {
						SetChapterElementIndex(i);
						VNEditorUtility.SetAllPanelStateRecursively(parent, VN_PANELSTATE.REFRESH);
					}
					else {
						dataElementIndx = -1;
						VNEditorUtility.SetAllPanelStateRecursively(parent, VN_PANELSTATE.REFRESH);
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
					dataNodeList[dataNodeIndx].selectedNodeIndx = dataElementIndx;
					//VNEditorUtility.SetAllPanelStateRecursively(parent, VN_PANELSTATE.REFRESH);
					continue;
				}

				elements[i].elementEnabled = false;
			}

			dataNodeList[dataNodeIndx].dataElements = elements;
		}

		private void UpdateChapterData() {
			int counter = (dataNodeIndx * 10) + 1;
			for (int i = 0; i < dataNodeList[dataNodeIndx].dataElements.Length; i++) {
				dataNodeList[dataNodeIndx].dataElements[i].elementData.DATA_ID = counter;
				dataNodeList[dataNodeIndx].dataElements[i].elementData.chapterName = string.Format(CHAPTER_NAME_FORMAT, counter);
				counter++;
			}
		}
	}
}