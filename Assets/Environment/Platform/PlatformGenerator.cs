using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {
	[SerializeField] private PlatformBlueprint blueprint;
	[SerializeField] private BoxCollider2D bc2d;
	public int width = 1;
	public int height = 1;

	/// <summary>
	/// You should only call this from the editor for now.
	/// </summary>
	public void GenerateEditor(PlatformGenerator target) {
		if (width < 1 || height < 1) return;

		// remove old bits
		foreach (PlatformBit oldBit in GetComponentsInChildren<PlatformBit>()) {
//			Debug.Log (oldBit);
			DestroyImmediate (oldBit.gameObject);
		}

		for (int j = 0; j < height; ++j) {
			Sprite ls;
			Sprite ms;
			Sprite rs;

			if (j == 0) {
				ls = blueprint.topLeft;
				ms = blueprint.topMiddle;
				rs = blueprint.topRight;
			} else if (j == height - 1) {
				ls = blueprint.bottomLeft;
				ms = blueprint.bottomMiddle;
				rs = blueprint.bottomRight;
			} else {
				ls = blueprint.left;
				ms = blueprint.middle;
				rs = blueprint.right;
			}

			// left
			GameObject platformBit = makeBit (ls, 0, j);

			// middle
			for (int i = 1; i < width - 1; ++i) {
				platformBit = makeBit (ms, i, j);
			}

			if (width >= 2) {
				// right
				platformBit = makeBit (rs, width - 1, j);
			}
		}

		// adjust box collider
		bc2d.size = new Vector2 (width * blueprint.tileWidth, bc2d.size.y);

		target.GetComponent<Platform> ().UpdateColor (true);
	}

	private GameObject makeBit (Sprite sprite, int x, int y) {
		GameObject platformBit = new GameObject ();

		platformBit.transform.SetParent (this.transform);
		platformBit.transform.localPosition = new Vector2 (-(width * blueprint.tileWidth - 1) / 2f + x * blueprint.tileWidth, -(height * blueprint.tileWidth - 1) / 2f + y * blueprint.tileWidth);
		if (blueprint.rot90) {
			platformBit.transform.localRotation = Quaternion.Euler (0, 0, 90);
		} else {
			platformBit.transform.localRotation = new Quaternion ();
		}
		platformBit.transform.localScale = Vector3.one;
		
		SpriteRenderer bitRenderer = platformBit.AddComponent<SpriteRenderer> ();
		platformBit.AddComponent<PlatformBit> ();
		bitRenderer.sprite = sprite;

		platformBit.name = "Tile (" + x + "," + y + ")";

		platformBit.hideFlags |= HideFlags.NotEditable | HideFlags.HideInInspector;

		return platformBit;
	}

	public PlatformBit[] GetPlatformBits() {
		return GetComponentsInChildren<PlatformBit>();
	}
}
