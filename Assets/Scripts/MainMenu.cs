using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject difficultyMenu;

    public GameObject dungeonManager;

    private void Start()
    {
        int firstLoad = PlayerPrefs.GetInt("FirstLoad", 0);
        if (firstLoad == 0)
        {
            Deck.SaveDeck();
            PlayerPrefs.SetInt("FirstLoad", 1);
        }
        
        float volume = PlayerPrefs.GetFloat("volume", 0.1f);
        AudioListener.volume = volume;
        
        OpenMain();
    }

    public void StartGame(int difficulty)
    {
        DungeonManager.difficulty = difficulty;
        Instantiate(dungeonManager);
        SceneManager.LoadScene("Level1");
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("GoldShop");
    }

    public void OpenDeckBuilder()
    {
        SceneManager.LoadScene("DeckBuilder");
    }

    public void OpenSettings()
    {
        CloseAll();
        settingsMenu.SetActive(true);
        LanguageController.UpdateTextLanguage();
    }
    
    public void OpenMain()
    {
        CloseAll();
        mainMenu.SetActive(true);
        LanguageController.UpdateTextLanguage();
    }
    
    public void OpenDifficulty()
    {
        CloseAll();
        difficultyMenu.SetActive(true);
        LanguageController.UpdateTextLanguage();
    }

    private void CloseAll()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        difficultyMenu.SetActive(false);
    }
}
