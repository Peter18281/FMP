using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorial;
    public GameObject lobby;
    public GameObject joinLobby;
    public GameObject settings;
    public NetworkManager networkManager;

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
        networkManager.StartHost();
    }

    public void SetIP(string ip){
        networkManager.networkAddress = ip;
    }

    public void JoinGame(){
        networkManager.StartClient();
    }

    public void Stop(){
        if(networkManager.mode == NetworkManagerMode.Host){
            networkManager.StopHost();
        }
        if(networkManager.mode == NetworkManagerMode.ClientOnly){
            networkManager.StopClient();
        }
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

    void Update()
    {

    }
}
