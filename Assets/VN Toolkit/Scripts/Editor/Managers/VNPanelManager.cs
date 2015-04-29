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
		private static VNStartEditor VnStartEditor { get; set; }

		public static void Initialize(UnityAction repaint) {
			if (VnStartEditor == null) {
				VnStartEditor = new VNStartEditor();
			}
			VnStartEditor.OnPanelEnable(repaint);

			vnEditorState = VN_EditorState.START;
		}

		public static void DrawPanels(Rect position) {
			if (vnEditorState == VN_EditorState.START) {
				VnStartEditor.OnPanelDraw(position);

				if (CurrentPanel != VnStartEditor) {
					CurrentPanel = VnStartEditor;
				}
			}
			else if (vnEditorState == VN_EditorState.MAIN) {

			}
		}

		public static void SetEditorState(VN_EditorState state) {
			vnEditorState = state;
		}

		public static VNIPanel GetCurrentChild(string title) {
			return CurrentPanel.GetChild(title);
		}
	}
}