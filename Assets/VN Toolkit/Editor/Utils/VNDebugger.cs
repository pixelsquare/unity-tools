using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

namespace VNToolkit {
	namespace VNEditor {
		namespace VNUtility {
			public class VNDebugger {

				// Private Variables
				private static bool logEnabled = false;
				private static Dictionary<string, List<string>> logs;

				private const string LOG_FORMAT = "[{0}]: {1}";
				private const string EXPORT_FORMAT = "[{0} | {1}]: {2}";

				private static void InitializeDebugger() {
					if (logs != null)
						return;

					logs = new Dictionary<string, List<string>>();
				}

				public static void Log(string id, string log) {
					InitializeDebugger();

					if (!logs.ContainsKey(id)) {
						logs.Add(id, new List<string>());
					}

					List<string> stringList = new List<string>();
					stringList.AddRange(logs[id]);

					string exportLog = string.Format(EXPORT_FORMAT, DateTime.Now, id, log);
					stringList.Add(exportLog);

					if (!logEnabled) return;

					string textLog = string.Format(LOG_FORMAT, id, log);
					Debug.Log(textLog);
				}

				public static void CleaerLogs() {
					logs.Clear();
				}

				public static void ExportToTextFile() {
					File.CreateText(Application.dataPath + "/Debug.txt");
					AssetDatabase.Refresh();
				}
			}

		}
	}
}