using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

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

        TestGetIP();
	}
	
	// decelerate as approaching target
	void Update () {
        for (int i = 0; i < uiElements.Count; ++i)
        {
            speed = Vector3.Distance(uiElements[i].transform.position, uiTargets[i].transform.position)/resistance;
            uiElements[i].transform.position = Vector3.MoveTowards(uiElements[i].transform.position, uiTargets[i].transform.position, speed);
        }
    }

    public void ResetAnimation()
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

    public void TestGetIP()
    {
        string strHostName = Dns.GetHostName();
        IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
        IPAddress[] addr = ipEntry.AddressList;
        Debug.Log(addr[addr.Length - 1].ToString());
    }
}
