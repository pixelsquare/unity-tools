using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility;
using UnityEditor;

namespace VNToolkit.VNEditor {

	public class VNChapterEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private VNChapterListEditor vnChapterListEditor;
		private VNChapterInfoEditor vnChapterInfoEditor;

		// Static Variables 
		public override void OnEnable() {
			if (vnChapterListEditor == null) {
				vnChapterListEditor = ScriptableObject.CreateInstance<VNChapterListEditor>();
			}

			if (vnChapterInfoEditor == null) {
				vnChapterInfoEditor = ScriptableObject.CreateInstance<VNChapterInfoEditor>();
			}

			AddChildren(vnChapterListEditor);
			AddChildren(vnChapterInfoEditor);
		}

		public override void OnDisable() {
			base.OnDisable();

			ScriptableObject.Destroy(vnChapterListEditor);
			ScriptableObject.Destroy(vnChapterInfoEditor);
		}

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_CHAPTER_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_CHAPTER; }
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
			get { return ChapterWindow; }
		}

		public override void OnPanelEnable(UnityEngine.Events.UnityAction repaint) {
			base.OnPanelEnable(repaint);
			vnChapterListEditor.OnPanelEnable(repaint);
			vnChapterInfoEditor.OnPanelEnable(repaint);
		}

		protected override void PanelOpen() {
			base.PanelOpen();
			VNEditorUtility.SetAllPanelStateRecursively(this, VN_PANELSTATE.OPEN);
			VNEditorUtility.SetAllPanelStateRecursively(this, VN_PANELSTATE.LOAD);
			VNEditorUtility.SetAllPanelStateRecursively(this, VN_PANELSTATE.REFRESH);
		}

		private void ChapterWindow(Rect position) {
			EditorGUILayout.BeginHorizontal();
			vnChapterListEditor.OnPanelDraw(position);

			EditorGUILayout.BeginVertical();
			vnChapterInfoEditor.OnPanelDraw(position);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Save", EditorStyles.miniButton, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
				vnChapterListEditor.SaveAllChapters();
				VNEditorUtility.SetAllPanelStateRecursively(this, VN_PANELSTATE.SAVE);
			}

			if (GUILayout.Button("Save All", EditorStyles.miniButton, GUILayout.Width(VNConstants.EDITOR_BUTTON_WIDTH))) {
				vnChapterListEditor.SaveAllChapters();
				VNEditorUtility.SetAllPanelStateRecursively(this, VN_PANELSTATE.SAVE);
				VNDataManager.SharedInstance.SaveData();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();
		}

		# endregion Panel Editor Abstract
	}
}