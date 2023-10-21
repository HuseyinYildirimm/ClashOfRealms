using UnityEngine;

public class Warrior : Character
{
    protected override void UseAbility()
    {
        if (audioManager != null )
        {
            audioManager.Play("Sword");
        }
    }
}
