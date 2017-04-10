using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPrint : MonoBehaviour {
    ArrayList msgContainer;

    void Start()
    {
        msgContainer = new ArrayList();
        // Does the player really need to know all this?
        //Application.logMessageReceivedThreaded += NotifyPlayer;
    }

    void limitEntries()
    {
        // limit all message entries to 2.
        if (msgContainer.Count > 2)
            msgContainer.RemoveAt(0);
    }

    // Parameters taken from Unity Docs https://docs.unity3d.com/ScriptReference/Application.LogCallback.html
    public void NotifyPlayer(string logString, string stackTrace, LogType type)
    {
        msgContainer.Add(logString);
        limitEntries();
        StartCoroutine(removeEntry());
    }

    private void OnGUI()
    {
        Text errMsg = this.GetComponent<Text>();
        errMsg.text = "";
        foreach(string msg in msgContainer)
            errMsg.text = errMsg.text + msg + "\n\n";
    }

    public void addMsg(string msg)
    {
        msgContainer.Add(msg);
        limitEntries();
        StartCoroutine(removeEntry());
    }

    public IEnumerator removeEntry()
    {
        yield return new WaitForSeconds(3f);
        if(msgContainer.Count > 0)
            msgContainer.RemoveAt(0);
    }
}
