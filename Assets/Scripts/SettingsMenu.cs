using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sensSlider;
    public TextMeshProUGUI sensValue;
    public Slider soundSlider;
    public TextMeshProUGUI soundValue;

    private void Start()
    {
        float sens = PlayerPrefs.GetFloat("sensitivity", 1f);
        sensSlider.value = sens;
        UpdateSens(sens);
        float volume = PlayerPrefs.GetFloat("volume", 0.1f);
        soundSlider.value = volume;
        AudioListener.volume = volume;
        UpdateSound(volume);
    }

    // Update is called once per frame
    void Update()
    {
        float sens = Mathf.Round(sensSlider.value * 100f) / 100f;
        UpdateSens(sens);
        float sound = Mathf.Round(soundSlider.value * 100f) / 100f;
        UpdateSound(sound);
    }

    private void UpdateSens(float sens)
    {
        sensValue.text = sens.ToString();
        PlayerPrefs.SetFloat("sensitivity", sens);
    }
    
    private void UpdateSound(float sound)
    {
        soundValue.text = sound.ToString();
        AudioListener.volume = sound;
        PlayerPrefs.SetFloat("volume", sound);
    }

    public void ResetSaved()
    {
        PlayerPrefs.SetFloat("Gold", 0f);
        Deck defaultDeck = Resources.Load<Deck>("Deck/DefaultDeck");
        Deck deck = Resources.Load<Deck>("Deck/Deck");
        deck.deck = defaultDeck.deck;
        deck.stockpile = defaultDeck.stockpile;
        deck.maxCards = defaultDeck.maxCards;
        Deck.SaveDeck();
    } 

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
