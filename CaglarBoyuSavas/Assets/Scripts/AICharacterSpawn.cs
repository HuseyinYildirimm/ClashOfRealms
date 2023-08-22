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

    private List<GameObject> spawnedCharacters = new List<GameObject>();
    private int currentLevel = 1;
    
    [Header("Time")]
    private bool isTimerStarted = false;
    private float timer = 0.0f;
    private float spawnTimer;
    private float spawnInterval = 5.0f;
    private float minTime = 1f;
    private float maxTime = 10f;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //sort by price
        charactersToSpawn = charactersToSpawn.OrderBy(character => character.Price).ToArray();
        AISpawnCharacter();

        spawnInterval = Random.Range(minTime, maxTime);
    }

    private void SetSpawnTimer()
    {
        spawnTimer = Random.Range(minTime, maxTime);
        maxTime -= 0.1f * Time.deltaTime;
        maxTime = Mathf.Max(minTime, maxTime); 
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            isTimerStarted = true;
            SetSpawnTimer();
        }


        if (isTimerStarted)
        {
            timer += Time.deltaTime;

            if (spawnTimer > 0f) spawnTimer -= Time.deltaTime * 2;

            if (spawnTimer <= 0f)
            {
                AISpawnCharacter();
            }
        }

        for (int i = 0; i < spawnedCharacters.Count; i++)
        {
            Character character = spawnedCharacters[i].GetComponent<Character>();

            if (character.isDead)
            {
                Destroy(spawnedCharacters[i]);
                spawnedCharacters.RemoveAt(i);
                i--;
                break;
            }

            if (i > 0)
            {
                Vector3 previousPosition = spawnedCharacters[i - 1].transform.position;
                Vector3 currentPosition = spawnedCharacters[i].transform.position;

                float distance = Vector3.Distance(previousPosition, currentPosition);

                if (distance < minDistance) character.canMove = false;

                else character.canMove = true;
            }

            else if (character.StopDetect()) character.canMove = false;

            else character.canMove = true;
        }
    }

    private void AISpawnCharacter()
    {
        isTimerStarted = false;
        CharacterScriptableObject selectedCharacter = GetHighestAffordableCharacter();

        if (selectedCharacter != null)
        {
            GameObject newCharacter = Instantiate(selectedCharacter.character, spawnPoint.position, Quaternion.identity, spawnPoint);
            spawnedCharacters.Add(newCharacter);
            newCharacter.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            newCharacter.tag = "Enemy";

            spawnTimer = selectedCharacter.spawnTimer;
            gameManager.AImoney -= selectedCharacter.Price;

            if (gameManager.AImoney > selectedCharacter.Price * 2) currentLevel++;

            else currentLevel = Mathf.Max(currentLevel - 1, 1);
          
        }
        else Debug.Log("Yeterli para yok.");
    }

    private CharacterScriptableObject GetHighestAffordableCharacter()
    {
        CharacterScriptableObject highestAffordableCharacter = null;

        foreach (CharacterScriptableObject character in charactersToSpawn)
        {
            if (gameManager.AImoney >= character.Price && (highestAffordableCharacter == null
                || character.Price > highestAffordableCharacter.Price))
            {
                highestAffordableCharacter = character;
            }
        }

        return highestAffordableCharacter;
    }


}

