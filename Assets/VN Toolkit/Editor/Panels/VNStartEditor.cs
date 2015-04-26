using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

namespace VNToolkit {
	namespace VNEditor {
		using VNUtility;
		using System.Collections.Generic;

		public class VNStartEditor : VNPanelAbstract {

			// Public Variables

			// Private Variables
			private VNNewProjectEditor vnNewProject;
			private VNLoadProjectEditor vnLoadProject;
			private VNProjectSettingsEditor vnProjectSettings;

			private string buttonText;
			private string closeButtonText;

			// Static Variables

			public override bool IsPanelFoldable {
				get { return false; }
			}

			public override string PanelTitle {
				get { return VNConstants.PANEL_START_NAME; }
			}

			public override float PanelWidth {
				get { return VNConstants.PANEL_START_WIDTH; }
			}

			public override string PanelControlName {
				get { return VNControlName.FOCUSED_PANEL_START; }
			}

			public override System.Action<Rect> OnEditorGUI {
				get { return StartWindow; }
			}

			public override void OnEditorEnable(UnityAction repaint) {
				base.OnEditorEnable(repaint);

				if (vnNewProject == null) {
					vnNewProject = new VNNewProjectEditor();
				}
				vnNewProject.OnEditorEnable(repaint);
				AddChildren(vnNewProject);

				if (vnLoadProject == null) {
					vnLoadProject = new VNLoadProjectEditor();
				}
				vnLoadProject.OnEditorEnable(repaint);
				AddChildren(vnLoadProject);

				if (vnProjectSettings == null) {
					vnProjectSettings = new VNProjectSettingsEditor();
				}
				vnProjectSettings.OnEditorEnable(repaint);
				AddChildren(vnProjectSettings);

				//vnNewProject.PanelOpen();

				buttonText = string.Empty;
				closeButtonText = "Close";
			}

			public override void PanelOpen() {
				base.PanelOpen();
			}

			public override void PanelClose() {
				base.PanelClose();
			}

			public override void PanelSave() {
				base.PanelSave();
			}

			public override void PanelLoad() {
				base.PanelLoad();
			}

			public override void PanelClear() {
				base.PanelClear();
				buttonText = string.Empty;
				closeButtonText = string.Empty;
			}

			public override void PanelReset() {
				base.PanelReset();
				closeButtonText = "Close";
			}

			public void StartWindow(Rect position) {
				vnNewProject.OnEditorDraw(position);
				vnLoadProject.OnEditorDraw(position);
				vnProjectSettings.OnEditorDraw(position);

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

			public void SubmitNewProject() {
				if (!vnNewProject.PanelEnabled)
					return;

				VNFileManager.CreateProjectPath(vnNewProject.ProjectDirectory);
				VNPanelManager.CurrentPanel.GetChild(VNConstants.PANEL_NEW_PROJECT_NAME).PanelSave();
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
		}
	}
}