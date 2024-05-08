using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    public bool active = true;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if(!active){
            gameObject.SetActive(false);
        }

    }
}
