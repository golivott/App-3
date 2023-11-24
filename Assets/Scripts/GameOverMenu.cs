using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
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
        
        GameObject dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager");
        if (dungeonManager != null)
        {
            while (dungeonManager.transform.parent != null)
            {
                dungeonManager = dungeonManager.transform.parent.gameObject;
            }
            Destroy(dungeonManager);
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

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
