using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public abstract class Character : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [HideInInspector] public AudioSource audioSource;
    protected Rigidbody rb;

    public CharacterScriptableObject character;
    private GameManager gameManager;
    [HideInInspector]
    public AudioManager audioManager;
    private UIManager uiManager;
    public BloodObjectPool bloodOjbect;
    private AICharacterSpawn aiLevel;
    private SpecialAbilities specialAbilities;
    [HideInInspector] public Character targetCharacter;

    public float currentHealth;
    [Space(10)]

    public LayerMask layerMask;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isAttacking;
    bool baseControl;
    [HideInInspector] public bool archerAttacking;
  
    Transform CoinFirstTarget;
    Transform CoinSecondlyTarget;

    RaycastHit hit;
    CapsuleCollider col;

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aiLevel = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AICharacterSpawn>();
        bloodOjbect = GameObject.FindGameObjectWithTag("BloodObjectPool").GetComponent<BloodObjectPool>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        specialAbilities = GameObject.FindGameObjectWithTag("AbilitiesManager").GetComponent<SpecialAbilities>();

        audioSource = GetComponent<AudioSource>();

        character.AudioSourceVolume = uiManager.SfxSlider.value;
        currentHealth = character.CurrentHealth;
    }

    public void FixedUpdate()
    {
        if ( gameManager.BaseCurrentHealth <= 0 || gameManager.EnemyCurrentHealth <=0 || gameObject.CompareTag("Base") ||
            gameObject.CompareTag("EnemyBase")) return;

        audioSource.volume = character.AudioSourceVolume;

        AttackDetect();

        if (!canMove || gameObject.CompareTag("Enemy") && StopDetect())
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
        if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.right, character.StopDistance, layerMask) && !baseControl)
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

                    targetCharacter =collider.gameObject.GetComponent<Character>();

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
        if (targetCharacter != null )
        {
            targetCharacter.TakeDamage(character.Damage);
        }
        UseAbility();
    }

    public void TakeDamage(float damage)
    {
        if (specialAbilities.armor && gameObject.CompareTag("Character")) return;
        Debug.Log("xd");
        currentHealth -= damage;
        
        if(character.ClassType == ClassType.Base)
        gameManager.BaseCurrentHealth = currentHealth;

        if (character.ClassType == ClassType.EnemyBase)
            gameManager.EnemyCurrentHealth = currentHealth;

        audioSource.PlayOneShot(character.TakeDamageSound);

        bloodOjbect.ActivateBloodEffect(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), transform.rotation);

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            audioSource.PlayOneShot(character.DyingSound);

            anim.Play("Death");
            col.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;

            Destroy(gameObject, 4f);

            if (gameObject.CompareTag("Enemy")) audioManager.Play("CoinSound");


            if (gameObject.CompareTag("Enemy"))
            {
                gameManager.exp += character.Exp;

                gameManager.money += character.KillReward;
                gameManager.moneyRise.text = "+" + character.KillReward.ToString("F0");
                StartCoroutine(UISettings());

                #region MoneyValueText

                Vector3 moneyObj = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                Vector3 moneyTarget = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);

                GameObject moneyClone = Instantiate(gameManager.moneyObj, moneyObj, Quaternion.identity, gameManager.transform);

                if (moneyClone != null)
                {
                    if (aiLevel.AILevelUp)
                    {
                        CoinFirstTarget = gameManager.darkCoinFirstTarget;
                        CoinSecondlyTarget = gameManager.darkCoinSecondlyTarget;
                    }
                    else
                    {
                        CoinFirstTarget = gameManager.desertCoinFirstTarget;
                        CoinSecondlyTarget = gameManager.desertcoinSecondlyTarget;
                    }

                    gameManager.OpenChest();
                    moneyClone.transform.DOMove(CoinFirstTarget.position, 1).SetEase(Ease.OutCubic).
                    OnComplete(() => moneyClone.transform.DOMove(CoinSecondlyTarget.position, 1).SetEase(Ease.OutCubic).
                    OnComplete(() => gameManager.CloseChest()));
                    moneyClone.transform.DORotate(new Vector3(0f, 720f, 0f), 2, RotateMode.FastBeyond360);

                    Destroy(moneyClone, 2.01f);
                }

                #endregion
            }
            else
            {
                #region DifficultyLevel

                gameManager.AIexp += character.Exp * 1.3f;

                if (gameManager.difficultyLevel == 0) gameManager.AImoney += character.KillReward * 1.3f;

                else if (gameManager.difficultyLevel == 1) gameManager.AImoney += character.KillReward * 1.8f;

                else gameManager.AImoney += character.KillReward * 2.3f;

                #endregion
            }
        }

        #region DamageValueText

        Vector3 damageVal = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        TextMeshProUGUI damageValueClone = Instantiate(gameManager.DamageValue, damageVal, Quaternion.identity, gameManager.DamageValue.transform.parent);

        gameManager.DamageValue.text = character.Damage.ToString("F0");

        Vector3 originalPosition = damageValueClone.rectTransform.anchoredPosition3D;
        Vector3 targetPosition = originalPosition + Vector3.up * 30f;

        damageValueClone.rectTransform.DOAnchorPos3D(targetPosition, 1f).SetEase(Ease.OutBack);
        damageValueClone.rectTransform.rotation = Quaternion.LookRotation(-Camera.main.transform.position);

        Destroy(damageValueClone.gameObject, 1f);
        #endregion
    }

    #endregion

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
