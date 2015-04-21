using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;

		public class VNPanelManager {

			// Public Variables
			public static VNIGUI CurrentPanel { get; set; }
			public static VNEditorState VnEditorState { get { return vnEditorState; } }

			// Private Variables
			private static VNEditorState vnEditorState;
			private static VNStartEditor VnStartEditor { get; set; }
			
			// Static Variables

			public static void Initialize(UnityAction repaint) {
				if (VnStartEditor == null) {
					VnStartEditor = new VNStartEditor();
				}
				VnStartEditor.Initialize(repaint);
			}

			public static void DrawPanels(Rect position) {
				if (vnEditorState == VNEditorState.START) {
					VnStartEditor.DrawGUI(position);

					if (CurrentPanel != VnStartEditor) {
						CurrentPanel = VnStartEditor;
					}
				}
				else if (vnEditorState == VNEditorState.MAIN) {

				}
			}

			public static void SetEditorState(VNEditorState state) {
				vnEditorState = state;
			}
		}
	}
}