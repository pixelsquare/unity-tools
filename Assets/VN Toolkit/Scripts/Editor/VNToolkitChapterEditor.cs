using UnityEngine;
using System.Collections;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using UnityEditor;

namespace VNToolkit {
	public class VNToolkitChapterEditor {

		// Public Variables

		// Private Variables
		private static int selectedChapterIndx;
		private static string[] chapterList;
		private static Vector2 chapterScrollView;

		private static AnimBool chapterInfoAnim;
		private static bool[] showInfoWindow;

		public static VNToolkitChapterData[] chapterInfo;

		// Static Variables

		public static void Initialize(UnityAction repaint) {
			selectedChapterIndx = -1;
			chapterList = new string[0];

			chapterInfoAnim = new AnimBool(true);
			chapterInfoAnim.valueChanged.AddListener(repaint);

			showInfoWindow = new bool[0];
			chapterInfo = new VNToolkitChapterData[0];
		}

		public static void SetDirty() {
			int infoCount = chapterInfo.Length;
			showInfoWindow = new bool[infoCount];
			chapterList = new string[infoCount];

			for (int i = 0; i < infoCount; i++) {
				showInfoWindow[i] = true;
				chapterList[i] = chapterInfo[i].chapterName;
			}
		}

		public static void ChapterWindow(UnityAction repaint) {
			EditorGUILayout.BeginVertical(
				"box", 
				GUILayout.Width(VNToolkitConstants.CHAPTER_WINDOW_FIXED_WIDTH), 
				GUILayout.Height(VNToolkitConstants.CHAPTER_WINDOW_FIXED_HEIGHT)
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
				VNToolkitNotifierEditor.TriggerNotification("Chapter Added!");
			}

			if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.Width(50f))) {
				if (selectedChapterIndx > 0) {
					ArrayUtility.RemoveAt<bool>(ref showInfoWindow, selectedChapterIndx);
					ArrayUtility.RemoveAt<VNToolkitChapterData>(ref chapterInfo, selectedChapterIndx);
					ArrayUtility.RemoveAt<string>(ref chapterList, selectedChapterIndx);

					selectedChapterIndx = -1;
					VNToolkitNotifierEditor.TriggerNotification("Chapter Removed!");
				}
				else {
					VNToolkitNotifierEditor.TriggerNotification("No Chapter Selected!");
				}
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			chapterScrollView = EditorGUILayout.BeginScrollView(chapterScrollView);
			selectedChapterIndx = GUILayout.SelectionGrid(selectedChapterIndx, chapterList, 1, EditorStyles.radioButton);
			EditorGUILayout.EndScrollView();

			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();
		}

		public static void ChapterInformationWindow() {
			if (selectedChapterIndx < 0)
				return;

			EditorGUILayout.BeginVertical("box", GUILayout.Width(VNToolkitConstants.CHAPTER_INFO_WINDOW_FIXED_WIDTH));

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Chapter Information", EditorStyles.boldLabel)) {
				showInfoWindow[selectedChapterIndx] = !showInfoWindow[selectedChapterIndx];
			}
			GUILayout.FlexibleSpace();
			GUILayout.Label("ID: " + chapterInfo[selectedChapterIndx].chapterId, EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical();
			chapterInfoAnim.target = showInfoWindow[selectedChapterIndx];
			if (EditorGUILayout.BeginFadeGroup(chapterInfoAnim.faded)) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name", EditorStyles.label, GUILayout.Width(70f));
				chapterInfo[selectedChapterIndx].chapterName = EditorGUILayout.TextArea(chapterInfo[selectedChapterIndx].chapterName, EditorStyles.textArea);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Description", EditorStyles.label, GUILayout.Width(70f));
				chapterInfo[selectedChapterIndx].chapterDesc = EditorGUILayout.TextArea(chapterInfo[selectedChapterIndx].chapterDesc, EditorStyles.textArea);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();
		}
	}
}