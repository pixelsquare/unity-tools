using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using JsonFx.Json;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace VNToolkit.VNEditor.VNUtility {

	public abstract class VNDataAbstract<T> where T : VNIData, new() {

		// Public Variables
		public abstract int DATA_ID { get; set; }

		public abstract string DATA_TITLE { get; }

		public abstract string BASE_PATH { get; }

		public abstract string FILE_EXT { get; }

		public abstract string FILE_FORMAT { get; }

		public virtual T Clone() {
			T clone = new T();
			clone.DATA_ID = this.DATA_ID;
			return clone;
		}

		public T Load(int id = 0) {
			Debug.Log("Loading data!");
			string tmpId = string.Format(FILE_FORMAT, id);

			string fileContent = File.ReadAllText(BASE_PATH + tmpId + FILE_EXT);
			fileContent = fileContent.Trim();

			JsonReaderSettings jsonSettings = new JsonReaderSettings();
			jsonSettings.TypeHintName = "__Type";

			JsonReader jsonReader = new JsonReader(fileContent);
			T data = jsonReader.Deserialize<T>();

			return data;
		}

		public void Save() {
			Debug.Log("Saving data!");
			JsonWriterSettings jsonSettings = new JsonWriterSettings();
			jsonSettings.TypeHintName = "__Type";
			jsonSettings.PrettyPrint = true;

			string tmpId = string.Format(FILE_FORMAT, DATA_ID);
			JsonWriter jsonWritter = new JsonWriter(BASE_PATH + tmpId + FILE_EXT, jsonSettings);
			jsonWritter.Write(this);
			jsonWritter.TextWriter.Flush();
			jsonWritter.TextWriter.Close();
			AssetDatabase.Refresh();
		}
	}
}