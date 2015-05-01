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

			if (vnChapterListEditor == null) {
				vnChapterListEditor = new VNChapterListEditor();
			}
			AddChildren(vnChapterListEditor);
			vnChapterListEditor.OnPanelEnable(repaint);

			if(vnChapterInfoEditor == null) {
				vnChapterInfoEditor = new VNChapterInfoEditor();
			}
			AddChildren(vnChapterInfoEditor);
			vnChapterInfoEditor.OnPanelEnable(repaint);
		}

		private void ChapterWindow(Rect position) {
			EditorGUILayout.BeginHorizontal();
			vnChapterListEditor.OnPanelDraw(position);
			vnChapterInfoEditor.OnPanelDraw(position);
			EditorGUILayout.EndHorizontal();
		}

		# endregion Panel Editor Abstract
	}
}