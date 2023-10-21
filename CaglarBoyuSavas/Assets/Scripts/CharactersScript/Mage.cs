using UnityEngine;

public class Mage : Character
{
    protected override void UseAbility()
    {
        GameObject attackVfxClone = Instantiate(character.AttackVFX, targetCharacter.transform.position,Quaternion.identity,targetCharacter.transform);
        Destroy(attackVfxClone, 1.2f);

        if (audioManager != null) audioManager.Play("Magic");

    }

    public void Update()
    {
       
    }
}
