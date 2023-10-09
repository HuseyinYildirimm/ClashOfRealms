using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAbilities : MonoBehaviour
{
    [Header("SPECIALARROW")]
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private GameObject arrowLoop0;
    [SerializeField] private GameObject arrowLoop1;
    [SerializeField] private Image arrowFill;
    private float lerpSpeed =0.1f;
    bool specialArrow;

    [Header("SPECÝALARMOR")]
    [SerializeField] private GameObject armorEffect;
    [SerializeField] private CharacterSpawn characters;
    [SerializeField] private Image armorFill;
    [HideInInspector] public bool armor;
    bool specialArmor;

    public void Update()
    {
        if (specialArrow)
        {
            arrowFill.fillAmount = Mathf.Lerp(arrowFill.fillAmount, 0f, lerpSpeed*Time.deltaTime);

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
            armorFill.fillAmount = Mathf.Lerp(armorFill.fillAmount, 0f, lerpSpeed * Time.deltaTime);
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
            Debug.Log("Armor");
            armorFill.fillAmount = 1f;
        }
    }

    IEnumerator SpecialArmorTimer()
    {
        yield return new WaitForSeconds(6f);
        armor = false;
    }

    #endregion
}
