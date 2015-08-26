using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Dmanager))]
public class DmanagerInspector : Editor {


	public override void OnInspectorGUI() {
		//base.OnInspectorGUI ();
		DrawDefaultInspector ();
				

		if (GUILayout.Button ("Regen")) {
			Dmanager dmanager = (Dmanager)target;
			dmanager.BuildMesh();

		}
	}
}