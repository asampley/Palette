using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPrint : MonoBehaviour {
    string txt;
    ArrayList a;

    void Start()
    {
        a = new ArrayList();
        Application.logMessageReceivedThreaded += NotifyPlayer;
    }
    // Parameters taken from Unity Docs https://docs.unity3d.com/ScriptReference/Application.LogCallback.html
    public void NotifyPlayer(string logString, string stackTrace, LogType type)
    {
            txt = logString;
            a.Add(logString);
            StartCoroutine(removeEntry());
    }
    private void OnGUI()
    {
        Text errMsg = this.GetComponent<Text>();
        errMsg.text = "";
        foreach(string s in a)
            errMsg.text = errMsg.text + s + "\n";
    }
    public IEnumerator removeEntry()
    {
        yield return new WaitForSeconds(5f);
        a.RemoveAt(0);
    }
}
