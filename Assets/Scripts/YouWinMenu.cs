using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinMenu : MonoBehaviour
{
    private void Start()
    {
        // Clean up
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            while (player.transform.parent != null)
            {
                player = player.transform.parent.gameObject;
            }
            Destroy(player);
        }
        
        GameObject artifact = GameObject.FindGameObjectWithTag("Artifact");
        if (artifact != null)
        {
            while (artifact.transform.parent != null)
            {
                artifact = artifact.transform.parent.gameObject;
            }
            Destroy(artifact);
        }
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        LanguageController.UpdateTextLanguage();
    }

    public void LoadGemShop()
    {
        SceneManager.LoadScene("GemShop");
    }
}
