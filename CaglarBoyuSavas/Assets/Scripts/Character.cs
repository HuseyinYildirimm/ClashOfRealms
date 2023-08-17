using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    Animator anim;
    protected Rigidbody rb;
    [SerializeField] private CharacterScriptableObject character;

    [Space(10)]
    public LayerMask layerMask;

   

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if (StopDetect())
        {
          StopMove();
        }
        else Move();

    }

    protected void Move()
    {
        rb.velocity = transform.right * Time.deltaTime * character.MovementSpeed;
    }
    private void StopMove()
    {
        rb.velocity = Vector3.zero;
    }

    public bool StopDetect()
    {
        if (!Physics.Raycast(transform.position, transform.right, character.StopDistance, layerMask))
        {
            return false;
        }
        return true;
    }

    public void AttackDetect()
    {
        if (Physics.CheckSphere(transform.position, 1))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, character.AttackDistance);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    //Attack

                }
            }
        }
    }

    protected abstract void UseAbility();

  
}
