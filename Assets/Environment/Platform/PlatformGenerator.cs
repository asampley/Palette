using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {
	[SerializeField] private PlatformBlueprint blueprint;
	[SerializeField] private BoxCollider2D bc2d;
	public int width;

	/// <summary>
	/// You should only call this from the editor for now.
	/// </summary>
	public void GenerateEditor(PlatformGenerator target) {
		if (width < 2) return;

		// remove old bits
		foreach (PlatformBit oldBit in GetComponentsInChildren<PlatformBit>()) {
//			Debug.Log (oldBit);
			DestroyImmediate (oldBit.gameObject);
		}

		// left
		GameObject platformBit = makeBit (blueprint.left, 0);

		// middle
		for (int i = 1; i < width - 1; ++i) {
			platformBit = makeBit (blueprint.middle, i);
		}

		// right
		platformBit = makeBit (blueprint.right, width - 1);

		// adjust box collider
		bc2d.size = new Vector2 (width * blueprint.tileWidth, bc2d.size.y);

		target.GetComponent<Platform> ().UpdateColor (true);
	}

	private GameObject makeBit (Sprite sprite, int index) {
		GameObject platformBit = new GameObject ();

		platformBit.transform.SetParent (this.transform);
		platformBit.transform.localPosition = new Vector2 (-(width * blueprint.tileWidth - 1) / 2f + index * blueprint.tileWidth, 0);
		platformBit.transform.localRotation = new Quaternion ();
		platformBit.transform.localScale = Vector3.one;
		
		SpriteRenderer bitRenderer = platformBit.AddComponent<SpriteRenderer> ();
		platformBit.AddComponent<PlatformBit> ();
		bitRenderer.sprite = sprite;

		platformBit.name = "Tile " + index;

		platformBit.hideFlags |= HideFlags.NotEditable | HideFlags.HideInInspector;

		return platformBit;
	}

	public PlatformBit[] GetPlatformBits() {
		return GetComponentsInChildren<PlatformBit>();
	}
}
