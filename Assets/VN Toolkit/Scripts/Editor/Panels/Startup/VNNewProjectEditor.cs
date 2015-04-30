using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Events;
using System.Collections;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	public class VNNewProjectEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private string projectName;
		private string projectDirectory;
		public string ProjectDirectory {
			get { return projectDirectory; }
		}

		// Static Variables
		
		private void GetProjectNameRecursively(int indx) {
			projectDirectory = Application.dataPath + VNConstants.PROJECT_PATH + "/" + projectName;
			if (Directory.Exists(projectDirectory)) {
				indx++;
				projectName = string.Format(VNConstants.NEW_PROJECT_FORMAT, indx);
				GetProjectNameRecursively(indx);
			}
		}

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_NEW_PROJECT_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_NEW_PROJECT; }
		}


		protected override bool IsPanelFoldable {
			get { return true; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override bool IsRefreshable {
			get { return true; }
		}

		protected override bool IsScrollable {
			get { return false; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		protected override System.Action<Rect> OnPanelGUI {
			get { return NewProjectWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);
			projectName = string.Empty;
			projectDirectory = string.Empty;
		}

		protected override void PanelOpen() {
			base.PanelOpen();
			parent.GetChild(VNPanelInfo.PANEL_LOAD_PROJECT_NAME).SetPanelState(VN_PANELSTATE.CLOSE);
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.OPEN);
			VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.OPEN);

			PanelReset();
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.RESET);
			VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.RESET);

			if (VNPanelManager.VnEditorState == VN_EditorState.STARTUP) {
				VNStartupEditor startEditor = VNPanelManager.CurrentPanel as VNStartupEditor;
				startEditor.SetButtonText("New Project");
			}
		}

		protected override void PanelClose() {
			base.PanelClose();
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.CLEAR);
			VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.CLEAR);
		}

		protected override void PanelSave() {
			base.PanelSave();
			VNDataManager.SharedInstance.VnProjectData.projectName = projectName;
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.SAVE);
			VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.SAVE);
		}

		protected override void PanelLoad() {
			base.PanelLoad();
			projectName = VNDataManager.SharedInstance.VnProjectData.projectName;
		}

		protected override void PanelClear() {
			base.PanelClear();
			projectName = string.Empty;
			projectDirectory = string.Empty;
		}

		protected override void PanelReset() {
			base.PanelReset();
			projectName = VNConstants.NEW_PROJECT_NAME;
			projectDirectory = Application.dataPath + VNConstants.PROJECT_PATH + "/" + projectName;
			GetProjectNameRecursively(0);
		}

		protected override void PanelRefresh() {
			projectName = string.Empty;
			projectDirectory = string.Empty;

			base.PanelRefresh();
		}

		public void NewProjectWindow(Rect position) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Project Name", EditorStyles.label, GUILayout.Width(VNConstants.EDITOR_LABEL_WIDTH));
			projectName = EditorGUILayout.TextField(projectName, EditorStyles.textField);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Project Directory", EditorStyles.label, GUILayout.Width(VNConstants.EDITOR_LABEL_WIDTH));
			projectDirectory = Application.dataPath + VNConstants.PROJECT_PATH + "/" + projectName;
			projectDirectory = EditorGUILayout.TextField(projectDirectory, EditorStyles.textField);
			EditorGUILayout.EndHorizontal();
		}
		# endregion Panel Editor Abstract
	}
}