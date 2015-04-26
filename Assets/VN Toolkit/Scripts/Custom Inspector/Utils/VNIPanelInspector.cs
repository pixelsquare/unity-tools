using UnityEngine;
using System.Collections;
using UnityEditor.AnimatedValues;

/// <summary>
/// Baseline for Panel Inspector Abstract class.
/// 
/// Author: Anthony Ganzon <anthony_0205@yahoo.com>
/// </summary>

namespace VNToolkit {
	namespace VNUtility {
		namespace VNCustomInspector {
			public interface VNIPanelInspector {

				// When panel is shown
				bool PanelActive { get; }

				// When panel is currently in use or opened
				bool PanelEnabled { get; }

				// When panel is foldable
				bool IsPanelFoldable { get; }

				// Enable revert button
				bool IsRevertButtonEnabled { get; }

				string PanelTitle { get; }

				System.Action OnInspectorGUI { get; }

				void PanelOpen();
				void PanelClose();

				void SetPanelActive(bool active);
			}
		}
	}
}