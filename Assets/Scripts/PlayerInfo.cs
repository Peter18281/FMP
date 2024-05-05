using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar] public string name;
    [SyncVar] public int id;

    void Start()
    {
            name = PlayerPrefs.GetString("PlayerName");
    }
}
