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

    [Header("Health")]
    public float BaseMaxHealth = 1000;
    public float BaseCurrentHealth;
    public Image basehealthBar;

    [Space(10)]

    public float EnemyMaxHealth = 1000;
    public float EnemyCurrentHealth;
    public Image enemyBaseHealthBar;
    public float lerpspeed;

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
    public TextMeshProUGUI moneyRise;
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
    bool isEnemeyLevelUp;

    [Space(10)]

    [Header("PARTICLE")]
    public GameObject bloodEffect;

    [Space(10)]

    [Header("CHEST")]
    public Animator desertChest;
    public Transform desertCoinFirstTarget;
    public Transform desertcoinSecondlyTarget;
    public Animator darkChest;
    public Transform darkCoinFirstTarget;
    public Transform darkCoinSecondlyTarget;

    public void Start()
    {
        aiLevel = GetComponent<AICharacterSpawn>();

        Level1.SetActive(true);
        Level2.SetActive(false);
        Level1Base.SetActive(true);
        Level2Base.SetActive(false);

        EnemyCurrentHealth = EnemyMaxHealth;
        BaseCurrentHealth = BaseMaxHealth;
    }

    public void Update()
    {

        Health();

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
            if (exp >= 6000f)
            {
                LevelUp();
                isLevelUp = false;
            }
            else isLevelUp = false;
        }
        if (aiLevel.AILevelUp && !isEnemeyLevelUp)
        {
            EnemyMaxHealth += 400f;
            EnemyCurrentHealth = EnemyMaxHealth;

            isEnemeyLevelUp = true;
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
        BaseMaxHealth += 400f;
        BaseCurrentHealth = BaseMaxHealth;

        Level1Base.SetActive(false);
        Level2Base.SetActive(true);

        BaseLevelButton.SetActive(false);
        oldAgeButtons.SetActive(false);
        newAgeButtons.SetActive(true);
    }

    public void Health()
    {
        basehealthBar.fillAmount = Mathf.Lerp(basehealthBar.fillAmount, BaseCurrentHealth / BaseMaxHealth, lerpspeed);

        enemyBaseHealthBar.fillAmount = Mathf.Lerp(enemyBaseHealthBar.fillAmount, EnemyCurrentHealth / EnemyMaxHealth, lerpspeed);
    }
   
    public void OpenChest()
    {
        if(desertChest!=null)
            desertChest.SetTrigger("open");

        if(darkChest !=null)
            darkChest.SetTrigger("open");
    }

    public void CloseChest()
    {
        if (desertChest != null)
            desertChest.SetTrigger("close");

        if (darkChest != null)
            darkChest.SetTrigger("close");
    }


}
