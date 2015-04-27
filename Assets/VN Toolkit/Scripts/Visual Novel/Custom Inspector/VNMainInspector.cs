using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Handles all the drawing of custom inspectors.
/// 
/// Author: Anthony Ganzon <anthony_0205@yahoo.com>
/// </summary>

namespace VNToolkit {
	namespace VNUtility {
		namespace VNCustomInspector {
			[CustomEditor(typeof(VNPanelInspectorAbstract), true)]
			public class VNMainInspector : Editor {

				// Private Variables
				private VNPanelInspectorAbstract vnPanelInspector;

				public void OnEnable() {
					vnPanelInspector = (VNPanelInspectorAbstract)target;
					vnPanelInspector.OnPanelEnable(Repaint);
				}

				public override void OnInspectorGUI() {
					base.OnInspectorGUI();
					vnPanelInspector.OnPanelDraw();

					if(GUI.changed && vnPanelInspector.isActiveAndEnabled)	EditorUtility.SetDirty(target);
				}
			}
		}
	}
}