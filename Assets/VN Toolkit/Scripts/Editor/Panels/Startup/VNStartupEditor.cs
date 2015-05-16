using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Events;
using VNToolkit.VNUtility;
using System.Collections.Generic;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	[System.Serializable]
	public class VNStartupEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		[SerializeField] private VNNewProjectEditor vnNewProject;
		[SerializeField] private VNLoadProjectEditor vnLoadProject;
		[SerializeField] private VNProjectSettingsEditor vnProjectSettings;

		private string buttonText;
		private string closeButtonText;
		private Vector2 windowVerticalScroll;

		// Static Variables
		public override void OnEnable() {
			if (vnNewProject == null) {
				vnNewProject = ScriptableObject.CreateInstance<VNNewProjectEditor>();
			}

			if (vnLoadProject == null) {
				vnLoadProject = ScriptableObject.CreateInstance<VNLoadProjectEditor>();
			}

			if (vnProjectSettings == null) {
				vnProjectSettings = ScriptableObject.CreateInstance<VNProjectSettingsEditor>();
			}

			AddChildren(vnNewProject);
			AddChildren(vnLoadProject);
			AddChildren(vnProjectSettings);
		}

		public void SubmitNewProject() {
			if (!vnNewProject.PanelEnabled)
				return;

			VNFileManager.CreateProjectPath(vnNewProject.ProjectDirectory);
			VNPanelManager.GetCurrentChild(VNPanelInfo.PANEL_NEW_PROJECT_NAME).SetPanelState(VN_PANELSTATE.SAVE);
			VNDataManager.SharedInstance.SaveData();
		}

		public void SubmitLoadProject() {
			if (!vnLoadProject.PanelEnabled)
				return;

			if (!vnLoadProject.CanLoad())
				return;

			VNEditorUtility.SetAllPanelStateRecursively(VNPanelManager.CurrentPanel, VN_PANELSTATE.SAVE);

			if (VNDataManager.CompareLoadedData<VNProjectData>()) {
				VNPanelManager.SetEditorState(VN_EditorState.SETUP);
			}
			else {
				if (EditorUtility.DisplayDialog(
					"Changes in Loaded Project Data",
					"Do you want to save changes?",
					"Ok!",
					"Cancel"
				)) {
					VNDataManager.SharedInstance.SaveData();
					VNPanelManager.SetEditorState(VN_EditorState.SETUP);
				}
			}
		}

		public void SetButtonText(string text) {
			buttonText = text;
		}

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_STARTUP_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_STARTUP; }
		}

		protected override bool IsPanelFoldable {
			get { return false; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override bool IsRefreshable {
			get { return true; }
		}

		protected override bool IsScrollable {
			get { return true; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		protected override System.Action<Rect> OnPanelGUI {
			get { return StartupWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);
			buttonText = string.Empty;
			closeButtonText = "Close";

			vnNewProject.OnPanelEnable(repaint);
			vnLoadProject.OnPanelEnable(repaint);
			vnProjectSettings.OnPanelEnable(repaint);
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

		public void StartupWindow(Rect position) {
			vnNewProject.OnPanelDraw(position);
			vnLoadProject.OnPanelDraw(position);
			vnProjectSettings.OnPanelDraw(position);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (vnNewProject.PanelEnabled || vnLoadProject.PanelEnabled) {
				if (GUILayout.Button(buttonText, EditorStyles.miniButton, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
					SubmitNewProject();
					SubmitLoadProject();
				}
			}

			if (GUILayout.Button(closeButtonText, EditorStyles.miniButton, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
				VNMainEditor.VnWindow.Close();
			}
			EditorGUILayout.EndHorizontal();
		}
		# endregion Panel Editor Abstract
	}
}