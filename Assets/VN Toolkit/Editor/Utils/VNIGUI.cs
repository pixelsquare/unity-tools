using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

namespace VNToolkit {
	namespace VNEditor {
		namespace VNUtility {
			public interface VNIGUI {

				// Public Variables
				VNIGUI parent { get; set; }
				List<VNIGUI> children { get; set; }

				// When panel is shown
				bool PanelActive { get; }

				// When panel is currently in use or opened
				bool PanelEnabled { get; }

				// When panel is foldable
				bool IsPanelFoldable { get; }

				string PanelTitle { get; }

				// NOTE: Panel Width set to negative value will make the panel width flexible
				float PanelWidth { get; }

				string PanelControlName { get; }

				System.Action<Rect> WindowGUI { get; }

				void Initialize(UnityAction repaint);
				void PanelOpen();
				void PanelClose();
				void PanelSave();
				void PanelLoad();

				void PanelClear();
				void PanelReset();

				void SetPanelActive(bool active);

				void AddChildren(VNIGUI child);
				List<VNIGUI> GetChildren();
				VNIGUI GetChild(string title);
			}
		}
	}
}