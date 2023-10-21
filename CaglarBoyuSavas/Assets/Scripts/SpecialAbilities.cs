using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAbilities : MonoBehaviour
{
    private float threshold = 0.09f;
    private float lerpSpeed =0.05f;

    [Header("SPECIALARROW")]
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private GameObject arrowLoop0;
    [SerializeField] private GameObject arrowLoop1;
    [SerializeField] private Image arrowFill;
    bool specialArrow;

    [Space(10)]
    [SerializeField] private GameObject[] AIarrows;
    [SerializeField] private GameObject AIarrowLoop0;
    [SerializeField] private GameObject AIarrowLoop1;
    bool aiSpecialArrow;

    [Header("SPECÝALARMOR")]
    [SerializeField] private GameObject armorEffect;
    [SerializeField] private CharacterSpawn characters;
    [SerializeField] private Image armorFill;
    [HideInInspector] public bool armor;
    bool specialArmor;

    [Space(10)]

    [SerializeField] private AICharacterSpawn enemies;
    [HideInInspector] public bool aiArmor;

    public void Start()
    {
        InvokeRepeating("AISpecialArmor", 30f, 90f);
        InvokeRepeating("AISpecialArrow", 45f, 60f);
    }

    public void Update()
    {
        if (specialArrow)
        {
            if (arrowFill.fillAmount < threshold) arrowFill.fillAmount = 0f;
           
            else arrowFill.fillAmount = Mathf.Lerp(arrowFill.fillAmount, 0f, lerpSpeed * Time.deltaTime);

            if (arrowFill.fillAmount <= 0.05)
            {
                foreach (GameObject arrow in arrows)
                {
                    arrow.transform.position = arrow.transform.parent.position;
                }
                arrowLoop0.SetActive(false);
                arrowLoop1.SetActive(false);
            }
        }

        if (specialArmor)
        {
            if (armorFill.fillAmount < threshold) armorFill.fillAmount = 0f;
           
            else armorFill.fillAmount = Mathf.Lerp(armorFill.fillAmount, 0f, lerpSpeed * Time.deltaTime);
           
        }
    }

    #region SPECÝAL ABILITY ARROW

    public void SpecialArrow()
    {
       if(arrowFill.fillAmount <= 0.001)
        {
            StartCoroutine(SpecialArrowTimer());
            specialArrow = true;
            arrowFill.fillAmount = 1f;
        }
    }

    IEnumerator SpecialArrowTimer()
    {
        arrowLoop0.SetActive(true);
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        arrowLoop1.SetActive(true);
    }
    #endregion

    #region SPECÝAL ABILITY ARMOR

    public void SpecialArmor()
    {
        if (armorFill.fillAmount <= 0.001)
        {
            foreach (var c in characters.spawnedCharacters)
            {
                GameObject armorClone = Instantiate(armorEffect, c.transform.position, c.transform.rotation, c.transform);
                Destroy(armorClone, 6f);
            }

            StartCoroutine(SpecialArmorTimer());
            specialArmor = true;
            armor = true;
            armorFill.fillAmount = 1f;
        }
    }

    IEnumerator SpecialArmorTimer()
    {
        yield return new WaitForSeconds(6f);
        armor = false;
    }

    #endregion

    #region AI SPECIAL ABILITY

    void AISpecialArmor()
    {
        foreach (var c in enemies.AIspawnedCharacters)
        {
            GameObject armorClone = Instantiate(armorEffect, c.transform.position, c.transform.rotation, c.transform);
            Destroy(armorClone, 6f);
        }
       
        aiArmor = true;
        StartCoroutine(AISpecialArmorTimer());

    }
    IEnumerator AISpecialArmorTimer()
    {
        yield return new WaitForSeconds(6f);
        aiArmor = false;
    }

    void AISpecialArrow()
    {
        Debug.LogError("arrow");
        aiSpecialArrow = true;
        if (aiSpecialArrow) StartCoroutine(AISpecialArrowControl());
    }

    IEnumerator AISpecialArrowControl()
    {
        AIarrowLoop0.SetActive(true);
        foreach (GameObject arrow in AIarrows)
        {
            arrow.SetActive(true);
            arrow.GetComponent<Rigidbody>().isKinematic = false;
        }
        yield return new WaitForSeconds(2f);
        AIarrowLoop1.SetActive(true);

        yield return new WaitForSeconds(4f);
        foreach (GameObject arrow in AIarrows)
        {
            arrow.SetActive(false);
            arrow.transform.position = arrow.transform.parent.position;
        }
        AIarrowLoop0.SetActive(false);
        AIarrowLoop1.SetActive(false);
        aiSpecialArrow = false;
    }

    #endregion
}
