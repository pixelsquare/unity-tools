using UnityEngine;
using System.Collections;

/// <summary>
/// Custom Inspector and Editor Panel information
/// 
/// Author: Anthony Ganzon <anthony_0205@yahoo.com>
/// </summary>

namespace VNToolkit {
	namespace VNUtility {
		public class VNPanelInfo {

			# region Editor Panels
			// Panels
			public const string PANEL_START_NAME = "Visual Novel Toolkit";
			public const string PANEL_NEW_PROJECT_NAME = "New Project";
			public const string PANEL_LOAD_PROJECT_NAME = "Load Project";
			public const string PANEL_PROJECT_SETTINGS_NAME = "Settings";

			// Plain Panels
			public const string PANEL_RESOLUTION_NAME = "Resolution";
			# endregion Editor Panels

			# region Inspector Panels
			public const string CAMERA_SETTING_NAME = "VN Camera Settings";
			public const string DIALOGUE_NAME = "VN Dialogue";
			# endregion Inspector Panels
		}
	}
}
