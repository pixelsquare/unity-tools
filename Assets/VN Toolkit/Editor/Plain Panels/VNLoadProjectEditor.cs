using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;
		using System.IO;

		public class VNLoadProjectEditor : VNPlainPanelAbstract {

			// Public Variables

			// Private Variables
			private string projectName;
			private string projectDirectory;
			private Object projectFolder;

			// Static Variables

			public override bool IsPanelFoldable {
				get { return true; }
			}

			public override string PanelTitle {
				get { return VNConstants.PANEL_LOAD_PROJECT_NAME; }
			}

			public override float PanelWidth {
				get { return VNConstants.PANEL_LOAD_PROJECT_WIDTH; }
			}

			public override string PanelControlName {
				get { return VNControlName.FOCUSED_PANEL_LOAD_PROJECT; }
			}

			public override System.Action<Rect> WindowGUI {
				get { return LoadProjectWindow; }
			}

			public override void Initialize(UnityAction repaint) {
				base.Initialize(repaint);
				projectName = string.Empty;
				projectDirectory = string.Empty;
				projectFolder = null;
			}

			public override void PanelOpen() {
				base.PanelOpen();
				parent.GetChild(VNConstants.PANEL_NEW_PROJECT_NAME).PanelClose();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelOpen();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).GetChildren().ForEach(a => a.PanelOpen());

				if (projectFolder != null && VNFileManager.IsPathValid()) {
					UpdateProjectData();
					PanelLoad();
					parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelLoad();
				}
				else {
					PanelClear();
					parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelClear();
				}

				if (VNPanelManager.VnEditorState == VNEditorState.START) {
					VNStartEditor startEditor = VNPanelManager.CurrentPanel as VNStartEditor;
					startEditor.SetButtonText("Load Project");
				}
			}

			public override void PanelClose() {
				base.PanelClose();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelClear();
			}

			public override void PanelSave() {
				base.PanelSave();
				VNDataManager.VnProjectData.projectName = projectName;
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelSave();
			}

			public override void PanelLoad() {
				base.PanelLoad();
				projectName = VNDataManager.VnProjectData.projectName;
			}

			public override void PanelClear() {
				base.PanelClear();
				projectName = string.Empty;
				projectDirectory = string.Empty;
				//projectFolder = null;
			}

			public override void PanelReset() {
				base.PanelReset();
			}

			public void LoadProjectWindow(Rect position) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Project Name", EditorStyles.label, GUILayout.Width(VNConstants.WINDOW_LABEL_WIDTH));
				projectName = EditorGUILayout.TextField(projectName, EditorStyles.textField);
				EditorGUILayout.EndHorizontal();

				Object folder = projectFolder;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Project Folder", EditorStyles.label, GUILayout.Width(VNConstants.WINDOW_LABEL_WIDTH));
				projectFolder = EditorGUILayout.ObjectField(projectFolder, typeof(Object), false);
				EditorGUILayout.EndHorizontal();

				if (folder != projectFolder) {
					if (projectFolder != null) {
						UpdateProjectData();
						PanelLoad();
						parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelLoad();
					}
					
					if(!VNFileManager.IsPathValid() || projectFolder == null) {
						PanelClear();
						parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelClear();
					}
				}
			}

			public void UpdateProjectData() {
				if (projectDirectory != AssetDatabase.GetAssetPath(projectFolder)) {
					projectDirectory = AssetDatabase.GetAssetPath(projectFolder);

					VNFileManager.LoadProjectPath(projectDirectory);

					if (VNFileManager.IsPathValid()) {
						VNDataManager.LoadData();
					}
				}
			}
		}
	}
}