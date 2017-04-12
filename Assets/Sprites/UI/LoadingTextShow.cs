using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTextShow : MonoBehaviour {

	public void ShowText(string name)
    {
        GameObject.Find(name).GetComponent<Text>().enabled = true;
        GameObject.Find("Main Camera").GetComponent<GUITexture>().texture = null;
    }
}
