using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sorceress : Character
{
    [SerializeField] private Transform attackPoint;

    protected override void UseAbility()
    {
        GameObject attackVfxClone = Instantiate(character.AttackVFX, attackPoint.position, Quaternion.identity, gameObject.transform);

        Vector3 enemyTarget = new Vector3(targetCharacter.transform.position.x, targetCharacter.transform.position.y + 2, targetCharacter.transform.position.z);

        attackVfxClone.transform.DOMove(enemyTarget, 0.3f).OnComplete(() => Destroy(attackVfxClone));

        if (audioManager != null) audioManager.Play("Sorceress");

    }
    public void Update()
    {
       
    }
}
