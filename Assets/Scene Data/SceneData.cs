using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneData {
	public const string TAG = "Scene Data";
	public static GameObject gameObject {
		get {
			return GameObject.FindWithTag (TAG);
		}
	}
}
