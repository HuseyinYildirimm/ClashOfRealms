using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AICharacterSpawn characters;
                                                                                                                                                                
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

    [Header("Graphics")]
    public TMP_Dropdown dropDown;
    public Volume volume;

    [Space(10)]

    [Header("Sound")]
    [SerializeField] private Slider MusicSlider;
    public Slider SfxSlider;

    public void Start()
    {
        Time.timeScale = 0;
        StartScene.SetActive(true);
        DifficultyScene.SetActive(false);
        OptionsScene.SetActive(false);
        GameOptionsButton.SetActive(false);


    }

    public void StartButton()
    {
        Time.timeScale = 1;
        StartScene.SetActive(false);
        GameOptionsButton.SetActive(true);
        startGame = true;
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

    #region GRAPHICS

    void GraphicsSettings()
    {
        //Low
        if (dropDown.value == 0)
        {
            QualitySettings.SetQualityLevel(0);
            volume.weight = 0f;
        }

        //Medium
        if (dropDown.value == 1) 
        {
            QualitySettings.SetQualityLevel(1);
            volume.weight = 0.6f;
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
        audioManager.GetComponent<AudioSource>().volume = MusicSlider.value;
    }

    public void SfxVolume()
    {
        foreach (var c in characters.charactersToSpawn)
        {
            c.AudioSourceVolume = SfxSlider.value;
        }
    }

    #endregion

}
