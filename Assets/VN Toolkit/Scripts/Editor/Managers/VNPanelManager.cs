using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	public class VNPanelManager {

		// Public Variables

		// Private Variables

		// Static Variables
		public static VNIPanel CurrentPanel { get; set; }
		public static VN_EditorState VnEditorState { get { return vnEditorState; } }

		private static VN_EditorState vnEditorState;
		private static VNStartupEditor VNStartup { get; set; }
		private static VNSetupEditor VNSetup { get; set; }

		private static UnityAction Repaint;

		public static void Initialize(UnityAction repaint) {
			if (VNStartup == null) {
				VNStartup = new VNStartupEditor();
			}

			if (VNSetup == null) {
				VNSetup = new VNSetupEditor();
			}

			Repaint = repaint;
		}

		public static void DrawPanels(Rect position) {
			VNPanelAbstract currentPanel = CurrentPanel as VNPanelAbstract;
			if (currentPanel != null) {
				currentPanel.OnPanelDraw(position);
			}
		}

		public static void SetEditorState(VN_EditorState state) {
			vnEditorState = state;

			if (vnEditorState == VN_EditorState.STARTUP) {
				VNStartup.OnPanelEnable(Repaint);
				SetCurrentPanel(VNStartup);
				VNMainEditor.VnWindow.SetWindowResolution(VNConstants.EDITOR_WINDOW_START_WIDTH, VNConstants.EDITOR_WINDOW_START_HEIGHT);
			}
			else if (vnEditorState == VN_EditorState.SETUP) {
				VNSetup.OnPanelEnable(Repaint);
				SetCurrentPanel(VNSetup);
				VNMainEditor.VnWindow.SetWindowResolution(VNConstants.EDITOR_WINDOW_DEFAULT_WIDTH, VNConstants.EDITOR_WINDOW_DEFAULT_HEIGHT);
			}
		}

		public static VNIPanel GetCurrentChild(string title) {
			return CurrentPanel.GetChild(title);
		}

		private static void SetCurrentPanel(VNIPanel panel) {
			if (CurrentPanel == panel)
				return;

			CurrentPanel = panel;
		}
	}
}