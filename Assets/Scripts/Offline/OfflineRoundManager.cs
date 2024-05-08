using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfflineRoundManager : MonoBehaviour
{
    private int roundsToWin = 2;
    private int gamesToWin = 2;
    public float roundTimer = 99.0f;
    private float roundEndTimer = 2.0f;
    public bool gameStarted = false;
    public int player1Rounds;
    public int player2Rounds;
    public int player1Games;
    public int player2Games;
    public TMP_Text timerText;
    public TMP_Text player1RoundWin;
    public TMP_Text player1GameWin;
    public TMP_Text player2RoundWin;
    public TMP_Text player2GameWin;
    public TMP_Text round1Start;
    public TMP_Text round2Start;
    public TMP_Text round3Start;
    public TMP_Text fight;
    public TMP_Text draw;
    [SerializeField]
    private GameObject player1Spawn;
    [SerializeField]
    private GameObject player2Spawn;
    public GameObject player1;
    public GameObject player2;
    public OfflinePlayer player1Script;
    public OfflinePlayer player2Script;
    public bool roundEnded;
    public bool roundStarted = false;
    [SerializeField]
    private GameObject postGame;
    [SerializeField]
    private TMP_Text setCount;
    [SerializeField]
    private Button rematch;

    public void AwardWin(bool player1)
    {
        if (player1)
        {
            player1Rounds++;
            if (player1Rounds != 2)
            {
                player1RoundWin.gameObject.SetActive(true);
            }
            else
            {
                EndGame();
            }
            roundEnded = true;
        }
        else
        {
            player2Rounds++;
            if (player2Rounds != 2)
            {
                player2RoundWin.gameObject.SetActive(true);
            }
            else
            {
                EndGame();
            }
            roundEnded = true;
        }
    }

    void Start()
    {
        gameStarted = false;
        roundStarted = false;
        roundEnded = false;
        Invoke("RoundStart", 1);
        Invoke("ClearRoundText", 2);
        Invoke("FightText", 3);
        Invoke("ClearFightText", 4);
        gameStarted = true;
    }

    void PostGame()
    {
        setCount.text = player1Games + " - " + player2Games;
        postGame.SetActive(true);
        player1GameWin.gameObject.SetActive(false);
        player2GameWin.gameObject.SetActive(false);
        if (player1Games == gamesToWin || player2Games == gamesToWin)
        {
            rematch.gameObject.SetActive(false);
        }
    }

    public void Rematch()
    {
        player1Rounds = 0;
        player2Rounds = 0;
        postGame.SetActive(false);
        Reset();
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void EndGame()
    {
        if (player1Rounds == roundsToWin && player2Rounds == roundsToWin)
        {
            player1Script.anim.SetTrigger("Died");
            player2Script.anim.SetTrigger("Died");
            player1Games++;
            player2Games++;
            draw.gameObject.SetActive(true);
            Invoke("PostGame", 2);
        }
        else if (player1Rounds == roundsToWin)
        {
            player1GameWin.gameObject.SetActive(true);
            player1Games++;
            Invoke("PostGame", 2);
        }
        else if (player2Rounds == roundsToWin)
        {
            player2GameWin.gameObject.SetActive(true);
            player2Games++;
            Invoke("PostGame", 2);
        }
    }

    private void ClearFightText()
    {
        fight.gameObject.SetActive(false);
        roundStarted = true;
    }

    private void FightText()
    {
        fight.gameObject.SetActive(true);
        Invoke("ClearFightText", 1);
    }

    private void ClearRoundText()
    {
        round1Start.gameObject.SetActive(false);
        round2Start.gameObject.SetActive(false);
        round3Start.gameObject.SetActive(false);
        Invoke("FightText", 1);
    }

    private void RoundStart()
    {
        if (player1Rounds + player2Rounds == 0)
        {
            round1Start.gameObject.SetActive(true);
            Invoke("ClearRoundText", 1);
        }
        else if (player1Rounds + player2Rounds == 1)
        {
            round2Start.gameObject.SetActive(true);
            Invoke("ClearRoundText", 1);
        }
        else if (player1Rounds + player2Rounds == 2 && (player1Rounds != 2 && player2Rounds != 2))
        {
            round3Start.gameObject.SetActive(true);
            Invoke("ClearRoundText", 1);
        }
    }

    public void Reset()
    {
        player1.transform.position = player1Spawn.transform.position;
        player2.transform.position = player2Spawn.transform.position;
        player1Script.health = 100;
        player2Script.health = 100;
        player1Script.lost = false;
        player2Script.lost = false;
        player1Script.anim.SetTrigger("roundStart");
        player2Script.anim.SetTrigger("roundStart");
        roundEnded = false;

        player1RoundWin.gameObject.SetActive(false);
        player2RoundWin.gameObject.SetActive(false);

        roundTimer = 99.0f;

        Invoke("RoundStart", 1);
    }

    void HealthCheck()
    {
        if (player1Script.health == player2Script.health)
        {
            player1Rounds++;
            player2Rounds++;
        }
        else if (player1Script.health > player2Script.health)
        {
            if (player1Script.player1)
            {
                player1Rounds++;
            }
            else
            {
                player2Rounds++;
            }
        }
        else if (player1Script.health < player2Script.health)
        {
            if (player1Script.player1)
            {
                player1Rounds++;
            }
            else
            {
                player2Rounds++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Script.health <= 0 && !roundEnded)
        {
            roundEnded = true;
            roundStarted = false;
            AwardWin(false);
        }
        else if (player2Script.health <= 0 && !roundEnded)
        {
            roundEnded = true;
            roundStarted = false;
            AwardWin(true);
        }

        if (roundEnded && !roundStarted)
        {
            Invoke("Reset", 2);
        }

        if (gameStarted && !roundEnded && roundStarted)
        {
            roundTimer -= Time.deltaTime;
            timerText.text = roundTimer.ToString("00");
        }

        if (roundTimer <= 0)
        {
            HealthCheck();
            Reset();
        }
    }
}
