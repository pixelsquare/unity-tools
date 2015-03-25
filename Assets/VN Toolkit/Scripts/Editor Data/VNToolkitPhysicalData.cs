using UnityEngine;
using System.Collections;
using JsonFx.Json;
using System.IO;

namespace VNToolkit {

	[SerializeField]
	[JsonName("VNPhysicalData")]
	public class VNToolkitPhysicalData {

		// Public Variables

		// Private Variables

		// Static Variables
		public static string BASE_PATH = Application.dataPath + "/VN Toolkit/Streaming Assets/Physical Data/";
		public const string STREAM_PATH = "/PhysicalData";
		public const string FILE_EXT = ".json";
		public const string FILE_FORMAT = "PhysicalData_{0}";

		public int id;

		public VNToolkitPhysicalData() {
			this.id = 0;
		}

		public VNToolkitPhysicalData Clone() {
			VNToolkitPhysicalData clone = new VNToolkitPhysicalData();
			clone.id = this.id;
			return clone;
		}

		public VNToolkitPhysicalData Load() {
			string fileContent = File.ReadAllText(STREAM_PATH + FILE_EXT);
			fileContent = fileContent.Trim();

			JsonReaderSettings jsonSettings = new JsonReaderSettings();
			jsonSettings.TypeHintName = "__Type";

			JsonReader jsonReader = new JsonReader(fileContent);
			VNToolkitPhysicalData phyiscalData = jsonReader.Deserialize<VNToolkitPhysicalData>();

			return phyiscalData;
		}

		public void Save() {
			JsonWriterSettings jsonSettings = new JsonWriterSettings();
			jsonSettings.TypeHintName = "__Type";
			jsonSettings.PrettyPrint = true;

			string tmpId = string.Format(FILE_FORMAT, id);
			JsonWriter jsonWritter = new JsonWriter(BASE_PATH + tmpId + FILE_EXT, jsonSettings);
			jsonWritter.Write(this);
			jsonWritter.TextWriter.Flush();
			jsonWritter.TextWriter.Close();
		}

		public void Remove(VNToolkitPhysicalData physicalData) {
			File.Delete(BASE_PATH + physicalData.id + FILE_EXT);
		}
	}
}