using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

namespace VNToolkit {
	public abstract class VNToolkitPanelAbstract {

		// Public Variables

		// Private Variables
		private Color originalColor;

		// Static Variables

		public abstract string ControlName { get; }

		public abstract bool PanelActive { get; }

		public abstract bool OnWindowChanged { get; }

		public abstract System.Action WindowGUI { get; }

		public abstract void Initialize(UnityAction repaint, Rect windowPos);

		public abstract void RepaintPanel(bool forcedSave = false);

		public abstract void SavePanel();

		public void DrawGUI() {
			if (!PanelActive)
				return;

			originalColor = GUI.color;
			GUI.color = (OnWindowChanged) ? Color.red : Color.grey;
			GUI.SetNextControlName(ControlName);
			EditorGUILayout.BeginVertical("box");
			GUI.color = originalColor;

			if (WindowGUI != null) WindowGUI();

			EditorGUILayout.EndVertical();
		}
	}
}