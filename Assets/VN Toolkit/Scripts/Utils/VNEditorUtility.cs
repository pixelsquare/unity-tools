using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace VNToolkit.VNEditor.VNUtility {

	public class VNEditorUtility {

		// Static Variables
		public static bool Compare<T>(T obj1, T obj2) {
			Type type = typeof(T);

			if (obj1 == null || obj2 == null)
				return false;

			//foreach (MemberInfo member in type.GetMembers()) {
			//    string obj1Val = string.Empty;
			//    string obj2Val = string.Empty;

				// Reflection on class fields
				//if (member.MemberType == MemberTypes.Field) {
					foreach (FieldInfo field in type.GetFields()) {
						string obj1Val = string.Empty;
						string obj2Val = string.Empty;

						if (type.GetField(field.Name).GetValue(obj1) != null) {
							obj1Val = type.GetField(field.Name).GetValue(obj1).ToString();
						}

						if (type.GetField(field.Name).GetValue(obj2) != null) {
							obj2Val = type.GetField(field.Name).GetValue(obj2).ToString();
						}

						if (obj1Val.Trim() != obj2Val.Trim()) {
							return false;
						}
					}
				//}

				// Reflection on class properties
				//if (member.MemberType == MemberTypes.Property) {
					foreach (PropertyInfo property in type.GetProperties()) {
						string obj1Val = string.Empty;
						string obj2Val = string.Empty;

						if (type.GetProperty(property.Name).GetValue(obj1, null) != null) {
							obj1Val = type.GetProperty(property.Name).GetValue(obj1, null).ToString();
						}

						if (type.GetProperty(property.Name).GetValue(obj2, null) != null) {
							obj2Val = type.GetProperty(property.Name).GetValue(obj2, null).ToString();
						}

						if (obj1Val.Trim() != obj2Val.Trim()) {
							return false;
						}
					}
			//	}
			//}

			return true;
		}

		public static void SetAllPanelStateRecursively(VNIPanel root, VN_PANELSTATE state) {
			root.SetPanelState(state);
			foreach (VNIPanel child in root.children) {
				if (child != null) {
					child.SetPanelState(state);
					SetAllPanelStateRecursively(child, state);
				}
			}
		}

		public static void ClearConsoleLog() {
			Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
			Type type = assembly.GetType("UnityEditorInternal.LogEntries");
			MethodInfo method = type.GetMethod("Clear");
			method.Invoke(new object(), null);
		}
	}
}