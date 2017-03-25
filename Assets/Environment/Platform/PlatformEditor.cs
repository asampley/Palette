#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(PlatformGenerator))]
public class PlatformEditor : Editor {
	PlatformGenerator gen;

	int oldWidth;
	int oldHeight;
	PlatformBlueprint oldBP;
	PaletteColorID oldColorID;

	void OnEnable() {
		gen = ((PlatformGenerator)target);
	}

	/// <summary>
	/// Custom code for viewing the editor script
	/// </summary>
	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		// basic fields go here
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("width"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("height"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("blueprint"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("bc2d"));

		serializedObject.ApplyModifiedProperties ();

		// get the width and generate the platform if it changes
		int width = serializedObject.FindProperty ("width").intValue;
		int height = serializedObject.FindProperty ("height").intValue;
		// get the blueprint and generate the platform if it changes
		PlatformBlueprint bp = (PlatformBlueprint)serializedObject.FindProperty ("blueprint").objectReferenceValue;

		if (width != oldWidth || height != oldHeight || bp != oldBP) {
			oldWidth = width;
			oldHeight = height;
			oldBP = bp;

			Debug.Log ("Regenerated platform");

			Undo.RecordObject (gen.gameObject, "Regenerated platform"); // sets platform to dirty, so the scene must save the changes
			gen.GetComponent<MeshRenderer> ().material = bp.material;
			if (!EditorUtility.IsPersistent (gen)) {
				gen.GenerateEditor (gen);
			}
		}

		PaletteColorID colorID = gen.GetComponent<ColorAdder>().GetBaseColorID();
		if (colorID != oldColorID) {
			Debug.Log (colorID);
			Undo.RecordObject (gen.gameObject.GetComponent<ColorAdder>(), "Recolored platform");

			gen.GetComponent<ColorAdder> ().SetBaseColorID(colorID);

			oldColorID = colorID;
		}


//		EditorGUILayout.PropertyField(serializedObject.FindProperty("networkManager"));
//		EditorGUILayout.PropertyField(serializedObject.FindProperty("mode"));
//		if ((NetworkConnector.Mode)(serializedObject.FindProperty ("mode").enumValueIndex) == NetworkConnector.Mode.CLIENT) {
//			EditorGUILayout.PropertyField (serializedObject.FindProperty ("ipSource"));
//		}
//		EditorGUILayout.PropertyField(serializedObject.FindProperty("onSuccess"));
//		EditorGUILayout.PropertyField(serializedObject.FindProperty("onFailure"));
		serializedObject.ApplyModifiedProperties ();
	}
}

#endif