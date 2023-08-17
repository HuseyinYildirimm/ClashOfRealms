using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawn : MonoBehaviour
{
    private CharacterScriptableObject pendingCharacter;
    GameManager gameManager;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Slider SpawnSlider;
    private bool isTimerStarted = false;
    private float timer = 0.0f;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void OnButtonClick(CharacterScriptableObject characterobj)
    {
        if (isTimerStarted) return;
        pendingCharacter = characterobj;
        SpawnSlider.maxValue = pendingCharacter.spawnTimer;
        SpawnSlider.value = SpawnSlider.maxValue;

        isTimerStarted = true;
        timer = 0.0f;
    }

    private void Update()
    {
        if (isTimerStarted)
        {
            if (gameManager.money >= pendingCharacter.Price)
            {
                timer += Time.deltaTime;

                if (SpawnSlider.value > 0f) SpawnSlider.value -= Time.deltaTime * 2;


                if (SpawnSlider.value == 0f)
                {
                    isTimerStarted = false;
                    SpawnSlider.value = SpawnSlider.maxValue;

                    GameObject newCharacter = Instantiate(pendingCharacter.character, spawnPoint.position, Quaternion.identity, spawnPoint);

                    gameManager.money -= pendingCharacter.Price;
                    pendingCharacter = null;
                }
            }
        }
    }
}