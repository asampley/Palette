using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour {

    public List<GameObject> uiElements;
    public List<Direction> uiDirections;

    private List<RectTransform> uiStartLocs;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < uiElements.Count; ++i)
        {
            Direction dir = uiDirections[i];
            switch (dir)
            {
                case Direction.N:
                    // MoveElementVertically(uiElements[i], 1);
                    break;
                case Direction.E:
                    // MoveElementHorizontally(uiElements[i], 1);
                    break;
                case Direction.S:
                    // MoveElementVertically(uiElements[i], -1);
                    break;
                case Direction.W:
                    // MoveElementHorizontally(uiElements[i], -1);
                    break;
                default:
                    break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MoveElementVertically(GameObject el, int dir)
    {

    }

    void MoveElementHorizontally(GameObject el, int dir)
    {

    }
}

public enum Direction
{
    N, E, S, W
}
