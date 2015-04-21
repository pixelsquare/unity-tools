using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;
using System.IO;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;

		public class VNNewProjectEditor : VNPlainPanelAbstract {

			// Public Variables

			// Private Variables
			private string projectName;
			private string projectDirectory;
			public string ProjectDirectory {
				get { return projectDirectory; }
			}

			// Static Variables

			public override bool IsPanelFoldable {
				get { return true; }
			}

			public override string PanelTitle {
				get { return VNConstants.PANEL_NEW_PROJECT_NAME; }
			}

			public override float PanelWidth {
				get { return VNConstants.PANEL_NEW_PROJECT_WIDTH; }
			}

			public override string PanelControlName {
				get { return VNControlName.FOCUSED_PANEL_NEW_PROJECT; }
			}

			public override System.Action<Rect> WindowGUI {
				get { return NewProjectWindow; }
			}

			public override void Initialize(UnityAction repaint) {
				base.Initialize(repaint);
				projectName = string.Empty;
				projectDirectory = string.Empty;
			}

			public override void PanelOpen() {
				base.PanelOpen();
				parent.GetChild(VNConstants.PANEL_LOAD_PROJECT_NAME).PanelClose();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelOpen();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).GetChildren().ForEach(a => a.PanelOpen());

				PanelReset();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelReset();

				if (VNPanelManager.VnEditorState == VNEditorState.START) {
					VNStartEditor startEditor = VNPanelManager.CurrentPanel as VNStartEditor;
					startEditor.SetButtonText("New Project");
				}
			}

			public override void PanelClose() {
				base.PanelClose();
				parent.GetChild(VNConstants.PANEL_PROJECT_SETTINGS_NAME).PanelClear();
			}

			public override void PanelSave() {
				base.PanelSave();
				VNDataManager.VnProjectData.projectName = projectName;
			}

			public override void PanelLoad() {
				base.PanelLoad();
				projectName = VNDataManager.VnProjectData.projectName;
			}

			public override void PanelClear() {
				base.PanelClear();
				projectName = string.Empty;
				projectDirectory = string.Empty;
			}

			public override void PanelReset() {
				base.PanelReset();
				projectName = VNConstants.NEW_PROJECT_NAME;
				projectDirectory = Application.dataPath + VNConstants.PROJECT_PATH + "/" + projectName;
				GetProjectNameRecursively(0);
			}

			public void NewProjectWindow(Rect position) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Project Name", EditorStyles.label, GUILayout.Width(VNConstants.WINDOW_LABEL_WIDTH));
				projectName = EditorGUILayout.TextField(projectName, EditorStyles.textField);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Project Directory", EditorStyles.label, GUILayout.Width(VNConstants.WINDOW_LABEL_WIDTH));
				projectDirectory = Application.dataPath + VNConstants.PROJECT_PATH + "/" + projectName;
				projectDirectory = EditorGUILayout.TextField(projectDirectory, EditorStyles.textField);
				EditorGUILayout.EndHorizontal();
			}

			private void GetProjectNameRecursively(int indx) {
				projectDirectory = Application.dataPath + VNConstants.PROJECT_PATH + "/" + projectName;
				if (Directory.Exists(projectDirectory)) {
					indx++;
					projectName = string.Format(VNConstants.NEW_PROJECT_FORMAT, indx);
					GetProjectNameRecursively(indx);
				}
			}
		}
	}
}