using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;

public class MenuAnimator : MonoBehaviour {

    public List<GameObject> uiElements;
    public List<GameObject> uiTargets;
    private List<Vector3> startPoints;

    public float resistance = 15;
    private float speed;

    private Vector3 pos;

    // Use this for initialization
    void Start () {
        startPoints = new List<Vector3>();
        foreach (GameObject thing in uiElements)
        {
            startPoints.Add(thing.transform.position);
        }

		Debug.Log (GetIP ());
	}
	
	// decelerate as approaching target
	void Update () {
        for (int i = 0; i < uiElements.Count; ++i)
        {
            speed = Vector3.Distance(uiElements[i].transform.position, uiTargets[i].transform.position)/resistance;
            uiElements[i].transform.position = Vector3.MoveTowards(uiElements[i].transform.position, uiTargets[i].transform.position, speed);
        }
    }

    void OnEnable()
    {
        try {
            for (int i = 0; i < uiElements.Count; ++i)
            {
                uiElements[i].transform.position = startPoints[i];
            }
        } catch (Exception e)
        {
            // do nothing
        }
    }

	// try and update location of where to start because the camera is always moving. Always. Moving.
	// Sampley, how do make bettr?
	void OnDisable() {
		try {
			startPoints.Clear();
			for (int i = 0; i < uiElements.Count; ++i)
			{
				startPoints.Add(uiElements[i].transform.position);
			}
		} catch (Exception e)
		{
			// do nothing
		}
	}

	// Want to have IP shown on pause menu. Probably move this code elsewhere.
    public string GetIP()
    {
		return Network.player.ipAddress;
    }
}
