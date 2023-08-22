using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float money;
    public float AImoney;
    public TextMeshProUGUI moneyAmount;
    public TextMeshProUGUI AImoneyAmount;
    public void Update()
    {
        AImoneyAmount.text = AImoney.ToString("F0");
        moneyAmount.text = money.ToString(("F0"));
        money += Time.deltaTime;
        AImoney += Time.deltaTime;

        if(money <=0f) money = 0f;
        if (AImoney <= 0f) AImoney = 0f;
    }


}
