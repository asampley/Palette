using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour {

    public List<GameObject> uiElements;
    public List<GameObject> uiTargets;

    private List<Vector3> starts = new List<Vector3>();


	// Use this for initialization
	void Start () {
        foreach (GameObject thing in uiElements)
        {
            starts.Add(thing.transform.position);
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < uiElements.Count; ++i)
        {
            uiElements[i].transform.position = starts[i] + new Vector3(Mathf.Sin(Time.time), 0.0f, 0.0f);
        }
    }

}
