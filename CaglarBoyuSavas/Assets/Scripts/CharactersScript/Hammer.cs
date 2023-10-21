using UnityEngine;

public class Hammer : Character
{
    [SerializeField] private GameObject trailObj;

    protected override void UseAbility()
    {
        if (audioManager != null) audioManager.Play("Hammer");
        trailObj.SetActive(true);
    }
    public void Update()
    {
        if (!isAttacking) trailObj.SetActive(false);
    }
}
