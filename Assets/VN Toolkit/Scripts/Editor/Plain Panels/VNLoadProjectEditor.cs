using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using UnityEngine.Events;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	public class VNLoadProjectEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private string projectName;
		private string projectDirectory;
		private Object projectFolder;

		// Static Variables

		public void UpdateProjectData() {
			if (projectDirectory != AssetDatabase.GetAssetPath(projectFolder)) {
				projectDirectory = AssetDatabase.GetAssetPath(projectFolder);

				VNFileManager.LoadProjectPath(projectDirectory);

				if (VNFileManager.IsPathValid()) {
					VNDataManager.LoadData();
				}
			}
		}

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
			get { return VNPanelInfo.PANEL_LOAD_PROJECT_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_LOAD_PROJECT; }
		}

		public override System.Action<Rect> OnPanelGUI {
			get { return LoadProjectWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);
			projectName = string.Empty;
			projectDirectory = string.Empty;
			projectFolder = null;
		}

		protected override void PanelOpen() {
			base.PanelOpen();
			parent.GetChild(VNPanelInfo.PANEL_NEW_PROJECT_NAME).SetPanelState(VN_PANELSTATE.CLOSE);
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.OPEN);
			VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.OPEN);

			if (projectFolder != null && VNFileManager.IsPathValid()) {
				UpdateProjectData();
				PanelLoad();
				parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.LOAD);
				VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.LOAD);
			}
			else {
				PanelClear();
				parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.CLEAR);
				VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.CLEAR);
			}

			if (VNPanelManager.VnEditorState == VN_EditorState.START) {
				VNStartEditor startEditor = VNPanelManager.CurrentPanel as VNStartEditor;
				startEditor.SetButtonText("Load Project");
			}
		}

		protected override void PanelClose() {
			base.PanelClose();
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.CLEAR);
			VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.CLEAR);
		}

		protected override void PanelSave() {
			base.PanelSave();
			VNDataManager.VnProjectData.projectName = projectName;
			parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.SAVE);
			VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.SAVE);
		}

		protected override void PanelLoad() {
			base.PanelLoad();
			projectName = VNDataManager.VnProjectData.projectName;
		}

		protected override void PanelClear() {
			base.PanelClear();
			projectName = string.Empty;
			projectDirectory = string.Empty;
			//projectFolder = null;
		}

		protected override void PanelReset() {
			base.PanelReset();
		}

		public void LoadProjectWindow(Rect position) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Project Name", EditorStyles.label, GUILayout.Width(VNConstants.EDITOR_LABEL_WIDTH));
			projectName = EditorGUILayout.TextField(projectName, EditorStyles.textField);
			EditorGUILayout.EndHorizontal();

			Object folder = projectFolder;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Project Folder", EditorStyles.label, GUILayout.Width(VNConstants.EDITOR_LABEL_WIDTH));
			projectFolder = EditorGUILayout.ObjectField(projectFolder, typeof(Object), false);
			EditorGUILayout.EndHorizontal();

			//GUILayout.Button(string.Empty, EditorStyles.miniButtonMid, GUILayout.Width(10f), GUILayout.Height(10f));

			if (projectFolder != null) {
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Clear", GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
					projectFolder = null;
				}
				EditorGUILayout.EndHorizontal();
			}

			if (folder != projectFolder) {
				if (projectFolder != null) {
					UpdateProjectData();
					PanelLoad();
					parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.LOAD);
					VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.LOAD);
				}

				if (!VNFileManager.IsPathValid() || projectFolder == null) {
					PanelClear();
					parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME).SetPanelState(VN_PANELSTATE.CLEAR);
					VNEditorUtility.UpdateAllPanelRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.CLEAR);
				}
			}
		}
		# endregion Panel Editor Abstract
	}
}