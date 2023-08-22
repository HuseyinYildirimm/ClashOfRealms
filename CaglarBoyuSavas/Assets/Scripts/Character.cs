using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            if (gameObject.CompareTag("Enemy")) gameManager.money += character.KillReward;

            else gameManager.AImoney += character.KillReward;
        }

    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(5f);
        isAttacking = false;
    }

    protected abstract void UseAbility();

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4);
    }
}
