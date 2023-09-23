using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    AICharacterSpawn aiLevel;

    [Header("LevelSettings")]
    public int difficultyLevel;
    [Space(10)]
    public GameObject Level1;
    public GameObject Level1Base;
    [Space(10)]
    public GameObject Level2;
    public GameObject Level2Base;
    [Space(10)]
    public Material Level2Skybox;

    [Space(10)]

    [Header("MONEY & EXP")]
    public GameObject moneyObj;
    public float money;
    public float exp;
    public float AImoney;
    public float AIexp;

    [Space(10)]

    [Header("TMP")]
    public TextMeshProUGUI moneyAmount;
    public TextMeshProUGUI expAmount;
    public TextMeshProUGUI AImoneyAmount;
    public TextMeshProUGUI DamageValue;
    public Transform iconTransform;

    [Space(10)]

    [Header("BUTTONS")]

    public GameObject BaseLevelButton;
    public GameObject oldAgeButtons;
    public GameObject newAgeButtons;
    bool isLevelUp;

    [Space(10)]

    [Header("SOUND")]
    public AudioClip GameMusicAC;
    private AudioSource GameMusicAS;

    public void Start()
    {
        aiLevel = GetComponent<AICharacterSpawn>();
        GameMusicAS = GetComponent<AudioSource>();

        Level1.SetActive(true);
        Level2.SetActive(false);
        Level1Base.SetActive(true);
        Level2Base.SetActive(false);
        GameMusicAS.Play();
    }

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

        #endregion

        if (isLevelUp)
        {
            if (exp >= 5000f)
            {
                LevelUp();
            }
            else isLevelUp = false;
        }
        if (aiLevel.AILevelUp)
        {
            Level1.SetActive(false);
            Level2.SetActive(true);

            RenderSettings.skybox = Level2Skybox;
        }
    }

    public void BaseLevelClick()
    {
        isLevelUp = true;
    }
    
    void LevelUp()
    {
        Level1Base.SetActive(false);
        Level2Base.SetActive(true);


        BaseLevelButton.SetActive(false);
        oldAgeButtons.SetActive(false);
        newAgeButtons.SetActive(true);
    }


}
