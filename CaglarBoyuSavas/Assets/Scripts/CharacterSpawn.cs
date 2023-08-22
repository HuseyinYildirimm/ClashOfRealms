using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawn : MonoBehaviour
{
    GameManager gameManager;

    public Transform spawnPoint;
    public Slider SpawnSlider;
    public float minDistance = 2.0f; 

    private CharacterScriptableObject pendingCharacter;
    private bool isTimerStarted = false;
    private float timer = 0.0f;
    private List<GameObject> spawnedCharacters = new List<GameObject>();


    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (isTimerStarted && gameManager.money > pendingCharacter.Price)
        {
            timer += Time.deltaTime;

            if (SpawnSlider.value > 0f) SpawnSlider.value -= Time.deltaTime * 2;

            if (SpawnSlider.value <= 0f)
            {
                SpawnCharacter();
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

            else if(character.StopDetect()) character.canMove = false;

            else character.canMove = true;
        }
    }

    private void SpawnCharacter()
    {
        gameManager.money -= pendingCharacter.Price;
        isTimerStarted = false;
        SpawnSlider.value = SpawnSlider.maxValue;

        GameObject newCharacter = Instantiate(pendingCharacter.character, spawnPoint.position, Quaternion.identity, spawnPoint);
        spawnedCharacters.Add(newCharacter);
        newCharacter.tag = "Character";

        pendingCharacter = null;
    }

    public void OnButtonClick(CharacterScriptableObject characterobj)
    {
        if (isTimerStarted) return;
        pendingCharacter = characterobj;
        SpawnSlider.maxValue = pendingCharacter.spawnTimer;
        SpawnSlider.value = SpawnSlider.maxValue;
        pendingCharacter.CurrentHealth = pendingCharacter.MaxHealth;
        Debug.Log(pendingCharacter.name);
        isTimerStarted = true;
        timer = 0.0f;
    }
}
