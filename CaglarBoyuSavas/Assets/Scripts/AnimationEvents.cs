using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    Archer archer;

    public void Start()
    {
        archer = GetComponentInParent<Archer>();
    }

    public void CallDamage()
    {
        gameObject.GetComponentInParent<Character>().Damage();
    }

    public void ArcherArrowActive()
    {
        if (archer != null) archer.arrow.SetActive(true);
    }

    public void ArcherArrowDeActive()
    {
        if (archer != null) archer.arrow.SetActive(false);
    }
}
