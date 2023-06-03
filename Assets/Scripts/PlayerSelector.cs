using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject backwardButton;
    public static PlayerSelector instance;
    public string playerPrefabName;
    public GameObject[] playerModel;
    public int selectedCharacter;

    private void Awake()
    {
        instance = this;        
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("SelectedCharacter"))
        {
            selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
            playerPrefabName = playerModel[selectedCharacter].GetComponent<PlayerModelName>().playerName;
            nextButton.SetActive(false);
            backwardButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
            backwardButton.SetActive(true);
        }

        foreach(GameObject player in playerModel)
        {
            player.SetActive(false);
        }

        playerModel[selectedCharacter].SetActive(true);
    }

    public void ChangeNext()
    {
        playerModel[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if (selectedCharacter == playerModel.Length)
            selectedCharacter = 0;
        playerModel[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        playerPrefabName = playerModel[selectedCharacter].GetComponent<PlayerModelName>().playerName;

    }
    public void ChangeBack()
    {
        playerModel[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter == -1)
            selectedCharacter = playerModel.Length -1;
        playerModel[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        playerPrefabName = playerModel[selectedCharacter].GetComponent<PlayerModelName>().playerName;

    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey("SelectedCharacter");
    }
}
