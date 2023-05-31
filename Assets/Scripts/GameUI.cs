using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI hpText;

    public static GameUI instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateGoldText(int goldAmount)
    {
        goldText.text = "" + goldAmount;
    }

    public void UpdateHpText(int curHP, int maxHP)
    {
        hpText.text = "" + curHP + "/" +""+ maxHP;
    }

}
