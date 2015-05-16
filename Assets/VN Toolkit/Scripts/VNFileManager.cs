using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace VNToolkit.VNEditor.VNUtility {

	public class VNFileManager {

		// Public Variables

		// Private Variables

		// Static Variables
		public static string BASE_PATH = string.Empty;
		public static string STREAM_PATH = string.Empty;
		public static string CHAPTER_PATH = string.Empty;
		public static string SCENE_PATH = string.Empty;

		public const string DATA_FILE_EXT = ".json";

		public static void CreateProjectPath(string basePath) {
			BASE_PATH = basePath;
			if (!Directory.Exists(BASE_PATH)) {
				Directory.CreateDirectory(BASE_PATH);

				string[] words = BASE_PATH.Split('/');
				Debug.Log("[FILE] " + words[words.Length - 1] + " created!");
			}

			STREAM_PATH = BASE_PATH + "/StreamingAssets/";
			if (!Directory.Exists(STREAM_PATH)) {
				Directory.CreateDirectory(STREAM_PATH);
				Debug.Log("[FILE] StreamingAssets created!");
			}

			CHAPTER_PATH = STREAM_PATH + "/Chapter Data/";
			if (!Directory.Exists(CHAPTER_PATH)) {
				Directory.CreateDirectory(CHAPTER_PATH);
				Debug.Log("[FILE] Chapter Data created!");
			}

			SCENE_PATH = STREAM_PATH + "/Scene Data/";
			if (!Directory.Exists(SCENE_PATH)) {
				Directory.CreateDirectory(SCENE_PATH);
				Debug.Log("[FILE] Scene Data created!");
			}

			AssetDatabase.Refresh(ImportAssetOptions.Default);
		}

		public static void LoadProjectPath(string basePath) {
			BASE_PATH = basePath;
			if (!Directory.Exists(BASE_PATH)) {
				Debug.LogWarning(BASE_PATH + " doesn't exist!");
				BASE_PATH = string.Empty;
			}

			STREAM_PATH = BASE_PATH + "/StreamingAssets/";
			if (!Directory.Exists(STREAM_PATH)) {
				Debug.LogWarning(STREAM_PATH + " doesn't exist!");
				STREAM_PATH = string.Empty;
			}

			CHAPTER_PATH = STREAM_PATH + "Chapter Data/";
			if (!Directory.Exists(CHAPTER_PATH)) {
				Debug.LogWarning(CHAPTER_PATH + " doesn't exist!");
				CHAPTER_PATH = string.Empty;
			}

			SCENE_PATH = STREAM_PATH + "Scene Data/";
			if (!Directory.Exists(SCENE_PATH)) {
				Debug.LogWarning(SCENE_PATH + " doesn't exist!");
				SCENE_PATH = string.Empty;
			}
		}

		public static bool IsPathValid() {
			return (Directory.Exists(BASE_PATH) && Directory.Exists(STREAM_PATH));
		}

		public static string[] LoadDataFromPath(string path) {
			if (!IsPathValid())
				return null;

			List<string> resultFileNames = new List<string>();
			string[] fileWords = new string[0];

			string[] files = Directory.GetFiles(path, "*" + DATA_FILE_EXT, SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++) {
				fileWords = files[i].Split('/');
				resultFileNames.Add(fileWords[fileWords.Length - 1]);
			}

			return resultFileNames.ToArray();
		}
	}
}