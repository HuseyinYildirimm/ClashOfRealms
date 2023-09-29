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
    private AudioManager audioManager;
    public BloodObjectPool bloodOjbect;
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
        bloodOjbect = GameObject.Find("BloodObjectPool").GetComponent<BloodObjectPool>();

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        
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

                    if (collider.gameObject.CompareTag("Base"))
                    {
                        gameManager.BaseCurrentHealth -= character.Damage;
                    }
                    else if (collider.gameObject.CompareTag("EnemyBase"))
                    {
                        gameManager.EnemyCurrentHealth -= character.Damage;
                    }

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

        bloodOjbect.ActivateBloodEffect(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), transform.rotation);
        
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            audioSource.PlayOneShot(character.DyingSound);

            anim.Play("Death");
            collider.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;

            Destroy(gameObject, 4f);

            if (gameObject.CompareTag("Enemy")) audioManager.Play("CoinSound");


            if (gameObject.CompareTag("Enemy"))
            {
                gameManager.exp += character.Exp;

                gameManager.money += character.KillReward;
                gameManager.moneyRise.text = "+"+character.KillReward.ToString("F0");
                StartCoroutine(UISettings());

                #region MoneyValueText

                Vector3 moneyObj = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                Vector3 moneyTarget = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);

                GameObject moneyClone = Instantiate(gameManager.moneyObj, moneyObj, Quaternion.identity, gameManager.transform);

                moneyClone.transform.DOMove(GetIconPosition(moneyTarget), 2).SetEase(Ease.OutCubic);
                moneyClone.transform.DORotate(new Vector3(0f, 720f, 0f), 2, RotateMode.FastBeyond360);

                Destroy(moneyClone, 1.8f);

                #endregion
            }
            else
            {
                #region DifficultyLevel

                gameManager.AIexp += character.Exp* 1.3f;

                if (gameManager.difficultyLevel == 0) gameManager.AImoney += character.KillReward * 1.3f;

                else if(gameManager.difficultyLevel == 1) gameManager.AImoney += character.KillReward * 1.8f;

                else gameManager.AImoney += character.KillReward * 2.3f;

                #endregion
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


    IEnumerator UISettings()
    {
        yield return new WaitForSeconds(1f);
        gameManager.moneyRise.text = null;
    }
}
