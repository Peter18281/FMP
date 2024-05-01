using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class RoundManager : NetworkBehaviour
{
    private int roundsToWin = 2;
    private int gamesToWin = 2;
    public float roundTimer = 99.0f;
    public bool gameStarted = true;
    public int player1Rounds;
    public int player2Rounds;
    public TMP_Text timerText;
    private GameObject[] players;
    [SerializeField]
    private GameObject player1Spawn;
    [SerializeField]
    private GameObject player2Spawn;

    public void AwardWin(bool player1)
    {
        if (player1)
        {
            player1Rounds++;
        }
        else
        {
            player2Rounds++;
        }
    }

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void Reset(){
        players[0].transform.position = player1Spawn.transform.position;
        players[1].transform.position = player2Spawn.transform.position;
        players[0].GetComponent<Player>().health = 100;
        players[1].GetComponent<Player>().health = 100;
        players[0].GetComponent<Player>().lost = false;
        players[1].GetComponent<Player>().lost = false;

        roundTimer = 99.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player1Rounds);

        if (players.Length < 2)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        if (gameStarted)
        {
            roundTimer -= Time.deltaTime;
            timerText.text = roundTimer.ToString("00");
        }

        if (roundTimer <= 0)
        {
            if (players[0].GetComponent<Player>().health == players[1].GetComponent<Player>().health)
            {
                player1Rounds++;
                player2Rounds++;
            }
            else if (players[0].GetComponent<Player>().health > players[1].GetComponent<Player>().health)
            {
                if (players[0].GetComponent<Player>().player1)
                {
                    player1Rounds++;
                }
                else
                {
                    player2Rounds++;
                }
            }
            else if (players[0].GetComponent<Player>().health < players[1].GetComponent<Player>().health)
            {
                if (players[0].GetComponent<Player>().player1)
                {
                    player1Rounds++;
                }
                else
                {
                    player2Rounds++;
                }
            }
            Reset();
        }
    }
}
