using UnityEngine;
using System.Collections;
using JsonFx.Json;
using System.IO;

namespace VNToolkit {

	[SerializeField]
	[JsonName("VNChapterData")]
	public class VNToolkitChapterData {

		// Public Variables

		// Private Variables

		// Static Variables
		public static string BASE_PATH = Application.dataPath + "/VN Toolkit/Streaming Assets/Chapter Data/";
		//public const string STREAM_PATH = "/ChapterData";
		public const string FILE_EXT = ".json";
		public const string FILE_FORMAT = "Chapter_{0}";

		public int chapterId;

		public string chapterName;

		public string chapterDesc;

		public VNToolkitChapterData() {
			this.chapterId = 0;
			this.chapterName = string.Empty;
			this.chapterDesc = string.Empty;
		}

		public VNToolkitChapterData Clone() {
			VNToolkitChapterData clone = new VNToolkitChapterData();
			clone.chapterId = this.chapterId;
			clone.chapterName = this.chapterName;
			clone.chapterDesc = this.chapterDesc;
			return clone;
		}

		public static VNToolkitChapterData Load(string id) {
			string fileContent = File.ReadAllText(BASE_PATH + id + FILE_EXT);
			fileContent = fileContent.Trim();

			JsonReaderSettings jsonSettings = new JsonReaderSettings();
			jsonSettings.TypeHintName = "__Type";

			JsonReader jsonReader = new JsonReader(fileContent);
			VNToolkitChapterData chapterData = jsonReader.Deserialize<VNToolkitChapterData>();

			return chapterData;
		}

		public void Save() {
			JsonWriterSettings jsonSettings = new JsonWriterSettings();
			jsonSettings.TypeHintName = "__Type";
			jsonSettings.PrettyPrint = true;

			string tmpId = string.Format(FILE_FORMAT, chapterId);
			JsonWriter jsonWritter = new JsonWriter(BASE_PATH + tmpId + FILE_EXT, jsonSettings);
			jsonWritter.Write(this);
			jsonWritter.TextWriter.Flush();
			jsonWritter.TextWriter.Close();
		}

		public void Remove(VNToolkitPhysicalData physicalData) {
			File.Delete(BASE_PATH + physicalData.id + FILE_EXT);
		}

		public override bool Equals(object obj) {
			if (obj == null) return false;

			VNToolkitChapterData data = obj as VNToolkitChapterData;
			if (data == null) return false;

			return (data.chapterId == this.chapterId) &&
				   (data.chapterName == this.chapterName) &&
				   (data.chapterDesc == this.chapterDesc);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}
}