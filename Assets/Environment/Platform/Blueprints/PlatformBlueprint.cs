using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlueprint : MonoBehaviour {
	public Material material;

	/*
	 * Specify some extra margins to extend mesh outside of collider. May
	 * cause some stretching of sprites, but is useful for glow effects
	 * outside of the standard sprite shape.
	 */
	public float horizontalMargin = 0;
	public float verticalMargin = 0;
}
