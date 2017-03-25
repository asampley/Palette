using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter))]
public class PlatformGenerator : MonoBehaviour {
	[SerializeField] private PlatformBlueprint blueprint;
	[SerializeField] private BoxCollider2D bc2d;
	public int width = 1;
	public int height = 1;

	private Mesh _mesh;
	private Mesh mesh { 
		get {
			if (_mesh == null) {
				mesh = new Mesh ();
			}
			return _mesh;
		}
		set {
				_mesh = value;
		}
	}

	/// <summary>
	/// You should only call this from the editor for now.
	/// </summary>
	public void GenerateEditor(PlatformGenerator target) {
		if (width < 1 || height < 1) return;

		float startTime = Time.realtimeSinceStartup;

		// remove old bits
		foreach (PlatformBit oldBit in GetComponentsInChildren<PlatformBit>()) {
//			Debug.Log (oldBit);
			DestroyImmediate (oldBit.gameObject);
		}
		mesh.Clear ();

		// Generate mesh
		Vector3[] vertices = new Vector3[4 * width * height];
		Vector2[] uvs = new Vector2[vertices.Length];
		int[] triangles = new int[3 * 2 * vertices.Length];
		for (int j = 0; j < height; ++j) {
			for (int i = 0; i < width; ++i) {
				int k = (j * width + i); // general index
				vertices [4 * k]     = new Vector2 (i, -j);
				vertices [4 * k + 1] = new Vector2 (i + 1, -j);
				vertices [4 * k + 2] = new Vector2 (i, -(j + 1));
				vertices [4 * k + 3] = new Vector2 (i + 1, -(j + 1));

				float uStart;
				float uEnd;
				float vStart;
				float vEnd;

				if (i == 0) {
					uStart = 0;
					uEnd = 1f / 3;
				} else if (i == width - 1 && width > 1) {
					uStart = 2f / 3;
					uEnd = 1;
				} else {
					uStart = 1f / 3;
					uEnd = 2f / 3;
				}

				if (j == 0) {
					vStart = 1;
					vEnd = 2f / 3;
				} else if (j == height - 1 && height > 1) {
					vStart = 1f / 3;
					vEnd = 0;
				} else {
					vStart = 2f / 3;
					vEnd = 1f / 3;
				}

				uvs      [4 * k]     = new Vector2 (uStart, vStart);
				uvs      [4 * k + 1] = new Vector2 (uEnd,   vStart);
				uvs      [4 * k + 2] = new Vector2 (uStart, vEnd);
				uvs      [4 * k + 3] = new Vector2 (uEnd,   vEnd);

				triangles [3 * 2 * k]     = 4 * k + 0;
				triangles [3 * 2 * k + 1] = 4 * k + 1;
				triangles [3 * 2 * k + 2] = 4 * k + 2;
				triangles [3 * 2 * k + 3] = 4 * k + 3;
				triangles [3 * 2 * k + 4] = 4 * k + 2;
				triangles [3 * 2 * k + 5] = 4 * k + 1;

//				for (int a = 0; a < 4; ++a) {
//					Debug.Log (vertices [4 * k + a]);
//					Debug.Log (uvs [4 * k + a]);
//				}
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;

		// apply mesh
		GetComponent<MeshFilter> ().mesh = mesh;

		// adjust box collider
		bc2d.size = new Vector2 (width * blueprint.tileWidth, height * blueprint.tileWidth);
		bc2d.offset = new Vector2 ((width * blueprint.tileWidth) / 2f, -(height * blueprint.tileWidth) / 2f);

		// change color
		target.GetComponent<Platform> ().UpdateColor (true);

		Debug.Log ("Generated platform in " + (Time.realtimeSinceStartup - startTime) + "s");
	}

	void OnDestroy() {
		Debug.Log ("Destruction");
	}
}
