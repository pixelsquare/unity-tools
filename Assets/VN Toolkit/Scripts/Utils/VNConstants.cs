using UnityEngine;
using System.Collections;

namespace VNToolkit.VNEditor.VNUtility {

	public class VNConstants {

		// Static Variables
		public const string MENU_TITLE = "VN Toolkit";
		public const string MENU_PATH = "Toolkit/";

		public const string ASSETS_PATH = "Assets/";
		public const string MAIN_PATH = "VN Toolkit/";
		public const string PROJECT_PATH = MAIN_PATH + "Projects";
		public const string VERSION_PATH = MAIN_PATH + "Version Number.txt";
		public const string ICON_PATH = MAIN_PATH + "Icon/";

		public const string DEFAULT_STYLE_BOX = "box";
		public const float EDITOR_LABEL_WIDTH = 100f;
		public const float EDITOR_BUTTON_WIDTH = 100f;

		public const float INSPECTOR_LABEL_WIDTH = 150f;
		public const float FOOTER_HEIGHT = 20f;

		// General
		public const string NEW_PROJECT_NAME = "New Visual Novel Project";
		public const string NEW_PROJECT_FORMAT = "New Visual Novel Project {0}";

		public const float EDITOR_WINDOW_START_WIDTH = 600f;
		public const float EDITOR_WINDOW_START_HEIGHT = 300f;

		public const float EDITOR_WINDOW_DEFAULT_WIDTH = 1024;
		public const float EDITOR_WINDOW_DEFAULT_HEIGHT = 768f;

		public const float CAMERA_DEFAULT_WIDTH = 1024f;
		public const float CAMERA_DEFAULT_HEIGHT = 768f;

		public const int CAMERA_DEFAULT_PIXELS_PER_UNIT = 100;
		public const string CAMERA_LAYER_MASK = "VN_LAYER";
	}
}