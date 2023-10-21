using System.Collections;
using UnityEngine;
using DG.Tweening;
public class TurretMechanic : MonoBehaviour
{
    Animator anim;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Transform rotObj;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject buildingEffect;
    [Space(10)]

    [Header("TURRET ACTION")]
    [SerializeField] private float attackTime;
    [SerializeField] private float turretDamage;
    [SerializeField] private float rotateSpeed;
    public bool turretAttack;
    [SerializeField] private GameObject attackEffect;
    float detectionRadius = 15f;

    GameObject firstTarget = null;
    string audioName = null;

    public void Start()
    {
        anim = GetComponent<Animator>();

        GameObject buildingEffectClone = Instantiate(buildingEffect, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        Destroy(buildingEffectClone, 3.5f);

        if (gameObject.CompareTag("WeakTurret")) audioName = "WeakTurret";
        if (gameObject.CompareTag("StrongTurret")) audioName = "StrongTurret";
    }

    public void Update()
    {
        TurretTarget();
    }

    public void TurretTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if ((gameObject.name != "BaseTurretLevel1" && gameObject.name != "BaseTurretLevel2" && collider.gameObject.CompareTag("Character")) ||
                  (gameObject.name != "EnemyTurretLevel1" && gameObject.name != "EnemyTurretLevel2" && collider.gameObject.CompareTag("Enemy")))
            {
                float distanceToTarget = Vector3.Distance(transform.position, collider.transform.position);

                if (distanceToTarget < closestDistance)
                {
                    firstTarget = collider.gameObject;
                    closestDistance = distanceToTarget;
                }
            }
        }

        if (firstTarget != null)
        {
            if (firstTarget.GetComponent<Character>().currentHealth > 0)
            {
                if (!turretAttack) TurretAttack(firstTarget.transform);
              
                Vector3 targetDirection = firstTarget.transform.position - rotObj.transform.position;
                Quaternion newRotation = Quaternion.LookRotation(targetDirection);
                rotObj.transform.rotation = Quaternion.Slerp(rotObj.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }
    }

    void TurretAttack(Transform target)
    {
        anim.Play("Attack");
        turretAttack = true;
        StartCoroutine(TurretAttackTimer());

        if (audioManager != null)
            audioManager.Play(audioName);

        bullet.transform.position = bullet.transform.parent.position;
        bullet.SetActive(true);
        attackEffect.SetActive(true);

        Vector3 turretTarget = new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z);

        bullet.transform.DOMove(turretTarget, 0.2f).OnComplete(() =>
        {
            target.gameObject.GetComponent<Character>().TakeDamage(turretDamage);
            bullet.SetActive(false);
            attackEffect.SetActive(false);
        });
    }

    IEnumerator TurretAttackTimer()
    {
        yield return new WaitForSeconds(attackTime);
        turretAttack = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 15f);
        Gizmos.DrawRay(transform.position, transform.right);
    }
}
