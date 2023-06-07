using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    private int gold;
    public GameObject nextButton;
    public GameObject backwardButton;
    public GameObject selectButton;
    public GameObject mainScreen;
    public GameObject changeButton;
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
        if (PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            gold = 0;
        }

        if (PlayerPrefs.HasKey("SelectedCharacter"))
        {
            selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
            playerPrefabName = playerModel[selectedCharacter].GetComponent<PlayerModelName>().playerName;
            nextButton.SetActive(false);
            backwardButton.SetActive(false);
            selectButton.SetActive(false);
            mainScreen.SetActive(true);
            changeButton.SetActive(true);

        }
        else
        {
            selectedCharacter = 0;
            playerPrefabName = playerModel[selectedCharacter].GetComponent<PlayerModelName>().playerName;
            nextButton.SetActive(true);
            backwardButton.SetActive(true);
            selectButton.SetActive(true);
            mainScreen.SetActive(false);
            changeButton.SetActive(false);
        }

        foreach(GameObject player in playerModel)
        {
            player.SetActive(false);
        }

        playerModel[selectedCharacter].SetActive(true);
    }

    public void ChangeNext()
    {
        AudioManager.instance.PlaySFX(0);
        playerModel[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if (selectedCharacter == playerModel.Length)
            selectedCharacter = 0;
        playerModel[selectedCharacter].SetActive(true);
       
        playerPrefabName = playerModel[selectedCharacter].GetComponent<PlayerModelName>().playerName;

    }
    public void ChangeBack()
    {
        AudioManager.instance.PlaySFX(0);
        playerModel[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter == -1)
            selectedCharacter = playerModel.Length -1;
        playerModel[selectedCharacter].SetActive(true);
        
        playerPrefabName = playerModel[selectedCharacter].GetComponent<PlayerModelName>().playerName;

    }

    public void SelectChar()
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        nextButton.SetActive(false);
        backwardButton.SetActive(false);
        selectButton.SetActive(false);
        mainScreen.SetActive(true);
        changeButton.SetActive(true);
        AudioManager.instance.PlaySFX(3);
    }

    public void ChangeChar(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;
            PlayerPrefs.SetInt("Gold", gold);
            PlayerPrefs.DeleteKey("SelectedCharacter");
            nextButton.SetActive(true);
            backwardButton.SetActive(true);
            selectButton.SetActive(true);
            mainScreen.SetActive(false);
            changeButton.SetActive(false);
            AudioManager.instance.PlaySFX(0);
        }
       
    }
}
