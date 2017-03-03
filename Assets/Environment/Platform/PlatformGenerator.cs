using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {
	[SerializeField] private PlatformBlueprint blueprint;
	[SerializeField] private BoxCollider2D bc2d;
	public int width;
	[SerializeField] private List<GameObject> platformBits = new List<GameObject>();
	[SerializeField] private PaletteColorID initialColor;

	public void Generate() {
		if (width < 2) return;

		GameObject platformBit;
		SpriteRenderer bitRenderer;

		// remove old bits
		foreach (GameObject oldBit in platformBits) {
			DestroyImmediate (oldBit);
		}
		platformBits.Clear ();

		// left
		platformBit = makeBit (blueprint.left, 0);
		platformBits.Add(platformBit);

		// middle
		for (int i = 1; i < width - 1; ++i) {
			platformBit = makeBit (blueprint.middle, i);
			platformBits.Add (platformBit);
		}

		// right
		platformBit = makeBit (blueprint.right, width - 1);
		platformBits.Add(platformBit);

		// adjust box collider
		bc2d.size = new Vector2 (width * blueprint.tileWidth, bc2d.size.y);
	}

	private GameObject makeBit (Sprite sprite, int index) {
		GameObject platformBit = new GameObject ();

		platformBit.transform.SetParent (this.transform);
		platformBit.transform.localPosition = new Vector2 (-(width * blueprint.tileWidth - 1) / 2f + index * blueprint.tileWidth, 0);

		
		SpriteRenderer bitRenderer = platformBit.AddComponent<SpriteRenderer> ();
		bitRenderer.sprite = sprite;

		platformBit.name = "Tile " + index;

		return platformBit;
	}

	public List<GameObject> GetPlatformBits() {
		return platformBits;
	}
}
