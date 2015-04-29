using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Events;
using VNToolkit.VNUtility;
using System.Collections.Generic;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	public class VNStartEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private VNNewProjectEditor vnNewProject;
		private VNLoadProjectEditor vnLoadProject;
		private VNProjectSettingsEditor vnProjectSettings;

		private string buttonText;
		private string closeButtonText;

		// Static Variables
		public void SubmitNewProject() {
			if (!vnNewProject.PanelEnabled)
				return;

			VNFileManager.CreateProjectPath(vnNewProject.ProjectDirectory);
			VNPanelManager.GetCurrentChild(VNPanelInfo.PANEL_NEW_PROJECT_NAME).SetPanelState(VN_PANELSTATE.SAVE);
			VNDataManager.SaveData();
		}

		public void SubmitLoadProject() {
			if (!vnLoadProject.PanelEnabled)
				return;

			VNEditorUtility.UpdateAllPanelRecursively(VNPanelManager.CurrentPanel, VN_PANELSTATE.SAVE);

			if (VNDataManager.CompareLoadedData()) {
				VNPanelManager.SetEditorState(VN_EditorState.MAIN);
			}
			else {
				if (EditorUtility.DisplayDialog(
					"Changes in Loaded Project Data",
					"Do you want to save changes?",
					"Ok!",
					"Cancel"
				)) {
					VNDataManager.SaveData();
					VNPanelManager.SetEditorState(VN_EditorState.MAIN);
				}
			}
		}

		public void SetButtonText(string text) {
			buttonText = text;
		}

		# region Panel Editor Abstract
		public override bool IsPanelFoldable {
			get { return false; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_START_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_START; }
		}

		public override System.Action<Rect> OnPanelGUI {
			get { return StartWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);

			if (vnNewProject == null) {
				vnNewProject = new VNNewProjectEditor();
			}
			vnNewProject.OnPanelEnable(repaint);
			AddChildren(vnNewProject);

			if (vnLoadProject == null) {
				vnLoadProject = new VNLoadProjectEditor();
			}
			vnLoadProject.OnPanelEnable(repaint);
			AddChildren(vnLoadProject);

			if (vnProjectSettings == null) {
				vnProjectSettings = new VNProjectSettingsEditor();
			}
			vnProjectSettings.OnPanelEnable(repaint);
			AddChildren(vnProjectSettings);

			buttonText = string.Empty;
			closeButtonText = "Close";
		}

		protected override void PanelOpen() {
			base.PanelOpen();
		}

		protected override void PanelClose() {
			base.PanelClose();
		}

		protected override void PanelSave() {
			base.PanelSave();
		}

		protected override void PanelLoad() {
			base.PanelLoad();
		}

		protected override void PanelClear() {
			base.PanelClear();
			buttonText = string.Empty;
			closeButtonText = string.Empty;
		}

		protected override void PanelReset() {
			base.PanelReset();
			closeButtonText = "Close";
		}

		public void StartWindow(Rect position) {
			vnNewProject.OnPanelDraw(position);
			vnLoadProject.OnPanelDraw(position);
			vnProjectSettings.OnPanelDraw(position);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (vnNewProject.PanelEnabled || vnLoadProject.PanelEnabled) {
				if (GUILayout.Button(buttonText, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
					SubmitNewProject();
					SubmitLoadProject();
				}
			}

			if (GUILayout.Button(closeButtonText, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
				VNMainEditor.vnWindow.Close();
			}
			EditorGUILayout.EndHorizontal();
		}
		# endregion Panel Editor Abstract
	}
}