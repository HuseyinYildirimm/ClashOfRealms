using UnityEngine;

public class Brute : Character
{
    public Transform attackVfxPoint;

    protected override void UseAbility()
    {
        GameObject attackVfxClone = Instantiate(character.AttackVFX, attackVfxPoint.position,attackVfxPoint.rotation,attackVfxPoint);
        Destroy(attackVfxClone, 4f);

        if (audioManager != null)
            audioManager.Play("Punch");
    }
}
