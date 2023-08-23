using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterSpawn : MonoBehaviour
{
    GameManager gameManager;
    public float minDistance= 2.0f;

    [Header ("Spawn")]
    public Transform spawnPoint;
    [SerializeField] private CharacterScriptableObject[] charactersToSpawn; 

    private List<GameObject> AIspawnedCharacters = new List<GameObject>();
    private int currentLevel = 0;
    
    [Header("Time")]
    private bool isTimerStarted = false;
    private float timer = 0.0f;
    private float spawnTimer;
    private float minTime = 1f;
    private float maxTime =5f;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
      
        //sort by price
        charactersToSpawn = charactersToSpawn.OrderBy(character => character.Price).ToArray();
     
        AISpawnCharacter();
    }
    
    public void FixedUpdate()
    {
        LevelControlByMoney();

        //queue-move control
        for (int i = 0; i < AIspawnedCharacters.Count; i++)
        {
            Character character = AIspawnedCharacters[i].GetComponent<Character>();

            if (character.isDead)
            {
                Destroy(AIspawnedCharacters[i]);
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
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0) isTimerStarted = true;
      
        if (isTimerStarted)
        {
            timer += Time.deltaTime;

            if (spawnTimer > 0f) spawnTimer -= Time.deltaTime * 2;

            if (spawnTimer <= 0f) AISpawnCharacter();
        }
    }

    void LevelControlByMoney()
    {
        if (gameManager.AImoney > 400) currentLevel = Random.Range(2,4);

        else if (gameManager.AImoney > 300) currentLevel = Random.Range(1, 3);

        else if (gameManager.AImoney > 200) currentLevel = Random.Range(0, 2);

        else if (gameManager.AImoney > 100) currentLevel = Random.Range(0, 1);

        else currentLevel = 0;
    }

    private void AISpawnCharacter()
    {
        isTimerStarted = false;
        CharacterScriptableObject selectedCharacter = GetAffordableCharacter();
      
        if (selectedCharacter != null)
        {
            GameObject newCharacter = Instantiate(selectedCharacter.character, spawnPoint.position, Quaternion.identity, spawnPoint);
            AIspawnedCharacters.Add(newCharacter);
            newCharacter.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            newCharacter.tag = "Enemy";

            //Difficulty Level settings
            spawnTimer = selectedCharacter.spawnTimer; //spawnTimer = Random.Range(1,5);
            gameManager.AImoney -= selectedCharacter.Price;
        }
    }

    private CharacterScriptableObject GetAffordableCharacter()
    {
        CharacterScriptableObject selectedCharacter = null;

        selectedCharacter = charactersToSpawn[currentLevel];
      
        return selectedCharacter;
    }
}

