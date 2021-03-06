﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour {

    public List<GameObject> uiElements;
    public List<GameObject> uiTargets;
    private List<Vector3> startPoints;

	public GameObject logo;
	public GameObject logoTarget;
    
    public float resistance = 15;
    public Boolean isPause = false;

    private float speed;

    private Vector3 pos;

    // Use this for initialization
    void Start () {
        startPoints = new List<Vector3>();
        foreach (GameObject thing in uiElements)
        {
            startPoints.Add(thing.transform.localPosition);
        }
	}
	
	// decelerate as approaching target
	void Update () {
        for (int i = 0; i < uiElements.Count; ++i)
        {
            speed = Vector3.Distance(uiElements[i].transform.localPosition, uiTargets[i].transform.localPosition)/resistance*20;
            uiElements[i].transform.localPosition = Vector3.MoveTowards(uiElements[i].transform.localPosition, uiTargets[i].transform.localPosition, speed*Time.deltaTime);
        }
		if (logo != null) {
			speed = Vector3.Distance (logo.transform.localPosition, logoTarget.transform.localPosition) / resistance * 20;
			logo.transform.localPosition = Vector3.MoveTowards (logo.transform.localPosition, logoTarget.transform.localPosition, speed * Time.deltaTime);
		}
    }


    void OnEnable()
    {
        try {
            for (int i = 0; i < uiElements.Count; ++i)
            {
                uiElements[i].transform.localPosition = startPoints[i];
            }
        } catch (Exception e)
        {
            // do nothing
        }
    }
}
