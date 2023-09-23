using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public abstract class Character : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    private Camera mainCam;
    private AudioSource audioSource;
    protected Rigidbody rb;
    public CharacterScriptableObject character;
    private GameManager gameManager;

    private Character targetCharacter;
    public float currentHealth;
    [Space(10)]

    public LayerMask layerMask;
    public bool canMove;
    public bool isDead = false;
    public bool isAttacking;
    bool baseControl;
    [HideInInspector] public bool archerAttacking;

    RaycastHit hit;
    CapsuleCollider collider;

    public void Start()
    {
        mainCam = Camera.main;
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        currentHealth = character.CurrentHealth;
    }

    public void FixedUpdate()
    {
        AttackDetect();

        if (!canMove || gameObject.CompareTag("Enemy") && StopDetect() )
        {
            StopMove();
            
        }
        else Move();


        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.right, out hit, character.StopDistance))
        {
            if ((hit.collider.CompareTag("Base") && gameObject.CompareTag("Enemy")) || hit.collider.CompareTag("EnemyBase") && gameObject.CompareTag("Character")) baseControl = true;
        }
    }

    #region Movement

    protected void Move()
    {
       Vector3 parentRight = transform.parent.TransformDirection(Vector3.right); 
       Vector3 newPosition = transform.position + parentRight * character.MovementSpeed * Time.deltaTime;
       
       rb.MovePosition(newPosition);
        anim.SetBool("Move", true);
    }

    protected void StopMove()
    {
        rb.velocity = Vector3.zero;
        anim.SetBool("Move", false);
    }

    public bool StopDetect()
    {
        if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.right, character.StopDistance, layerMask) && !baseControl )
        {
            return false;
        }

        else return true;
    }

    #endregion

    #region Attack

    public void AttackDetect()
    {
        if (Physics.CheckSphere(transform.position, character.AttackDistance) && !isAttacking && !isDead)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, character.AttackDistance);
            foreach (Collider collider in colliders)
            {
                if (!gameObject.CompareTag("Character") && (collider.gameObject.CompareTag("Character") || collider.gameObject.CompareTag("Base")) ||
                     !gameObject.CompareTag("Enemy") && (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("EnemyBase")))
                {
                    targetCharacter = collider.gameObject.GetComponent<Character>();
                    isAttacking = true;
                    archerAttacking = true;
                    
                    anim.Play("Attack1");
                    anim.SetBool("Attack", true);  //for sorceress
                    audioSource.PlayOneShot(character.AttackSound);

                    StartCoroutine(AttackTimer());
                }
            }
        }
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(character.AttackTimer);
        isAttacking = false;
       
    }

    public void Damage() //AnimationEvent
    {
        if (targetCharacter != null)
        {
            targetCharacter.TakeDamage(character.Damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        audioSource.PlayOneShot(character.TakeDamageSound);

        if (currentHealth <= 0)
        {
            isDead = true;
            audioSource.PlayOneShot(character.DyingSound);
            collider.enabled = false;
            anim.Play("Death");
            Destroy(gameObject, 2f);



            if (gameObject.CompareTag("Enemy"))
            {
                gameManager.exp += character.Exp;

                gameManager.money += character.KillReward;
               
                #region MoneyValueText

                Vector3 moneyObj = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

                GameObject moneyClone = Instantiate(gameManager.moneyObj, moneyObj, Quaternion.identity, gameManager.transform);
                moneyClone.transform.DOMove(GetIconPosition(gameManager.iconTransform.position), 2).SetEase(Ease.OutCubic);
                Destroy(moneyClone, 1.8f);

                #endregion
            }
            else
            {
                gameManager.AIexp += character.Exp;

                if (gameManager.difficultyLevel == 0) gameManager.AImoney += character.KillReward * 1.2f;

                else if(gameManager.difficultyLevel == 1) gameManager.AImoney += character.KillReward * 1.5f;

                else gameManager.AImoney += character.KillReward * 2f;
            }
        }

        #region DamageValueText

        Vector3 damageVal = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        TextMeshProUGUI damageValueClone = Instantiate(gameManager.DamageValue, damageVal, Quaternion.identity, gameManager.DamageValue.transform.parent);

        gameManager.DamageValue.text =character.Damage.ToString("F0");

        Vector3 originalPosition = damageValueClone.rectTransform.anchoredPosition3D;
        Vector3 targetPosition = originalPosition + Vector3.up * 30f;

        damageValueClone.rectTransform.DOAnchorPos3D(targetPosition, 1f).SetEase(Ease.OutBack);
        damageValueClone.rectTransform.rotation = Quaternion.LookRotation(-Camera.main.transform.position);

        Destroy(damageValueClone.gameObject, 1f);
        #endregion
    }

    #endregion

    public Vector3 GetIconPosition(Vector3 target)
    {
        Vector3 uiPos = gameManager.iconTransform.position;
        uiPos.z = (target - mainCam.transform.position).z;
        Vector3 result = mainCam.ScreenToWorldPoint(uiPos);

        return result;
    }

    protected abstract void UseAbility();


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, character.AttackDistance);
        Gizmos.DrawRay(transform.position, transform.right);
    }
}
