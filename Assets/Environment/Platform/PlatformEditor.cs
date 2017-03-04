#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(PlatformGenerator))]
public class PlatformEditor : Editor {
	PlatformGenerator gen;

	int oldWidth;
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
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("initialColor"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("blueprint"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("bc2d"));

		serializedObject.ApplyModifiedProperties ();

		if (!EditorUtility.IsPersistent (gen)) { // DO NOT DO IT IF IT IS A PREFAB
			
			// get the width and generate the platform if it changes
			int width = serializedObject.FindProperty ("width").intValue;

			if (width != oldWidth) {
				oldWidth = width;

				Undo.RecordObject (gen.gameObject, "Regenerated platform"); // sets platform to dirty, so the scene must save the changes
				gen.Generate ();
			}

			// get the starting color of the platform and change it if it changes

		}

		PaletteColorID colorID = (PaletteColorID) (serializedObject.FindProperty ("initialColor").enumValueIndex);
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