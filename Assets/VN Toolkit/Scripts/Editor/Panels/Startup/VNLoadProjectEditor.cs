using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using UnityEngine.Events;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	[System.Serializable]
	public class VNLoadProjectEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		[SerializeField] private string projectName;
		[SerializeField] private Object projectFolder;
		private bool autoRefresh;

		//private const string FOLDER_PATH_KEY = "FOLDER_PATH_KEY";

		// Static Variables

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_LOAD_PROJECT_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_LOAD_PROJECT; }
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
			get { return LoadProjectWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);
		}

		protected override void PanelOpen() {
			base.PanelOpen();
			parent.GetChild(VNPanelInfo.PANEL_NEW_PROJECT_NAME).SetPanelState(VN_PANELSTATE.CLOSE);
			VNEditorUtility.SetAllPanelStateRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.OPEN);

			PanelLoad();

			//if(EditorPrefs.HasKey(FOLDER_PATH_KEY)) {
			//    string path = EditorPrefs.GetString(FOLDER_PATH_KEY, string.Empty);
			//    projectFolder = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
			//}

			if (VNPanelManager.VnEditorState == VN_EditorState.STARTUP) {
				VNStartupEditor startEditor = VNPanelManager.CurrentPanel as VNStartupEditor;
				startEditor.SetButtonText("Load Project");
			}
		}

		protected override void PanelClose() {
			base.PanelClose();
			VNEditorUtility.SetAllPanelStateRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.CLOSE);
		}

		protected override void PanelSave() {
			base.PanelSave();
			VNDataManager.SharedInstance.VnProjectData.projectName = projectName;
			VNEditorUtility.SetAllPanelStateRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.SAVE);
		}

		protected override void PanelLoad() {
			if (projectFolder == null)
				return;

			string projectDirectory = AssetDatabase.GetAssetPath(projectFolder);
			VNFileManager.LoadProjectPath(projectDirectory);
			VNDataManager.SharedInstance.LoadData();

			//projectName = VNDataManager.SharedInstance.VnProjectData.projectName;
			VNEditorUtility.SetAllPanelStateRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.LOAD);
			base.PanelLoad();
		}

		protected override void PanelClear() {
			base.PanelClear();

			if (projectFolder != null)
				return;

			projectName = string.Empty;
			VNEditorUtility.SetAllPanelStateRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.CLEAR);
		}

		protected override void PanelReset() {
			base.PanelReset();

			projectName = string.Empty;
			projectFolder = null;
			autoRefresh = false;
			VNEditorUtility.SetAllPanelStateRecursively(parent.GetChild(VNPanelInfo.PANEL_PROJECT_SETTINGS_NAME), VN_PANELSTATE.RESET);
		}

		protected override void PanelRefresh() {
			PanelLoad();
			PanelClear();

			//if(projectFolder != null)
			//    EditorPrefs.SetString(FOLDER_PATH_KEY, AssetDatabase.GetAssetPath(projectFolder));

			base.PanelRefresh();
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

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Auto Refresh", EditorStyles.label, GUILayout.Width(VNConstants.EDITOR_LABEL_WIDTH));
			autoRefresh = EditorGUILayout.Toggle(autoRefresh, EditorStyles.toggle);
			EditorGUILayout.EndHorizontal();

			if (projectFolder != null) {
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Load", EditorStyles.miniButton, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
					PanelLoad();
				}

				if (GUILayout.Button("Clear", EditorStyles.miniButton, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
					projectFolder = null;
					PanelClear();
				}
				EditorGUILayout.EndHorizontal();
			}

			if (folder != projectFolder && autoRefresh) {
				PanelRefresh();
			}
		}
		# endregion Panel Editor Abstract

		public bool CanLoad() {
			return projectFolder != null;
		}
	}
}