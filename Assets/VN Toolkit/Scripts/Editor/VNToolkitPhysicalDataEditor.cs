using UnityEngine;
using System.Collections;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using UnityEditor;

namespace VNToolkit {
	public class VNToolkitPhysicalDataEditor  {

		// Public Variables

		// Private Variables
		private static bool initialize;
		private static bool foldPhysicalWindow;
		private static AnimBool physicalDataWindowAnim;

		// Static Variables

		private static void Initialize(UnityAction repaint) {
			if (!initialize) {
				foldPhysicalWindow = true;
				physicalDataWindowAnim = new AnimBool(true);
				physicalDataWindowAnim.valueChanged.AddListener(repaint);
				initialize = true;
			}
		}

		public static void PhysicalDataWindow(UnityAction repaint) {
			Initialize(repaint);

			EditorGUILayout.BeginVertical("box");

			if (GUILayout.Button("Physical Data", EditorStyles.boldLabel)) {
				foldPhysicalWindow = !foldPhysicalWindow;
			}

			physicalDataWindowAnim.target = foldPhysicalWindow;
			if (EditorGUILayout.BeginFadeGroup(physicalDataWindowAnim.faded)) {

				
				EditorGUILayout.LabelField("HELLO");
				EditorGUILayout.LabelField("HELLO");
				EditorGUILayout.LabelField("HELLO");
				EditorGUILayout.LabelField("HELLO");
				EditorGUILayout.LabelField("HELLO");
				EditorGUILayout.LabelField("HELLO");
			}

			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.EndVertical();
		}
	}
}