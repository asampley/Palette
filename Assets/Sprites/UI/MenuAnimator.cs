using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour {

    public List<GameObject> uiElements;
    public List<Directions> uiDirections;

    private List<RectTransform> uiStartLocs;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < uiElements.Count; ++i)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum Directions
{
    N, E, S, W
}
