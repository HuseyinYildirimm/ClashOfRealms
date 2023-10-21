using UnityEngine;

public class Knight : Character
{
    [SerializeField] private GameObject trailObj;

    protected override void UseAbility()
    {
        if (audioManager != null)
            audioManager.Play("Sword");
    }
    public void Update()
    {
        if (!isAttacking) trailObj.SetActive(false);

        else trailObj.SetActive(true);
    }
}
