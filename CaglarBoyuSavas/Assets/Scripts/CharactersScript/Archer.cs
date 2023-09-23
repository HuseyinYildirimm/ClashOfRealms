using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Archer : Character
{
    public Rig rig;
    

    protected override void UseAbility()
    {
       
    }

    public void Update()
    {
        
        if (archerAttacking) rig.weight = Mathf.Lerp(0f,1f,3f );

        else rig.weight = Mathf.Lerp(1f, 0f, 3f );
    }
}
