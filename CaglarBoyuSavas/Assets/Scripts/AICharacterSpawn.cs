using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICharacterSpawn : MonoBehaviour
{
    GameManager gameManager;
    public float minDistance= 2.0f;
    [HideInInspector] public bool AILevelUp;

    [Header ("Spawn")]
    public Transform spawnPoint;
    public CharacterScriptableObject[] charactersToSpawn; 
    private List<GameObject> AIspawnedCharacters = new List<GameObject>();
    int i;

    [Header("LEVEL")]
    private int LevelUpExp;
    private int currentLevel = 0;
    private int baseLevel = 0;

    [Header("Time")]
    private bool isTimerStarted = false;
    private float timer = 0.0f;
    private float spawnTimer;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
      
        //sort by price
        charactersToSpawn = charactersToSpawn.OrderBy(character => character.Price).ToArray();

        AISpawnCharacter();

        if (gameManager.difficultyLevel == 0) LevelUpExp = 6500;
        if (gameManager.difficultyLevel == 1) LevelUpExp = 5000;
        if (gameManager.difficultyLevel == 2) LevelUpExp = 4000;

    }

    public void FixedUpdate()
    {
        LevelControlByMoney();

        //queue-move control
        for ( i = 0; i < AIspawnedCharacters.Count; i++)
        {
            Character character = AIspawnedCharacters[i].GetComponent<Character>();

            if (character.isDead)
            {
                AIspawnedCharacters.RemoveAt(i);
                i--;
                break;
            }

            if (i > 0)
            {
                Vector3 previousPosition = AIspawnedCharacters[i - 1].transform.position;
                Vector3 currentPosition = AIspawnedCharacters[i].transform.position;

                float distance = Vector3.Distance(previousPosition, currentPosition);

                if (distance < minDistance) character.canMove = false;

                else character.canMove = true;
            }

            else if (character.StopDetect()) character.canMove = false;

            else character.canMove = true;
        }
    }

    private void Update()
    {
        SpawnTimeControl();
    }

    void LevelControlByMoney()
    {
        if (gameManager.AIexp > LevelUpExp)
        {
            AILevelUp = true;
            baseLevel = 1;
            if (gameManager.AImoney > 900) currentLevel = Random.Range(8, 10);

            else if (gameManager.AImoney > 600) currentLevel = Random.Range(6, 7);

            else if (gameManager.AImoney > 250) currentLevel = Random.Range(5, 7);

            else currentLevel = 5;
        }
        else if (baseLevel == 0)
        {
            if (gameManager.AImoney > 400) currentLevel = Random.Range(2, 4);

            else if (gameManager.AImoney > 300) currentLevel = Random.Range(1, 3);

            else if (gameManager.AImoney > 200) currentLevel = Random.Range(0, 2);

            else if (gameManager.AImoney > 100) currentLevel = Random.Range(0, 1);

            else currentLevel = 0;
        }
    }

    void SpawnTimeControl()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0) isTimerStarted = true;

        if (isTimerStarted && i<10)
        {
            timer += Time.deltaTime;

            if (spawnTimer > 0f) spawnTimer -= Time.deltaTime * 2;

            if (spawnTimer <= 0f) AISpawnCharacter();
        }
    }

    private void AISpawnCharacter()
    {
        isTimerStarted = false;
        CharacterScriptableObject selectedCharacter = GetAffordableCharacter();
      
        if (selectedCharacter != null && gameManager.AImoney >selectedCharacter.Price)
        {
            GameObject newCharacter = Instantiate(selectedCharacter.character, spawnPoint.position, Quaternion.identity, spawnPoint);
            AIspawnedCharacters.Add(newCharacter);
            newCharacter.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            newCharacter.tag = "Enemy";

            gameManager.AImoney -= selectedCharacter.Price;
            selectedCharacter.Reset();

            #region DIFFICULTY LEVEL 

            if (gameManager.difficultyLevel ==0)
            {
                if (currentLevel > 6)
                {
                    selectedCharacter.CurrentHealth += 3;
                    spawnTimer = Random.Range(3, 7);
                }
                else if(currentLevel >3)
                {
                    selectedCharacter.CurrentHealth += 2;
                    spawnTimer = Random.Range(4, 8);
                }
                else
                {
                    selectedCharacter.CurrentHealth += 1;
                    spawnTimer = Random.Range(5, 9);
                }
            }
            else if(gameManager.difficultyLevel == 1)
            {
                if (currentLevel > 6)
                {
                    selectedCharacter.CurrentHealth += 5;
                    spawnTimer = Random.Range(3, 6);
                }
                else if (currentLevel > 3)
                {
                    selectedCharacter.CurrentHealth += 4;
                    spawnTimer = Random.Range(4, 7);
                }
                else
                {
                    selectedCharacter.CurrentHealth += 3;
                    spawnTimer = Random.Range(5, 8);
                }
            }
            else
            {
                if (currentLevel > 6)
                {
                    selectedCharacter.CurrentHealth += 9;
                    spawnTimer = Random.Range(3, 5);
                }
                else if (currentLevel > 3)
                {
                    selectedCharacter.CurrentHealth += 7;
                    spawnTimer = Random.Range(4, 6);
                }
                else
                {
                    selectedCharacter.CurrentHealth += 5;
                    spawnTimer = Random.Range(5, 7);
                }
            }

            #endregion

        }
    }

    private CharacterScriptableObject GetAffordableCharacter()
    {
        CharacterScriptableObject selectedCharacter = null;

        selectedCharacter = charactersToSpawn[currentLevel];
      
        return selectedCharacter;
    }
}

