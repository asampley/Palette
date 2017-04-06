#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(NetworkConnector))]
public class NetworkConnectorEditor : Editor {
	/// <summary>
	/// Custom code for viewing the editor script
	/// </summary>
	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("networkManager"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("mode"));
		if ((NetworkConnector.Mode)(serializedObject.FindProperty ("mode").enumValueIndex) == NetworkConnector.Mode.CLIENT) {
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("ipSource"));
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("onSuccess"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("onFailure"));
		serializedObject.ApplyModifiedProperties ();
	}
}

#endif