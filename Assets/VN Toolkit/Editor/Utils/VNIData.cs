using UnityEngine;
using System.Collections;
using System;

namespace VNToolkit {
	namespace VNEditor {
		namespace VNUtility {
			public interface VNIData {

				// Public Variables
				int DATA_ID { get; set; }

				string DATA_TITLE { get; }

				string BASE_PATH { get; }

				string FILE_EXT { get; }

				string FILE_FORMAT { get; }
			}
		}
	}
}