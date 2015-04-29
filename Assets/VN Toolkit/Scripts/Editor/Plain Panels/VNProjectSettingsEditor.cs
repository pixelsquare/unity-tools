using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	public class VNProjectSettingsEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private string pixelsPerUnitText = string.Empty;

		private VNResolutionEditor vnResolution;

		// Static Variables

		# region Panel Editor Abstract
		public override bool IsPanelFoldable {
			get { return true; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_PROJECT_SETTINGS; }
		}

		public override System.Action<Rect> OnPanelGUI {
			get { return ProjectSettingsWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);
			if (vnResolution == null) {
				vnResolution = new VNResolutionEditor();
			}
			vnResolution.OnPanelEnable(repaint);
			AddChildren(vnResolution);

			pixelsPerUnitText = string.Empty;
		}

		protected override void PanelOpen() {
			base.PanelOpen();
		}

		protected override void PanelClose() {
			base.PanelClose();
		}

		protected override void PanelSave() {
			base.PanelSave();
			VNDataManager.VnProjectData.projectPixelsPerUnit = int.Parse(pixelsPerUnitText);
		}

		protected override void PanelLoad() {
			base.PanelLoad();
			pixelsPerUnitText = VNDataManager.VnProjectData.projectPixelsPerUnit.ToString();
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.SAVE);
		}

		protected override void PanelClear() {
			base.PanelClear();
			pixelsPerUnitText = string.Empty;
		}

		protected override void PanelReset() {
			base.PanelReset();
			pixelsPerUnitText = VNConstants.CAMERA_DEFAULT_PIXELS_PER_UNIT.ToString();
		}

		private void ProjectSettingsWindow(Rect position) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Pixels per Unit", EditorStyles.label, GUILayout.Width(VNConstants.EDITOR_LABEL_WIDTH));
			pixelsPerUnitText = EditorGUILayout.TextField(pixelsPerUnitText, EditorStyles.textField);
			EditorGUILayout.EndHorizontal();
			vnResolution.OnPanelDraw(position);
		}
		# endregion Panel Editor Abstract
	}
}