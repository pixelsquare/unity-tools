using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	public enum VN_EditorState {
		STARTUP,
		SETUP
	}

	public class VNMainEditor : EditorWindow {

		// Public Variables

		// Private Variables
		private string versionNumber;
		private bool enabled;

		// Static Variables
		private static VNMainEditor vnWindow;
		public static VNMainEditor VnWindow {
			get {
				if (vnWindow == null) {
					InitializeWindow();
				}

				return vnWindow;
			}
		}

		[MenuItem(VNConstants.MENU_PATH + VNConstants.MENU_TITLE)]
		private static void InitializeWindow() {
			Debug.Log("VN Toolkit Window Initialized!");
			vnWindow = (VNMainEditor)EditorWindow.GetWindow(typeof(VNMainEditor));
			vnWindow.title = VNConstants.MENU_TITLE;
			vnWindow.Show();
		}

		private void OnEnable() {
			Debug.Log("VN Toolkit Window Enabled!");
			hideFlags = HideFlags.HideAndDontSave;
			versionNumber = File.ReadAllText(Application.dataPath + "/" + VNConstants.VERSION_PATH);

			VNPanelManager.Initialize(Repaint);
			VNPanelManager.SetEditorState(VN_EditorState.SETUP);

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
			GUILayout.BeginArea(new Rect(0f, position.height - VNConstants.FOOTER_HEIGHT, position.width, VNConstants.FOOTER_HEIGHT), EditorStyles.textField);

			GUILayout.BeginArea(new Rect(0f, 0f, 150f, VNConstants.FOOTER_HEIGHT), EditorStyles.textField);
			EditorGUI.DropShadowLabel(new Rect(0f, 0f, 150f, 15f), versionNumber);
			GUILayout.EndArea();

			GUILayout.EndArea();
		}

		public void SetWindowResolution(float width, float height) {
			vnWindow.position = new Rect(Screen.currentResolution.width * 0.1f, Screen.currentResolution.height * 0.05f, width, height);
			vnWindow.minSize = new Vector2(width, height);
		}
	}
}