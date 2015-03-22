using UnityEngine;
using System.Collections;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using UnityEditor;

namespace VNToolkit {
	public class VNToolkitChapterEditor {

		// Public Variables

		// Private Variables
		private static bool initialize = false;

		private static int selectedChapterIndx;
		private static string[] chapterList;
		private static Vector2 chapterScrollView;

		// Static Variables

		private static void Initialize() {
			if (!initialize) {
				selectedChapterIndx = -1;
				chapterList = new string[0];
				initialize = true;
			}
		}

		public static void ChapterWindow(UnityAction repaint) {
			Initialize();

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
				ArrayUtility.Add<string>(ref chapterList, "Chapter " + counter);
				selectedChapterIndx = counter;
				VNToolkitNotifierEditor.TriggerNotification("Chapter Added!");
			}

			if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.Width(50f))) {
				if (selectedChapterIndx > 0) {
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
	}
}