using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AICharacterSpawn characters;
    Camera cam;

    [Space(10)]

    [Header("SOUND")]
    [SerializeField] private AudioClip soundtrack;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip gameOverSound;

    private Transform characterTransform;
    private Character character;

    [Space(10)]

    [Header("STARTSCENE")]
    [SerializeField] private GameObject StartScene;
    bool startGame;

    [Space(10)]

    [Header("DIFFICULTYSCENE")]
    [SerializeField] private GameObject DifficultyScene;

    [Space(10)]

    [Header("OPTIONSSCENE")]
    [SerializeField] private GameObject OptionsScene;
    [SerializeField] private GameObject GameOptionsButton;

    [Space(10)]

    [Header("GAMEOVERSCENE")]
    [SerializeField] private GameObject GUI;
    [SerializeField] private GameObject GameOverScene;
    [SerializeField] private TextMeshProUGUI gameOverMat;
    public float startDilate = -0.5f;
    public float targetDilate = 0f;
    public float lerpDuration = 1f;
    private float t;
    private float currentDilate;
    private float lerpStartTime;
    private bool isLerpingGameOver = false;
    private bool gameOverUI;

    [Space(10)]


    [Header("WINSCENE")]
    [SerializeField] private GameObject winScene;
    [SerializeField] private TextMeshProUGUI winMat;
    private bool winUI;
    private bool isLerpingWin = false;

    [Space(10)]

    [Header("Graphics")]
    public TMP_Dropdown dropDown;
    public Volume volume;

    [Space(10)]

    [Header("Sound")]
    public Slider MusicSlider;
    public Slider SfxSlider;

    Vector3 healthBar;
  
    public void Start()
    {
        cam = Camera.main;

        Time.timeScale = 0;
        StartScene.SetActive(true);
        DifficultyScene.SetActive(false);
        OptionsScene.SetActive(false);
        GameOptionsButton.SetActive(false);
        GameOverScene.SetActive(false);
        winScene.SetActive(false);

        currentDilate = startDilate;
        gameOverMat.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentDilate);
        winMat.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentDilate);
        lerpStartTime = Time.time;

        characterTransform = null;
        character = null;
    }

    public void Update()
    {
        if (gameManager.gameOver && !isLerpingGameOver)
        {
            StartLerpGameOver();
        }

        if (isLerpingGameOver)
        {
            GameOver();
        }
        if (gameManager.Win && !isLerpingWin)
        {
            StartLerpWin();
        }

        if (isLerpingWin)
        {
            Win();
        }

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) //CharacterHealthBar
        {
            Vector2 inputPosition = Vector2.zero;

            if (Input.GetMouseButtonDown(0))
            {
                inputPosition = Input.mousePosition;
            }
            else if (Input.touchCount > 0)
            {
                inputPosition = Input.GetTouch(0).position;
            }

            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Character") || hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Character characterHealth = hit.collider.gameObject.GetComponent<Character>();

                    healthBar = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 4,
                    hit.collider.gameObject.transform.position.z);
                    SetCharacter(characterHealth, hit.collider.gameObject.transform);
                    gameManager.characterHealth.transform.position = healthBar;
                    gameManager.characterHealth.gameObject.SetActive(true);

                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (character != null)
        {
            UpdateHealthUI();
        }
    }

    #region HEALTHBAR

    public void SetCharacter(Character character, Transform characterTransform)
    {
        this.character = character;
        this.characterTransform = characterTransform;
    }

    private void UpdateHealthUI()
    {

        gameManager.characterHealth.fillAmount = Mathf.Lerp(gameManager.characterHealth.fillAmount, character.currentHealth / character.character.MaxHealth, 1f);

        if (characterTransform != null)
        {
            Vector3 healthBarPosition = new Vector3(characterTransform.position.x, characterTransform.position.y + 4f, characterTransform.position.z);
            gameManager.characterHealth.transform.position = healthBarPosition;
        }

        if (character.currentHealth <= 0) gameManager.characterHealth.gameObject.SetActive(false);

        gameManager.characterHealth.rectTransform.localRotation = Quaternion.LookRotation(cam.transform.position);

    }
    #endregion

    #region BUTTON

    public void GameSpeedDouble()
    {
        Time.timeScale = 2f;
    }

    public void GameSpeedNormal()
    {
        Time.timeScale = 1f;
    }

    public void StartButton()
    {
        Time.timeScale = 1;
        StartScene.SetActive(false);
        GameOptionsButton.SetActive(true);
        startGame = true;
        
        gameManager.GetComponent<AudioSource>().clip = soundtrack;
        gameManager.GetComponent<AudioSource>().Play();

    }

    public void DifficultyButton()
    {
        StartScene.SetActive(false);
        DifficultyScene.SetActive(true);

    }

    public void OptionsButton()
    {
        Time.timeScale = 0;

        StartScene.SetActive(false);
        DifficultyScene.SetActive(false);
        OptionsScene.SetActive(true);

    }

    public void MeinMenu()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void EasyModeButton()
    {
        gameManager.difficultyLevel = 0;
        StartScene.SetActive(true);
        DifficultyScene.SetActive(false);
    }

    public void NormalModeButton()
    {
        gameManager.difficultyLevel = 1;
        StartScene.SetActive(true);
        DifficultyScene.SetActive(false);
    }

    public void HardModeButton()
    {
        gameManager.difficultyLevel = 2;
        StartScene.SetActive(true);
        DifficultyScene.SetActive(false);
    }

    public void ExtremelyModeButton()
    {
        StartScene.SetActive(true);
        DifficultyScene.SetActive(false);
    }

    public void ReturnButton()
    {
        GraphicsSettings();

        if (!startGame)
        {
            StartScene.SetActive(true);
            DifficultyScene.SetActive(false);
            OptionsScene.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            StartScene.SetActive(false);
            DifficultyScene.SetActive(false);
            OptionsScene.SetActive(false);
        }
    }

    #endregion

    #region GRAPHICS

    void GraphicsSettings()
    {
        //Low
        if (dropDown.value == 0)
        {
            QualitySettings.SetQualityLevel(0);
            volume.weight = 0.3f;
        }

        //Medium
        if (dropDown.value == 1)
        {
            QualitySettings.SetQualityLevel(1);
            volume.weight = 0.7f;
        }

        //High
        if (dropDown.value == 2)
        {
            QualitySettings.SetQualityLevel(2);
            volume.weight = 1f;
        }
    }


    #endregion

    #region SOUND

    public void MusicVolume()
    {
        gameManager.GetComponent<AudioSource>().volume = MusicSlider.value;
    }

    public void SfxVolume()
    {
        foreach (var c in characters.charactersToSpawn)
        {
            c.AudioSourceVolume = SfxSlider.value;
        }
        if (audioManager != null)
        {
            audioManager.AdjustAllVolumes(SfxSlider.value);
        }
    }
  
    #endregion

    #region GAMEOVER

    public void StartLerpGameOver()
    {
        GUI.SetActive(false);
        isLerpingGameOver = true;
        lerpStartTime = Time.time;
        currentDilate = startDilate;
        GameOverScene.SetActive(true);

        gameManager.GetComponent<AudioSource>().Stop();
        gameManager.GetComponent<AudioSource>().PlayOneShot(gameOverSound);

        Time.timeScale = 1;
    }

    public void GameOver()
    {    
        float timeSinceStart = Time.time - lerpStartTime;
        t = timeSinceStart / lerpDuration;
        currentDilate = Mathf.Lerp(startDilate, targetDilate, t);
        gameOverMat.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentDilate);

        if (!gameOverUI)
        {
            gameOverUI = true;
            gameOverMat.rectTransform.DOScale(1.5f, 1f).OnComplete(() => gameOverMat.rectTransform.DOScale(1, 1f)).SetEase(Ease.OutBack).SetLoops(100);
        }

    }
    #endregion

    #region WIN

    public void StartLerpWin()
    {
        GUI.SetActive(false);
        isLerpingWin = true;
        lerpStartTime = Time.time;
        currentDilate = startDilate;
        winScene.SetActive(true);

        gameManager.GetComponent<AudioSource>().Stop();
        gameManager.GetComponent<AudioSource>().PlayOneShot(winSound);

        Time.timeScale = 1;
    }

    public void Win()
    {
        float timeSinceStart = Time.time - lerpStartTime;
        t = timeSinceStart / lerpDuration;
        currentDilate = Mathf.Lerp(startDilate, targetDilate, t);
        winMat.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentDilate);

        if (!winUI)
        {
            winUI = true;
            winMat.rectTransform.DOScale(1.5f, 1f).OnComplete(() => winMat.rectTransform.DOScale(1, 1f)).SetEase(Ease.OutBack).SetLoops(100);
        }
    }

    #endregion

}


