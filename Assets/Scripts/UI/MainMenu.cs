using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorial;
    public GameObject lobby;
    public GameObject joinLobby;
    public GameObject settings;

    public void ShowTutorial()
    {
        tutorial.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorial.SetActive(false);
    }

    public void ShowLobby()
    {
        lobby.SetActive(true);
    }

    public void HideLobby()
    {
        lobby.SetActive(false);
    }

    public void ShowJoinLobby()
    {
        joinLobby.SetActive(true);
    }

    public void HideJoinLobby()
    {
        joinLobby.SetActive(false);
    }

    public void ShowSettings()
    {
        settings.SetActive(true);
    }

    public void HideSettings()
    {
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
