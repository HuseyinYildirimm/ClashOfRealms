using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Character
{
    bool attackSound;
    [SerializeField] private GameObject trailObj;

    protected override void UseAbility()
    {
        //special ability
    }

    public void Update()
    {
        if (audioManager != null && isAttacking && !attackSound)
        {
            audioManager.Play("Spear");
            trailObj.SetActive(true);
            attackSound = true;
        }
        else if (!isAttacking)
        {
            trailObj.SetActive(false);
            attackSound = false;
        }
    }
}
