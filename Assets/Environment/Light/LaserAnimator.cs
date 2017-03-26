using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(Renderer))]
public class LaserAnimator : MonoBehaviour {
	private MaterialPropertyBlock mpb;

	public int allTheVariablesThatMatterAreStatic;

	private static bool dirtySprite = true;
	private static int currentSprite = 0;
	public static int numSprites = 8;

	public const float animFPS = 30;
	private static float timeSinceLastSpriteUpdate = 0;

	private static Mesh _sharedMesh;
	private static Mesh sharedMesh {
		get {
			if (_sharedMesh == null) {
				_sharedMesh = new Mesh ();

				Vector3[] vertices = new Vector3[4];
				int[] triangles = new int[3 * 2 * vertices.Length];

				vertices [0] = new Vector2 (0, -0.5f);
				vertices [1] = new Vector2 (meshLength, -0.5f);
				vertices [2] = new Vector2 (0, 0.5f);
				vertices [3] = new Vector2 (meshLength, 0.5f);

				triangles [0] = 2;
				triangles [1] = 1;
				triangles [2] = 0;
				triangles [3] = 1;
				triangles [4] = 2;
				triangles [5] = 3;

				_sharedMesh.vertices = vertices;
				_sharedMesh.triangles = triangles;
			}
			return _sharedMesh;
		}
	}
	private static float meshLength = 1000;

	void Awake() {
		mpb = new MaterialPropertyBlock ();
	}

	void Start() {
		GetComponent<MeshFilter> ().sharedMesh = sharedMesh;
	}

	void Update() {
		float now = Time.time;

		if (now - timeSinceLastSpriteUpdate > 1 / animFPS) {
			currentSprite = (currentSprite + 1) % numSprites;
			dirtySprite = true;
			timeSinceLastSpriteUpdate = now;
		}

		if (dirtySprite) {
			Mesh mesh = GetComponent<MeshFilter> ().sharedMesh;
			Debug.Assert (GetComponent<MeshFilter> ().sharedMesh.vertexCount == 4);

			Vector2[] uvs = new Vector2[4];
		
			// Assumes vertical is to switch animation
			uvs [0] = new Vector2 (0, (float)currentSprite / numSprites);
			uvs [1] = new Vector2 (1, (float)currentSprite / numSprites);
			uvs [2] = new Vector2 (0, (float)(currentSprite + 1) / numSprites);
			uvs [3] = new Vector2 (1, (float)(currentSprite + 1) / numSprites);

			mesh.uv = uvs;

			dirtySprite = false;

//			Debug.Log ("Switched all lasers to sprite " + currentSprite);
			for (int a = 0; a < uvs.Length; ++a) {
//				Debug.Log (uvs [a]);
				Debug.Log (mesh.vertices [a]);
			}
		}
	}

	public void SetLength(float length) {
		mpb.SetFloat ("_MaxX", length);
//		Debug.Log (length / (2 * GetComponent<Renderer>().bounds.size.x));
		Debug.Log (mpb.GetFloat("_MaxX"));
		GetComponent<Renderer> ().SetPropertyBlock (mpb);
	}

	public void SetVisible(bool visible) {
		GetComponent<Renderer> ().enabled = visible;
	}
	
	public void SetColor(PaletteColor color) {
		SetColor (color.ToColor ());
	}

	public void SetColor(Color color) {
		mpb.SetColor ("_Color", color);
		GetComponent<Renderer> ().SetPropertyBlock (mpb);
	}
}
