using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace VNToolkit {
	public class VNToolkitPanelManager {

		// Public Variables

		// Private Variables
		private static VNToolkitPanelManager instance;
		public static VNToolkitPanelManager SharedInstance {
			get {
				if (instance == null)
					instance = new VNToolkitPanelManager();

				return instance;
			}
		}

		// Static Variables
		private VNToolkitChapterEditor vnToolkitChapterEditor;
		private VNToolkitChapterInfoEditor vnToolkitChapterInfoEditor;

		private List<VNToolkitPanelAbstract> vnToolkitPanels = new List<VNToolkitPanelAbstract>();

		public void Initialize(UnityAction repaint, Rect windowPos) {
			if (vnToolkitChapterEditor == null) {
				vnToolkitChapterEditor = new VNToolkitChapterEditor();
				vnToolkitPanels.Add(vnToolkitChapterEditor);
			}

			if (vnToolkitChapterInfoEditor == null) {
				vnToolkitChapterInfoEditor = new VNToolkitChapterInfoEditor();
				vnToolkitPanels.Add(vnToolkitChapterInfoEditor);
			}

			for (int i = 0; i < vnToolkitPanels.Count; i++) {
				vnToolkitPanels[i].Initialize(repaint, windowPos);
			}
		}

		public void DrawPanels() {
			for (int i = 0; i < vnToolkitPanels.Count; i++) {
				vnToolkitPanels[i].DrawGUI();
			}
		}

		public void RepaintPanels(bool forcedSave = false) {
			for (int i = 0; i < vnToolkitPanels.Count; i++) {
				vnToolkitPanels[i].RepaintPanel(forcedSave);
			}
		}

		public void SavePanels() {
			for (int i = 0; i < vnToolkitPanels.Count; i++) {
				vnToolkitPanels[i].SavePanel();
			}
		}

		public void Repaint(string controlName) {
			int panelIndx = vnToolkitPanels.FindIndex(p => p.ControlName == controlName);
			vnToolkitPanels[panelIndx].RepaintPanel();
		}

		public VNToolkitPanelAbstract GetPanel(string controlName) {
			int panelIndx = vnToolkitPanels.FindIndex(p => p.ControlName == controlName);
			return vnToolkitPanels[panelIndx];
		}
	}
}