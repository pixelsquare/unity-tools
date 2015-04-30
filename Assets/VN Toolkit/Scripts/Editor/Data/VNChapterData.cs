using UnityEngine;
using System.Collections;
using JsonFx.Json;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	[SerializeField]
	[JsonName("VNChapterData")]
	public class VNChapterData : VNDataAbstract<VNChapterData>, VNIData {

		// Public Variables

		// Private Variables
		public string chapterName;

		public string chapterDesc;

		// Static Variables 

		public override int DATA_ID { get; set; }

		public override string DATA_TITLE {
			get { return VNDataName.PROJECT_SETTINGS_NAME; }
		}

		public override string BASE_PATH {
			get { return VNFileManager.CHAPTER_PATH; }
		}

		public override string FILE_EXT {
			get { return VNFileManager.DATA_FILE_EXT; }
		}

		public override string FILE_FORMAT {
			get { return VNDataName.PROJECT_CHAPTER_FORMAT; }
		}

		public override VNChapterData Clone() {
			VNChapterData clone = base.Clone();
			return clone;
		}
	}
}