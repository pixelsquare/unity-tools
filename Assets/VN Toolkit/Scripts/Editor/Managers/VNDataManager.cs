using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNEditor {

	public class VNDataManager {

		// Static Variables
		private static VNProjectData vnLoadedProjectData;
		private static VNProjectData vnProjectData;
		public static VNProjectData VnProjectData {
			get {
				if (vnProjectData == null)
					vnProjectData = new VNProjectData();

				return vnProjectData;
			}

			set { vnProjectData = value; }
		}

		public static void SaveData() {
			VnProjectData.Save();
		}

		public static void LoadData() {
			VnProjectData = VnProjectData.Load() as VNProjectData;

			vnLoadedProjectData = new VNProjectData();
			vnLoadedProjectData = VnProjectData.Clone() as VNProjectData;
		}

		public static bool CompareLoadedData() {
			return VNEditorUtility.Compare<VNProjectData>(vnProjectData, vnLoadedProjectData);
		}
	}
}