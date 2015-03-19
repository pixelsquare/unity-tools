using UnityEngine;
using UnityEditor;
using System.IO;

public class VNToolkitEditor : EditorWindow {

	// Public Variables

	// Private Variables
	private string versionTitle;

	// Static Variables
	private const float WINDOW_MIN_Y = 400f;
	private const float WINDOW_MIN_X = 500f;

	[MenuItem("Toolkit/VN Toolkit")]
	private static void Initialize() {
		string versionTitle = File.ReadAllText(Application.dataPath + "/VN Toolkit/Version Number.txt");

		VNToolkitEditor window = (VNToolkitEditor)EditorWindow.GetWindow(typeof(VNToolkitEditor));
		window.title = versionTitle;
		window.minSize = new Vector2(WINDOW_MIN_X, WINDOW_MIN_Y);
		window.Show();
	}

	private void OnGUI() {
		GUILayout.Label("VN Toolkit", EditorStyles.boldLabel);
	}
}