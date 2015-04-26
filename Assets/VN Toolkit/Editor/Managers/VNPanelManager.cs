using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;

		public class VNPanelManager {

			// Public Variables
			public static VNIPanel CurrentPanel { get; set; }
			public static VN_EditorState VnEditorState { get { return vnEditorState; } }

			// Private Variables
			private static VN_EditorState vnEditorState;
			private static VNStartEditor VnStartEditor { get; set; }
			
			// Static Variables

			public static void Initialize(UnityAction repaint) {
				if (VnStartEditor == null) {
					VnStartEditor = new VNStartEditor();
				}
				VnStartEditor.OnEditorEnable(repaint);

				vnEditorState = VN_EditorState.START;
			}

			public static void DrawPanels(Rect position) {
				if (vnEditorState == VN_EditorState.START) {
					VnStartEditor.OnEditorDraw(position);

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
		}
	}
}