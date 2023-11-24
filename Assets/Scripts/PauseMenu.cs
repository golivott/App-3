using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Slider sensSlider;
    public TextMeshProUGUI sensValue;
    public Slider soundSlider;
    public TextMeshProUGUI soundValue;

    public PlayerCam playerCam;
    
    private bool isPaused;

    private void Start()
    {
        Resume();
        float sens = PlayerPrefs.GetFloat("sensitivity", 1f);
        sensSlider.value = sens;
        UpdateSens(sens);
        float volume = PlayerPrefs.GetFloat("volume", 0.1f);
        soundSlider.value = volume;
        UpdateSound(volume);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (isPaused)
        {
            float sens = Mathf.Round(sensSlider.value * 100f) / 100f;
            UpdateSens(sens);
            float sound = Mathf.Round(soundSlider.value * 100f) / 100f;
            UpdateSound(sound);
        }
    }

    private void UpdateSens(float sens)
    {
        sensValue.text = sens.ToString();
        playerCam.sensX = sens;
        playerCam.sensY = sens;
        PlayerPrefs.SetFloat("sensitivity", sens);
    }
    
    private void UpdateSound(float sound)
    {
        soundValue.text = sound.ToString();
        PlayerPrefs.SetFloat("volume", sound);
    }

    void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerPrefs.Save();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        LanguageController.UpdateTextLanguage();
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
