using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;

		public class VNProjectSettingsEditor : VNPlainPanelAbstract {

			// Public Variables

			// Private Variables
			private string pixelsPerUnitText = string.Empty;

			private VNResolutionEditor vnResolution;

			// Static Variables

			public override bool IsPanelFoldable {
				get { return true; }
			}

			public override string PanelTitle {
				get { return VNConstants.PANEL_PROJECT_SETTINGS_NAME; }
			}

			public override float PanelWidth {
				get { return VNConstants.PANEL_PROJECT_SETTINGS_WIDTH; }
			}

			public override string PanelControlName {
				get { return VNControlName.FOCUSED_PANEL_PROJECT_SETTINGS; }
			}

			public override System.Action<Rect> OnEditorGUI {
				get { return ProjectSettingsWindow; }
			}

			public override void OnEditorEnable(UnityAction repaint) {
				base.OnEditorEnable(repaint);
				if (vnResolution == null) {
					vnResolution = new VNResolutionEditor();
				}
				vnResolution.OnEditorEnable(repaint);
				AddChildren(vnResolution);

				pixelsPerUnitText = string.Empty;
			}

			public override void PanelOpen() {
				base.PanelOpen();
			}

			public override void PanelClose() {
				base.PanelClose();
			}

			public override void PanelSave() {
				base.PanelSave();
				VNDataManager.VnProjectData.projectPixelsPerUnit = int.Parse(pixelsPerUnitText);
			}

			public override void PanelLoad() {
				base.PanelLoad();
				pixelsPerUnitText = VNDataManager.VnProjectData.projectPixelsPerUnit.ToString();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelSave();
			}

			public override void PanelClear() {
				base.PanelClear();
				pixelsPerUnitText = string.Empty;
			}

			public override void PanelReset() {
				base.PanelReset();
				pixelsPerUnitText = VNConstants.CAMERA_DEFAULT_PIXELS_PER_UNIT.ToString();
			}

			private void ProjectSettingsWindow(Rect position) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Pixels per Unit", EditorStyles.label, GUILayout.Width(VNConstants.EDITOR_LABEL_WIDTH));
				pixelsPerUnitText = EditorGUILayout.TextField(pixelsPerUnitText, EditorStyles.textField);
				EditorGUILayout.EndHorizontal();
				vnResolution.OnEditorDraw(position);
			}
		}
	}
}