using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEditor;

namespace VNToolkit {
	public class VNToolkitNotifierEditor {

		// Public Variables

		// Private Variables
		private static bool initialize = false;
		private static string notificationMessage;
		private static double notificationTimeout;

		// Static Variables

		private static void Initialize() {
			if (!initialize) {
				notificationMessage = string.Empty;
				notificationTimeout = EditorApplication.timeSinceStartup;
				initialize = true;
			}
		}

		public static void NotifierWindow(UnityAction repaint, Rect position) {
			Initialize();

			GUI.Box(new Rect(100f, position.height - 20f, 150f, 20f), string.Empty, EditorStyles.textField);
			//Debug.Log(EditorApplication.timeSinceStartup + " " + notificationTimeout);
			if (EditorApplication.timeSinceStartup < notificationTimeout) {
				EditorGUI.DropShadowLabel(new Rect(100f, position.height - 15f, 150f, 10f), notificationMessage);
			}
			repaint();
		}

		public static void TriggerNotification(string message, double sec = 1) {
			notificationMessage = message;
			notificationTimeout = EditorApplication.timeSinceStartup + sec;
		}
	}
}