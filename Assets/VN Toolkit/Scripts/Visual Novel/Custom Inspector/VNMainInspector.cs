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

		public void OnEnable() {
			vnPanelInspector = (VNPanelMonoAbstract)target;
			vnPanelInspector.OnPanelEnable(Repaint);
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			vnPanelInspector.OnPanelDraw();

			if (GUI.changed && vnPanelInspector.isActiveAndEnabled) 
				EditorUtility.SetDirty(target);
		}
	}
}