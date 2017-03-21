using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralaxing : MonoBehaviour {

    public Transform[] backgrounds;
    private float[] parallaxScales; //proportion of cameras movement to move backrounds by
    public float smoothing=1f;    //how smooth parallax is going to be

    private Transform cam;      //reference to main cameras transfofrm
    private Vector3 previousCamPos;     //position of camera in previous frame

    //called before start, assigns reference to camera
    void Awake()
    {
        //sets it to main camera
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start () {
        //previous fram had current frames camera pos
        previousCamPos = cam.position;

        //assign scales
        parallaxScales = new float[backgrounds.Length];
        for (int i=0;i<backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
        for (int i = 0; i< backgrounds.Length; i++)
        {
            //parallax is opposite of camera movement b/c previous frame *scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            //set target x pos which is current pos plus parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create targget position which is background curent position with its target x pos
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between curr pos and target pos
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //set prev camera pos to camera pos at end of frame
        previousCamPos = cam.position;
	}
}
