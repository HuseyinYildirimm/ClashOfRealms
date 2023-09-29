using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodObjectPool : MonoBehaviour
{
    public GameObject bloodEffectPrefab;
    public int poolSize = 10;

    private List<GameObject> bloodEffectPool;

    private void Start()
    {
        bloodEffectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, Vector3.zero, Quaternion.identity);
            bloodEffect.SetActive(false);
            bloodEffect.transform.parent = transform;
            bloodEffectPool.Add(bloodEffect);
        }
    }

    public void ActivateBloodEffect(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject bloodEffect in bloodEffectPool)
        {
            if (!bloodEffect.activeInHierarchy)
            {
                bloodEffect.transform.position = position;
                bloodEffect.transform.rotation = rotation;
                bloodEffect.SetActive(true);
              
                StartCoroutine(DeactivateAfterDelay(bloodEffect, 2f));
                break;
            }
        }
    }

    private IEnumerator DeactivateAfterDelay(GameObject bloodEffect, float delay)
    {
        yield return new WaitForSeconds(delay);
        bloodEffect.SetActive(false);
    }
}

