using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;

		public enum VN_EditorState {
			START,
			MAIN
		}

		public class VNMainEditor : EditorWindow {

			// Public Variables

			// Private Variables
			private string versionNumber;
			private bool enabled;

			// Static Variables
			public static VNMainEditor vnWindow;

			[MenuItem(VNConstants.MENU_PATH + VNConstants.MENU_TITLE)]
			private static void InitializeWindow() {
				Debug.Log("VN Toolkit Window Initialized!");
				vnWindow = (VNMainEditor)EditorWindow.GetWindow(typeof(VNMainEditor));
				vnWindow.title = VNConstants.MENU_TITLE;
				vnWindow.SetWindowResolution(VNConstants.WINDOW_START_WIDTH, VNConstants.WINDOW_START_HEIGHT);
				
				vnWindow.Show();
			}

			private void OnEnable() {
				Debug.Log("VN Toolkit Window Enabled!");
				hideFlags = HideFlags.HideAndDontSave;
				versionNumber = File.ReadAllText(Application.dataPath + VNConstants.VERSION_PATH);
				VNPanelManager.Initialize(Repaint);
				enabled = true;
			}

			private void OnDisable() {
				Debug.Log("VN Toolkit Window Disabled!");
				enabled = false;
			}

			private void OnGUI() {
				VNPanelManager.DrawPanels(position);
				WindowFooter();

				if (GUI.changed && enabled) EditorUtility.SetDirty(vnWindow);
			}

			private void WindowFooter() {
				GUILayout.BeginArea(new Rect(0f, position.height - 20f, position.width, 20f), EditorStyles.textField);

				GUILayout.BeginArea(new Rect(0f, 0f, 150f, 20f), EditorStyles.textField);
				EditorGUI.DropShadowLabel(new Rect(0f, 0f, 150f, 15f), versionNumber);
				GUILayout.EndArea();

				GUILayout.EndArea();
			}

			public void SetWindowResolution(float width, float height) {
				vnWindow.position = new Rect(Screen.width * 0.1f, Screen.height * 0.1f, width, height);
				vnWindow.minSize = new Vector2(width, height);
			}
		}
	}
}