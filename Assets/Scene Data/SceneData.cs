using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneData {
	public const string NAME = "__Scene Data__";
	public static GameObject gameObject {
		get {
			return GameObject.Find (NAME);
		}
	}
}
