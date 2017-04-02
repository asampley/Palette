using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour {

    public List<GameObject> uiElements;
    public List<GameObject> uiTargets;
    public float speed;

    private Vector3 pos;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;

        for (int i = 0; i < uiElements.Count; ++i)
        {
            uiElements[i].transform.position = Vector3.MoveTowards(uiElements[i].transform.position, uiTargets[i].transform.position, step);
        }
        
    }

}
