using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Character
{
    [SerializeField] private GameObject[] trailObj;

    protected override void UseAbility()
    {
        if (audioManager != null)
            audioManager.Play("Sword");
    }
    public void Update()
    {
        if (!isAttacking)
        {
            trailObj[0].SetActive(false);
            trailObj[1].SetActive(false);
        }

        else 
        {
            trailObj[0].SetActive(true);
            trailObj[1].SetActive(true);
        }
    }
}
