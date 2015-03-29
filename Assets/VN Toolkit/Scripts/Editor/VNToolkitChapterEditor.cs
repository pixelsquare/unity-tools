using UnityEngine;
using System.Collections;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using UnityEditor;

namespace VNToolkit {
	public class VNToolkitChapterEditor : VNToolkitPanelAbstract {

		// Public Variables
		public string[] chapterList;
		public Vector2 chapterScrollView;

		public AnimBool chapterInfoAnim;
		public bool[] showInfoWindow;

		// Private Variables
		private int chapterListCount;

		// Static Variables
		public static int selectedChapterIndx;
		public static VNToolkitChapterData[] chapterInfo;

		public override string ControlName {
			get { return VNToolkitControlName.FOCUSED_CHAPTER_PANEL; }
		}

		public override bool PanelActive {
			get { return true; }
		}

		public override bool OnWindowChanged {
			get { return chapterList.Length != chapterListCount; }
		}

		public override System.Action WindowGUI {
			get { return ChapterWindow; }
		}

		public override void Initialize(UnityAction repaint, Rect windowPos) {
			selectedChapterIndx = -1;
			chapterList = new string[0];

			chapterInfoAnim = new AnimBool(true);
			chapterInfoAnim.valueChanged.AddListener(repaint);

			showInfoWindow = new bool[0];
			chapterInfo = new VNToolkitChapterData[0];
			chapterListCount = chapterList.Length;

			RepaintPanel();
		}

		public override void RepaintPanel(bool forcedSave = false) {
			int infoCount = chapterInfo.Length;
			showInfoWindow = new bool[infoCount];
			chapterList = new string[infoCount];

			if (forcedSave) {
				chapterListCount = chapterList.Length;
			}

			for (int i = 0; i < infoCount; i++) {
				showInfoWindow[i] = true;
				chapterList[i] = chapterInfo[i].chapterName;
			}
		}

		public override void SavePanel() {
			RepaintPanel(true);
		}

		public void ChapterWindow() {
			EditorGUILayout.BeginVertical(
				"box", 
				GUILayout.Width(VNToolkitConstants.CHAPTER_WINDOW_FIXED_WIDTH)
			);

			if (GUILayout.Button("Chapter", EditorStyles.boldLabel)) {
				selectedChapterIndx = -1;
			}

			EditorGUILayout.BeginVertical();
			EditorGUI.indentLevel++;

			EditorGUILayout.BeginHorizontal("toolbar");
			if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(50f))) {
				int counter = chapterList.Length;
				ArrayUtility.Add<bool>(ref showInfoWindow, true);

				VNToolkitChapterData chapterData = new VNToolkitChapterData();
				chapterData.chapterId = counter;
				chapterData.chapterName = string.Format("Chapter_{0}", counter);
				ArrayUtility.Add<VNToolkitChapterData>(ref chapterInfo,	chapterData);
				ArrayUtility.Add<string>(ref chapterList, chapterData.chapterName);

				selectedChapterIndx = counter;
				VNToolkitPanelManager.SharedInstance.Repaint(VNToolkitControlName.FOCUSED_CHAPTER_INFO_PANEL);

				VNToolkitNotifierEditor.TriggerNotification("Chapter Added!");
			}

			if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.Width(50f))) {
				if (selectedChapterIndx > -1) {
					int oldChapterIndx = selectedChapterIndx;
					ArrayUtility.RemoveAt<bool>(ref showInfoWindow, selectedChapterIndx);
					ArrayUtility.RemoveAt<VNToolkitChapterData>(ref chapterInfo, selectedChapterIndx);
					ArrayUtility.RemoveAt<string>(ref chapterList, selectedChapterIndx);

					selectedChapterIndx = oldChapterIndx - 1;
					VNToolkitPanelManager.SharedInstance.Repaint(VNToolkitControlName.FOCUSED_CHAPTER_INFO_PANEL);
					VNToolkitNotifierEditor.TriggerNotification("Chapter Removed!");
				}
				else {
					VNToolkitNotifierEditor.TriggerNotification("No Chapter Selected!");
				}
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			chapterScrollView = EditorGUILayout.BeginScrollView(chapterScrollView);
			int oldIndx = selectedChapterIndx;
			GUI.SetNextControlName(VNToolkitControlName.FOCUSED_CHAPTER_TOGGLE);
			selectedChapterIndx = GUILayout.SelectionGrid(selectedChapterIndx, chapterList, 1, EditorStyles.toolbarButton);
			if (oldIndx != selectedChapterIndx) {
				GUI.FocusControl(VNToolkitControlName.FOCUSED_CHAPTER_TOGGLE);
				VNToolkitPanelManager.SharedInstance.Repaint(VNToolkitControlName.FOCUSED_CHAPTER_INFO_PANEL);
			}
			EditorGUILayout.EndScrollView();

			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
		}
	}
}