using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Handles all the drawing of custom inspectors.
/// 
/// Author: Anthony Ganzon <anthony_0205@yahoo.com>
/// </summary>

namespace VNToolkit.VNUtility.VNCustomInspector {

	[CustomEditor(typeof(VNPanelMonoAbstract), true)]
	public class VNMainInspector : Editor {

		// Private Variables
		private VNPanelMonoAbstract vnPanelInspector;
		private bool initialized;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if (!initialized) {
				vnPanelInspector = (VNPanelMonoAbstract)target;
				vnPanelInspector.OnPanelEnable(Repaint);
				initialized = true;
			}

			vnPanelInspector.OnPanelDraw();

			if (GUI.changed && vnPanelInspector.isActiveAndEnabled) 
				EditorUtility.SetDirty(target);
		}
	}
}