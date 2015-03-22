using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VNToolkitEditorObject {

	// Public Variables

	// Private Variables
	private static GameObject headObject;
	private static GameObject pooledObject;
	private static GameObject backgroundPoolObject;
	private static GameObject avatarPoolObject;

	private static List<GameObject> unknownObjects;

	// Static Variables
	private const string VN_HEAD_NAME = "VN Toolkit";
	private const string VN_OBJECT_POOL_NAME = "VN Object Pool Data";
	private const string VN_BACKGROUND_POOL_NAME = "VN Background Pool";
	private const string VN_AVATAR_POOL_NAME = "VN Avatar Pool";

	public static void CreateLoadBase() {
		headObject = GameObject.Find(VN_HEAD_NAME);
		if (headObject == null) {
			headObject = new GameObject(VN_HEAD_NAME);
		}

		pooledObject = GameObject.Find(VN_OBJECT_POOL_NAME);
		if (pooledObject == null) {
			pooledObject = new GameObject(VN_OBJECT_POOL_NAME);
		}
		pooledObject.transform.parent = headObject.transform;

		backgroundPoolObject = GameObject.Find(VN_BACKGROUND_POOL_NAME);
		if (backgroundPoolObject == null) {
			backgroundPoolObject = new GameObject(VN_BACKGROUND_POOL_NAME);
		}
		backgroundPoolObject.transform.parent = pooledObject.transform;

		avatarPoolObject = GameObject.Find(VN_AVATAR_POOL_NAME);
		if (avatarPoolObject == null) {
			avatarPoolObject = new GameObject(VN_AVATAR_POOL_NAME);
		}
		avatarPoolObject.transform.parent = pooledObject.transform;

		CheckForUnknowns();
		BroadcastUnknowns();
	}

	private static void CheckForUnknowns() {
		unknownObjects = new List<GameObject>();
		foreach (Transform obj in headObject.transform) {
			if (obj.name.Equals(VN_OBJECT_POOL_NAME)) {
				continue;
			}

			unknownObjects.Add(obj.gameObject);
		}

		foreach (Transform obj in pooledObject.transform) {
			if (obj.name.Equals(VN_BACKGROUND_POOL_NAME) ||
				obj.name.Equals(VN_AVATAR_POOL_NAME)) {
				continue;
			}

			unknownObjects.Add(obj.gameObject);
		}
	}

	private static void BroadcastUnknowns() {
		if (unknownObjects != null && unknownObjects.Count > 0) {
			for (int i = 0; i < unknownObjects.Count; i++) {
				Debug.Log("VN Editor: Unknown objects. " + unknownObjects[i].name);
			}
		}
	}
}