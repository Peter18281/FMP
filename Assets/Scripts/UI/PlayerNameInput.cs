using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private Button enterButton;
    public string displayName;
    private string PlayerPrefsNameKey = "PlayerName";
    [SerializeField]
    private GameObject nameField;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            nameField.SetActive(true);
            return;
        }
    }

    public void SetPlayerName(string name)
    {
        displayName = nameInputField.text;

        enterButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        PlayerPrefs.SetString(PlayerPrefsNameKey, displayName);

        nameField.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerName(displayName);
    }
}
