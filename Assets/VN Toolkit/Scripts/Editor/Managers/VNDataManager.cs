using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

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

		private List<VNChapterData> vnChapterData;
		private List<VNChapterData> vnLoadedChapterData;

		public List<VNChapterData> VnChapterData {
			get {
				if (vnChapterData == null)
					vnChapterData = new List<VNChapterData>();

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
				VnChapterData.ForEach(a => a.Save());
			}
		}

		public void LoadData() {
			if (!VNFileManager.IsPathValid())
				return;

			if (VNPanelManager.VnEditorState == VN_EditorState.STARTUP) {
				VnProjectData = VnProjectData.Load() as VNProjectData;

				vnLoadedProjectData = new VNProjectData();
				vnLoadedProjectData = VnProjectData.Clone() as VNProjectData;
				SetCurrentData(VnProjectData, vnLoadedProjectData);
			}
			else if (VNPanelManager.VnEditorState == VN_EditorState.SETUP) {
				VnChapterData = new List<VNChapterData>();

				string[] files = VNFileManager.LoadDataFromPath(VNFileManager.CHAPTER_PATH);
				if (files != null) {
					for (int i = 0; i < files.Length; i++) {
						MatchCollection mCollection = Regex.Matches(files[i], @"\d");
						StringBuilder sb = new StringBuilder();
						for (int j = 0; j < mCollection.Count; j++) {
							sb.Append(mCollection[j].ToString());
						}

						int fileID = int.Parse(sb.ToString());
						VNChapterData tmpData = new VNChapterData();
						tmpData = tmpData.Load(fileID);
						VnChapterData.Add(tmpData);
					}
				}
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

		public static VNIData GetData<T>(VNIData data) {
			Type t = typeof(T);

			if (t == typeof(VNProjectData)) {
				return SharedInstance.VnProjectData;
			}
			else if (t == typeof(VNChapterData)) {
				return SharedInstance.VnChapterData.Find(a => a.Equals(data));
			}

			return null;
		}
	}
}