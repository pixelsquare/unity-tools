using UnityEngine;
using System.Collections;

namespace VNToolkit {
	namespace VNEditor {
		namespace VNUtility {
			public class VNConstants {

				// Static Variables
				public const string MENU_TITLE = "VN Toolkit";
				public const string MENU_PATH = "Toolkit/";

				public const string PROJECT_PATH = "/VN Toolkit/Projects";
				public const string VERSION_PATH = "/VN Toolkit/Version Number.txt";
				public const string DATA_FILE_EXT = ".json";

				public const string DEFAULT_STYLE_BOX = "box";
				public const float EDITOR_LABEL_WIDTH = 100f;
				public const float EDITOR_BUTTON_WIDTH = 100f;

				public const float INSPECTOR_LABEL_WIDTH = 150f;

				// General
				public const string NEW_PROJECT_NAME = "New Visual Novel Project";
				public const string NEW_PROJECT_FORMAT = "New Visual Novel Project {0}";

				public const float WINDOW_START_WIDTH = 600f;
				public const float WINDOW_START_HEIGHT = 400f;

				public const float CAMERA_DEFAULT_WIDTH = 1024f;
				public const float CAMERA_DEFAULT_HEIGHT = 768f;

				public const int CAMERA_DEFAULT_PIXELS_PER_UNIT = 100;
				public const string CAMERA_LAYER_MASK = "VN_LAYER";

				// Panels
				public const string PANEL_START_NAME = "Visual Novel Toolkit";
				public const float PANEL_START_WIDTH = -1f;

				public const string PANEL_NEW_PROJECT_NAME = "New Project";
				public const float PANEL_NEW_PROJECT_WIDTH = -1f;

				public const string PANEL_LOAD_PROJECT_NAME = "Load Project";
				public const float PANEL_LOAD_PROJECT_WIDTH = -1f;

				public const string PANEL_PROJECT_SETTINGS_NAME = "Settings";
				public const float PANEL_PROJECT_SETTINGS_WIDTH = -1f;

				// Plain Panels
				public const string PANEL_RESOLUTION_NAME = "Resolution";
				public const float PANEL_RESOLUTION_WIDTH = -1f;
			}
		}
	}
}