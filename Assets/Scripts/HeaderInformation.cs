using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class HeaderInformation : MonoBehaviourPun
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerLevel;
    public Image healthBar;
    private float maxHealthValue;

    public void InitializedEnemy(string text, int maxVal)
    {
        playerName.text = text;
        maxHealthValue = maxVal;
        healthBar.fillAmount = 1.0f;
    }
    public void InitializedPlayer(int level, string text, int maxVal)
    {
        playerName.text = text;
        maxHealthValue = maxVal;
        healthBar.fillAmount = 1.0f;
        playerLevel.text ="" + level;
    }

    [PunRPC]
    void UpdateHealthBar(int value)
    {
        healthBar.fillAmount = (float)value / maxHealthValue;
    }

    [PunRPC]
    void UpdatePlayerLevel(int value)
    {
        playerLevel.text = "" + value;
    }
}
