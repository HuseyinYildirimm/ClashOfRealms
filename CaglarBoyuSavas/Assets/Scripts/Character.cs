using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public abstract class Character : MonoBehaviour
{
    Animator anim;
    protected Rigidbody rb;
    [SerializeField] private CharacterScriptableObject character;
    GameManager gameManager;
    Transform spawnPoint;

    [Space(10)]
    public LayerMask layerMask;
    public bool canMove;
    public float currentHealth;
    public bool isDead = false;
    public bool isAttacking;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        currentHealth = character.MaxHealth;
    }
    public void FixedUpdate()
    {
        AttackDetect();

        if (!canMove || gameObject.CompareTag("Enemy") && StopDetect())
        {
            StopMove();
        }
        else Move();


        if (isDead) gameObject.SetActive(false);
    }
    #region Movement

    protected void Move()
    {
        Vector3 parentRight = transform.parent.TransformDirection(Vector3.right); 
        Vector3 newPosition = transform.position + parentRight * character.MovementSpeed * Time.deltaTime;

        rb.MovePosition(newPosition);
    }

    private void StopMove() => rb.velocity = Vector3.zero;

    public bool StopDetect()
    {
        if (!Physics.Raycast(transform.position, transform.right, character.StopDistance, layerMask))
        {
            return false;
        }
        return true;
    }

    #endregion

    public void AttackDetect()
    {
        if (Physics.CheckSphere(transform.position, character.AttackDistance))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, character.AttackDistance);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Character") && !gameObject.CompareTag("Character")
                    || collider.gameObject.CompareTag("Enemy") && !gameObject.CompareTag("Enemy"))
                {
                    Character targetCharacter = collider.gameObject.GetComponent<Character>();
                    if (targetCharacter != null && !isAttacking)
                    {
                        isAttacking = true;
                        targetCharacter.TakeDamage(character.Damage);
                        StartCoroutine(AttackTimer());
                    }
                }
            }
        }
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(character.AttackTimer);
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            if (gameObject.CompareTag("Enemy"))
            {
                gameManager.exp += character.Exp;
                gameManager.money += character.KillReward;

                #region MoneyValueText

                Vector3 moneyObj = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

                GameObject moneyClone = Instantiate(gameManager.moneyObj, moneyObj, Quaternion.identity, gameManager.transform);
                moneyClone.transform.DOMove(gameManager.transform.position, 2).SetEase(Ease.OutCubic);
                Destroy(moneyClone, 1.8f);

                #endregion
            }
            else
            {
                gameManager.AIexp += character.Exp;
                gameManager.AImoney += character.KillReward * 2f;
            }

           
        }
        #region DamageValueText

        Vector3 damageVal = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        TextMeshProUGUI damageValueClone = Instantiate(gameManager.DamageValue, damageVal, Quaternion.identity, gameManager.DamageValue.transform.parent);

        gameManager.DamageValue.text =character.Damage.ToString("F0");

        Vector3 originalPosition = damageValueClone.rectTransform.anchoredPosition3D;
        Vector3 targetPosition = originalPosition + Vector3.up * 30f;

        damageValueClone.rectTransform.DOAnchorPos3D(targetPosition, 1f).SetEase(Ease.OutBack);
        Destroy(damageValueClone.gameObject, 1f);
        #endregion
    }

    protected abstract void UseAbility();

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4);
    }
}
