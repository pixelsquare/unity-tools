using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;

		public class VNResolutionEditor : VNPlainPanelAbstract {

			// Public Variables

			// Private Variables
			private string widthText;
			private string heightText;

			// Static Variables

			public override bool IsPanelFoldable {
				get { return true; }
			}

			public override string PanelTitle {
				get { return VNConstants.PANEL_RESOLUTION_NAME; }
			}

			public override float PanelWidth {
				get { return VNConstants.PANEL_RESOLUTION_WIDTH; }
			}

			public override string PanelControlName {
				get { return VNControlName.FOCUSED_PANEL_RESOLUTION; }
			}

			public override System.Action<Rect> WindowGUI {
				get { return ResolutionWindow; }
			}

			public override void Initialize(UnityAction repaint) {
				base.Initialize(repaint);

				widthText = string.Empty;
				heightText = string.Empty;
			}

			public override void PanelOpen() {
				base.PanelOpen();
			}

			public override void PanelClose() {
				base.PanelClose();
			}

			public override void PanelSave() {
				base.PanelSave();
				VNDataManager.VnProjectData.projectWidth = int.Parse(widthText);
				VNDataManager.VnProjectData.projectHeight = int.Parse(heightText);
			}

			public override void PanelLoad() {
				base.PanelLoad();
				widthText = VNDataManager.VnProjectData.projectWidth.ToString();
				heightText = VNDataManager.VnProjectData.projectHeight.ToString();
			}

			public override void PanelClear() {
				base.PanelClear();
				widthText = string.Empty;
				heightText = string.Empty;
			}

			public override void PanelReset() {
				base.PanelReset();
				widthText = VNConstants.WINDOW_DEFAULT_WIDTH.ToString();
				heightText = VNConstants.WINDOW_DEFAULT_HEIGHT.ToString();
			}

			private void ResolutionWindow(Rect position) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Width", GUILayout.Width(VNConstants.WINDOW_LABEL_WIDTH));
				widthText = EditorGUILayout.TextField(widthText, EditorStyles.textField);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Height", GUILayout.Width(VNConstants.WINDOW_LABEL_WIDTH));
				heightText = EditorGUILayout.TextField(heightText, EditorStyles.textField);
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}