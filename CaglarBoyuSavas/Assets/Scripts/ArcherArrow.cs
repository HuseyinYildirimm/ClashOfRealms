using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : MonoBehaviour
{
    private float activeTime;
    Rigidbody rb;
    CapsuleCollider col;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("SpecialArrow"))
        {
            activeTime = 0f;
            collision.gameObject.GetComponent<Character>().TakeDamage(300f);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            activeTime = 1.5f;
            col.enabled = false;
            rb.isKinematic = true;
        }
        else
        {
            activeTime = 0f;
        }
        StartCoroutine(ArrowActiveTime());
    }

    IEnumerator ArrowActiveTime()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
        col.enabled = true;
        rb.isKinematic = false;
    }
}
