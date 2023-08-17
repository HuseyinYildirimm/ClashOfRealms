using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float money;
    public TextMeshProUGUI moneyAmount;
    CharacterScriptableObject character;

    public void Update()
    {
        moneyAmount.text = money.ToString(("F0"));
        money += Time.deltaTime;
    }


}
