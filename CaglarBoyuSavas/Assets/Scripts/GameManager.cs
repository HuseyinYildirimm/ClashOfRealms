using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject moneyObj;
    public float money;
    public float exp;
    public float AImoney;
    public float AIexp;
    public TextMeshProUGUI moneyAmount;
    public TextMeshProUGUI expAmount;
    public TextMeshProUGUI AImoneyAmount;
    public TextMeshProUGUI DamageValue;

    public GameObject BaseLevelButton;
    public GameObject oldAgeButtons;
    public GameObject newAgeButtons;
    bool isLevelUp;


    public void Update()
    {
        #region Money

        AImoneyAmount.text = AImoney.ToString("F0");
        moneyAmount.text = money.ToString("F0");
        expAmount.text = exp.ToString("F0");

        money += Time.deltaTime;
        AImoney += Time.deltaTime;

        if(money <=0f) money = 0f;
        if (AImoney <= 0f) AImoney = 0f;

        if (isLevelUp)
        {
            if(exp > 5000f)
            {
                LevelUp();
                exp -= 5000f;
            }
        }

        #endregion
    }

    public void BaseLevelClick()
    {
        isLevelUp = true;
    }
    
    void LevelUp()
    {
        oldAgeButtons.SetActive(false);
        newAgeButtons.SetActive(true);
    }

}
