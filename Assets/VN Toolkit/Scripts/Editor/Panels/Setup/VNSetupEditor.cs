using UnityEngine;
using System.Collections;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;
using UnityEngine.Events;

namespace VNToolkit.VNEditor {

	public class VNSetupEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private VNChapterEditor vnChapterEditor;

		// Static Variables 

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_SETUP_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_SETUP; }
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
			get { return SetupWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);

			if (vnChapterEditor == null) {
				vnChapterEditor = new VNChapterEditor();
			}
			vnChapterEditor.OnPanelEnable(repaint);
			AddChildren(vnChapterEditor);
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
		}

		protected override void PanelReset() {
			base.PanelReset();
		}

		protected override void PanelRefresh() {
			base.PanelRefresh();
		}

		private void SetupWindow(Rect position) {
			vnChapterEditor.OnPanelDraw(position);
		}

		# endregion Panel Editor Abstract
	}
} 