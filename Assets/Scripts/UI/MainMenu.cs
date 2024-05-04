using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorial;
    
    public void ShowTutorial(){
        tutorial.SetActive(true);
    }

    public void HideTutorial(){
        tutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
