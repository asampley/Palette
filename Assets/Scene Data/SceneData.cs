using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneData {
	private const string TAG = "Scene Data";
	public static GameObject sceneObject {
		get {
			return GameObject.FindWithTag (TAG);
		}
	}
}
