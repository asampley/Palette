using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour {

    public List<GameObject> uiElements;
    public List<GameObject> uiTargets;
    public float resistance = 15;
    private float speed;

    private Vector3 pos;

    // Use this for initialization
    void Start () {

	}
	
	// decelerate as approaching target
	void Update () {
        //float step = Mathf.Max(Mathf.Sin(Time.time), Mathf.Cos(3.14f/2)) ;
        //accel = Mathf.Sin(Time.time)*quickness;


        for (int i = 0; i < uiElements.Count; ++i)
        {
            speed = Vector3.Distance(uiElements[i].transform.position, uiTargets[i].transform.position)/resistance;
            uiElements[i].transform.position = Vector3.MoveTowards(uiElements[i].transform.position, uiTargets[i].transform.position, speed);
        }
        

    }
    
}
