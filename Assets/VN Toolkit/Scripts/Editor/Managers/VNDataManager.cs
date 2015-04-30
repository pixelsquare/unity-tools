using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;
using System;

namespace VNToolkit.VNEditor {

	public class VNDataManager {

		// Private Variables
		private VNIData CurrentData { get; set; }
		private VNIData CurrentLoadedData { get; set; }

		private VNProjectData vnProjectData;
		private VNProjectData vnLoadedProjectData;

		public VNProjectData VnProjectData {
			get {
				if (vnProjectData == null)
					vnProjectData = new VNProjectData();

				return vnProjectData; 
			}

			set { vnProjectData = value; }
		}

		private VNChapterData vnChapterData;
		private VNChapterData vnLoadedChapterData;

		public VNChapterData VnChapterData {
			get {
				if (vnChapterData == null)
					vnChapterData = new VNChapterData();

				return vnChapterData;
			}

			set { vnChapterData = value; }
		}

		// Static Variables
		private static VNDataManager instance;
		public static VNDataManager SharedInstance {
			get {
				if (instance == null)
					instance = new VNDataManager();

				return instance;
			}
		}

		public void SaveData() {
			if (VNPanelManager.VnEditorState == VN_EditorState.STARTUP) {
				VnProjectData.Save();
			}
			else if (VNPanelManager.VnEditorState == VN_EditorState.SETUP) {
				VnChapterData.Save();
			}
		}

		public void LoadData() {
			if (VNPanelManager.VnEditorState == VN_EditorState.STARTUP) {
				VnProjectData = VnProjectData.Load() as VNProjectData;

				vnLoadedProjectData = new VNProjectData();
				vnLoadedProjectData = VnProjectData.Clone() as VNProjectData;
				SetCurrentData(VnProjectData, vnLoadedProjectData);
			}
			else if (VNPanelManager.VnEditorState == VN_EditorState.SETUP) {
				VnChapterData = VnChapterData.Load() as VNChapterData;

				vnLoadedChapterData = new VNChapterData();
				vnLoadedChapterData = VnChapterData.Clone() as VNChapterData;
				SetCurrentData(VnChapterData, vnLoadedChapterData);
			}
		}

		public void SetCurrentData(VNIData data, VNIData loadedData) {
			if (CurrentData == data && CurrentLoadedData == loadedData)
				return;

			CurrentData = data;
			CurrentLoadedData = loadedData;
		}

		public static bool CompareLoadedData<T>() {
			T oldData = (T)VNDataManager.SharedInstance.CurrentLoadedData;
			T newData = (T)VNDataManager.SharedInstance.CurrentData;
			return VNEditorUtility.Compare<T>(newData, oldData);
		}
	}
}