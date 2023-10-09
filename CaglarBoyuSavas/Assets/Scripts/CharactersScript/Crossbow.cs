using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crossbow : Character
{
    public GameObject arrowPref;
    GameObject arrowClone;
    [SerializeField] private Transform arrowPoint;
  

    protected override void UseAbility()
    {
        Vector3 enemyTarget = new Vector3(targetCharacter.transform.position.x, targetCharacter.transform.position.y + 2, targetCharacter.transform.position.z);

        arrowClone = Instantiate(arrowPref, arrowPoint.position, arrowPoint.rotation, gameObject.transform);

        arrowClone.transform.DOMove(enemyTarget, 0.2f).OnComplete(() => Destroy(arrowClone));

        if (audioManager != null)
            audioManager.Play("Arrow");
    }
    public void Update()
    {
      
    }
}
