using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SpawnInfo {
	public Transform spawn;
	public PaletteColorID colorID;
	public RuntimeAnimatorController controller;
}
