using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : NetworkBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorial;
    public GameObject lobby;
    public GameObject joinLobby;
    public GameObject settings;
    public NetworkRoomManager networkManager;
    public GameObject player2;
    public TMP_Text player1Name;
    public TMP_Text player2Name;
    public Sprite notReadySprite;
    public Sprite readySprite;
    public Image player1Img;
    public Image player2Img;

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
        SceneManager.LoadScene("Room", LoadSceneMode.Single);
        lobby.SetActive(true);
        networkManager.StartHost();
    }

    public void SetIP(string ip)
    {
        networkManager.networkAddress = ip;
    }

    public void JoinGame()
    {
        networkManager.StartClient();
    }

    public void Stop()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            networkManager.StopHost();
        }
        if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
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

    // void checkPlayers()
    // {

    //     if (networkManager.roomSlots.Count >= 1)
    //     {
    //         player1Name.text = networkManager.roomSlots[0].GetComponent<PlayerInfo>().name;

    //         if (networkManager.roomSlots[0].GetComponent<NetworkRoomPlayerEXT>().isReady)
    //         {
    //             player1Img.sprite = readySprite;
    //         }
    //         else
    //         {
    //             player1Img.sprite = notReadySprite;
    //         }

    //         if (networkManager.roomSlots.Count == 2)
    //         {
    //             player2.SetActive(true);
    //             player2Name.text = networkManager.roomSlots[1].GetComponent<PlayerInfo>().name;

    //             if (networkManager.roomSlots[1].GetComponent<NetworkRoomPlayerEXT>().isReady)
    //             {
    //                 player2Img.sprite = readySprite;
    //             }
    //             else
    //             {
    //                 player2Img.sprite = notReadySprite;
    //             }
    //         }
    //     }
    // }

    // public void Ready()
    // {
    //     if (networkManager.roomSlots.Count >= 1)
    //     {
    //         networkManager.roomSlots[0].GetComponent<NetworkRoomPlayerEXT>().changeReady();
    //         if (networkManager.roomSlots.Count == 2)
    //         {
    //             networkManager.roomSlots[1].GetComponent<NetworkRoomPlayerEXT>().changeReady();

    //             if(networkManager.roomSlots[1].GetComponent<NetworkRoomPlayerEXT>().isReady && networkManager.roomSlots[0].GetComponent<NetworkRoomPlayerEXT>().isReady){

    //             }
    //         }
    //     }
    // }

    void Update()
    {
        // checkPlayers();

        if (SceneManager.GetActiveScene().name == "Room")
        {
            mainMenu.SetActive(true);
            joinLobby.SetActive(false);
            lobby.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "Main")
        {
            mainMenu.SetActive(false);
        }
    }
}
