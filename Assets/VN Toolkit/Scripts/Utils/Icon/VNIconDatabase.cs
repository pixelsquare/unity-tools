using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEditor;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNUtility.VNIcon {

	public class VNIconDatabase {

		// Public Variables

		// Private Variables
		private Dictionary<string, Texture2D> icons;

		private const string FILE_EXT = ".png";

		// Static Variables 

		private static VNIconDatabase instance;
		public static VNIconDatabase SharedInstance {
			get {
				if (instance == null) {
					instance = new VNIconDatabase();
					instance.LoadFiles();
				}

				return instance;
			}
		}

		private void LoadFiles() {
			icons = new Dictionary<string, Texture2D>();

			Type type = typeof(VNIconName);
			VNIconName iconNames = new VNIconName();

			foreach(FieldInfo field in type.GetFields()) {
				string fieldValue = type.GetField(field.Name).GetValue(iconNames).ToString();

				StringBuilder stringPath = new StringBuilder();
				stringPath.Append(VNConstants.ASSETS_PATH);
				stringPath.Append(VNConstants.ICON_PATH);
				stringPath.Append(fieldValue);
				stringPath.Append(FILE_EXT);

				Texture2D iconTexture = AssetDatabase.LoadAssetAtPath(stringPath.ToString(), typeof(Texture2D)) as Texture2D;
				//Debug.Log(fieldValue + " " + iconTexture);
				AddIcon(fieldValue, iconTexture);
			}
		}

		public Texture2D GetIcon(string iconID) {
			if (icons == null)
				return null;

			if (!icons.ContainsKey(iconID)) {
				Debug.LogWarning("ICON DATABASE! " + iconID + " not found!");
				return null;
			}

			return icons[iconID];
		}

		public void AddIcon(string iconID, Texture2D icon) {
			if (icons == null)
				return;

			if (icons.ContainsKey(iconID)) {
				Debug.LogWarning("ICON DATABASE! " + iconID + " already in database!");
				return;
			}

			icons.Add(iconID, icon);
		}
	}
}