using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEditor.AnimatedValues;
using UnityEditor;

namespace VNToolkit {
	public class VNToolkitSceneEditor {

		// Public Variables

		// Private Variables
		private static bool initialize = false;

		private static int selectedSceneIndx;
		private static string[] sceneList;

		private static Vector2 sceneScrollView;

		// Static Variables

		private static void Initialize() {
			if (!initialize) {
				selectedSceneIndx = -1;
				sceneList = new string[0];
				initialize = true;
			}
		}

		public static void SceneWindow(UnityAction repaint) {
			Initialize();

			EditorGUILayout.BeginVertical(
				"box", 
				GUILayout.Width(VNToolkitConstants.SCENE_WINDOW_FIXED_WIDTH)
			);

			if (GUILayout.Button("Scene", EditorStyles.boldLabel)) {
				selectedSceneIndx = -1;
			}

			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal("toolbar");
			if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(50f))) {
				int counter = sceneList.Length;
				ArrayUtility.Add<string>(ref sceneList, "Scene " + counter);
				selectedSceneIndx = counter;
				VNToolkitNotifierEditor.TriggerNotification("Scene Added!");
			}

			if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.Width(50f))) {
				if (selectedSceneIndx > 0) {
					ArrayUtility.RemoveAt<string>(ref sceneList, selectedSceneIndx);
					selectedSceneIndx = -1;
					VNToolkitNotifierEditor.TriggerNotification("Scene Removed!");
				}
				else {
					VNToolkitNotifierEditor.TriggerNotification("No Scene Selected!");
				}
			}

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			sceneScrollView = EditorGUILayout.BeginScrollView(sceneScrollView);
			selectedSceneIndx = GUILayout.SelectionGrid(selectedSceneIndx, sceneList, 1, EditorStyles.radioButton);
			EditorGUILayout.EndScrollView();

			EditorGUILayout.EndVertical();

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
		}
	}
}