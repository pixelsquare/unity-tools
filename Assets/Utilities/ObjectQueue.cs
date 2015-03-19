using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectQueue {

	// Public Variables

	// Private Variables
	private List<object> objList;

	// Static Variables

	public ObjectQueue() {
		objList = new List<object>();
	}

	public void Enqueue(object obj) {
		objList.Add(obj);
	}

	public object Dequeue() {
		if (objList.Count > 0) {
			object obj = objList[0];
			objList.RemoveAt(0);
			return obj;
		}

		return null;
	}

	public void Clear() {
		objList.Clear();
	}

	public int Count() {
		return objList.Count;
	}
}