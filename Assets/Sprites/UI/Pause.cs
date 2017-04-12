using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public GameObject pauseScreen;
    public Sprite toggleMusicOn;
    public Sprite toggleMusicOff;
    public Sprite toggleColorWheelOn;
    public Sprite toggleColorWheelOff;
    private Player player;
    private bool inGameColorWheel;

    private void Start()
    {
        inGameColorWheel = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
            Debug.Log("Pressed Pause");
        }
    }

    public void TogglePauseMenu()
    {
        player = SceneData.sceneObject.GetComponent<LocalPlayer>().localPlayer;
        if (pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(false);
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<LaserController>().enabled = true;
            GameObject.Find("ColorWheelInGame").GetComponent<Image>().enabled = inGameColorWheel;
        }
        else
        {
            pauseScreen.SetActive(true);
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<LaserController>().enabled = false;
            GameObject.Find("ColorWheelInGame").GetComponent<Image>().enabled = false;

            // Show toggle buttons either on or off correctly
            ToggleButtonMusic();

            // Show toggle buttons for colorwheel correctly
            ToggleButtonColorWheel();
        }
    }

    public void GotoMainScreen()
    {
        // go to the main screen
        Application.LoadLevel("Main Menu");
        // need to disconnect.

    }

    public void QuitToDesktop()
    {
        // quit application
        Application.Quit();
    }

    public void ToggleMusic()
    {
        // toggle the music
        Music music = SceneData.sceneObject.GetComponent<Music>();
        if (music.musicOn)
        {
            music.musicOn = false;
            music.backtrack.GetComponentInChildren<AudioSource>().mute = true;
        }
        else
        {
            music.musicOn = true;
            music.backtrack.GetComponentInChildren<AudioSource>().mute = false;
        }
        ToggleButtonMusic();
    }

    public void ToggleColorwheelInGame()
    {
        inGameColorWheel = !inGameColorWheel;
        ToggleButtonColorWheel();
    }

    private void ToggleButtonColorWheel()
    {
        Image mToggleColorImg = GameObject.Find("BtnColorImage").GetComponentInChildren<Image>();
        if (inGameColorWheel)
            mToggleColorImg.sprite = toggleColorWheelOn;
        else
            mToggleColorImg.sprite = toggleColorWheelOff;
    }

    private void ToggleButtonMusic()
    {
        Music mToggle = SceneData.sceneObject.GetComponent<Music>();
        Image mToggleImg = GameObject.Find("BtnMusicImage").GetComponentInChildren<Image>();
        if (mToggle.musicOn)
            mToggleImg.sprite = toggleMusicOn;
        else
            mToggleImg.sprite = toggleMusicOff;
    }

}
