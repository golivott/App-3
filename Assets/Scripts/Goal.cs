using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float gold = PlayerPrefs.GetFloat("Gold", 0);
            PlayerPrefs.SetFloat("Gold", gold + DungeonManager.Instance.currGold);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Win");
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(col.center,col.size);
    }
}
