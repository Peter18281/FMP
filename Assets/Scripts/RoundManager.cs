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
    public TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted){
            roundTimer -= Time.deltaTime;
            timerText.text = roundTimer.ToString("00");
        }
    }
}
