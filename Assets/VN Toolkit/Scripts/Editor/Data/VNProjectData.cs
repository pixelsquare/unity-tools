using UnityEngine;
using System.Collections;
using JsonFx.Json;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	[SerializeField]
	[JsonName("VNProjectData")]
	public class VNProjectData : VNDataAbstract<VNProjectData>, VNIData {

		// Public Variables

		// Private Variables
		public string projectName;

		public int projectWidth;

		public int projectHeight;

		public int projectPixelsPerUnit;

		// Static Variables

		public override int DATA_ID { get; set; }

		public override string DATA_TITLE {
			get { return VNDataName.PROJECT_SETTINGS_NAME; }
		}

		public override string BASE_PATH {
			get { return VNFileManager.STREAM_PATH; }
		}

		public override string FILE_EXT {
			get { return VNFileManager.DATA_FILE_EXT; }
		}

		public override string FILE_FORMAT {
			get { return VNDataName.PROJECT_SETTING_FORMAT; }
		}

		public override VNProjectData Clone() {
			VNProjectData clone = base.Clone();
			clone.projectName = this.projectName;
			clone.projectWidth = this.projectWidth;
			clone.projectHeight = this.projectHeight;
			clone.projectPixelsPerUnit = this.projectPixelsPerUnit;
			return clone;
		}
	}
}