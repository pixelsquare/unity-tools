using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace VNToolkit {
	public class VNToolkitEditorDataCache {

		// Public Variables

		// Private Variables

		// Static Variables

		public List<VNToolkitChapterData> chapterData;

		private const string chapterDataStr = "chapterDataStr";

		public void Initialize() {
			chapterData = new List<VNToolkitChapterData>();
		}

		public void Save() {
			for (int i = 0; i < chapterData.Count; i++) {
				chapterData[i].Save();
			}
		}

		public void Load() {
			chapterData = new List<VNToolkitChapterData>();
			string[] fileDirectory = Directory.GetFiles(VNToolkitChapterData.BASE_PATH, "*.json", SearchOption.AllDirectories);
			for (int i = 0; i < fileDirectory.Length; i++) {
				fileDirectory[i] = Path.GetFileNameWithoutExtension(fileDirectory[i]);
			}

			foreach (string file in fileDirectory) {
				chapterData.Add(VNToolkitChapterData.Load(file));
			}
		}
	}
}