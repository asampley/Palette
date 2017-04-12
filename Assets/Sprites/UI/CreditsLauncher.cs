using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsLauncher : MonoBehaviour {

	public void Launch()
    {
        SceneManager.LoadScene("Credits");
    }
}
