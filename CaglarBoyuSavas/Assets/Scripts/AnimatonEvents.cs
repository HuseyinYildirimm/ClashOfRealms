using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatonEvents : MonoBehaviour
{
    public void CallDamage()
    {
        gameObject.GetComponentInParent<Character>().Damage();
    }
}
