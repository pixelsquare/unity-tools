using UnityEngine;
using System.Collections;

/// <summary>
/// Custom Inspector and Editor Panel information
/// 
/// Author: Anthony Ganzon <anthony_0205@yahoo.com>
/// </summary>

namespace VNToolkit.VNUtility {

	public class VNPanelInfo {

		# region Editor Panels
		public const string PANEL_STARTUP_NAME = "Visual Novel Toolkit";
		public const string PANEL_SETUP_NAME = "Visial Novel Toolkit";

		public const string PANEL_NEW_PROJECT_NAME = "New Project";
		public const string PANEL_LOAD_PROJECT_NAME = "Load Project";
		public const string PANEL_PROJECT_SETTINGS_NAME = "Settings";
		public const string PANEL_RESOLUTION_NAME = "Resolution";

		public const string PANEL_DIALOGUE_WRITER_NAME = "Dialogue Writer";
		public const string PANEL_DIALOGUE_READER_NAME = "Dialogue Reader";
		public const string PANEL_DIALOGUE_INFO_NAME = "Dialogue Info";

		public const string PANEL_CHAPTER_NAME = "Chapter";

		# endregion Editor Panels

		# region Inspector Panels
		public const string CAMERA_SETTING_NAME = "VN Camera Settings";
		public const string DIALOGUE_NAME = "VN Dialogue";
		# endregion Inspector Panels
	}
}