using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace VNToolkit {
	namespace VNEditor {
		namespace VNUtility {
			public class VNEditorUtility {

				// Static Variables
				public static bool Compare<T>(T obj1, T obj2) {
					Type type = typeof(T);

					if (obj1 == null || obj2 == null)
						return false;

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

					return true;
				}
			}
		}
	}
}