using UnityEngine;

public class Spearman : Character
{
   
    [SerializeField] private GameObject trailObj;

    protected override void UseAbility()
    {
        audioManager.Play("Spear");
    }

    public void Update()
    {
        if ( isAttacking) trailObj.SetActive(true);
      
        else if (!isAttacking) trailObj.SetActive(false);

    }
}
