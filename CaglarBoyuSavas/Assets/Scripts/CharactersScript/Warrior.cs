using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character
{
    bool attackSound;


    protected override void UseAbility()
    {
        if (audioManager != null )
        {
            audioManager.Play("Sword");
        }
    }

    public void Update()
    {
        
    }
}
