using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class Archer : Character
{
    public Rig rig;
    public GameObject handArrow;
    public GameObject arrowPref;
    GameObject arrowClone;
    
    [SerializeField] Transform arrowPoint;

    protected override void UseAbility()
    {
        Vector3 enemyTarget = new Vector3(targetCharacter.transform.position.x, targetCharacter.transform.position.y + 2, targetCharacter.transform.position.z);

        arrowClone= Instantiate(arrowPref, arrowPoint.position, arrowPoint.rotation,gameObject.transform);

        arrowClone.transform.DOMove(enemyTarget, 0.2f).OnComplete(() => Destroy(arrowClone));

        if (audioManager != null)
            audioManager.Play("Arrow");

    }

    public void Update()
    {
        if (archerAttacking) rig.weight = Mathf.Lerp(0f,1f,3f );

        else rig.weight = Mathf.Lerp(1f, 0f, 3f );
    }
}
