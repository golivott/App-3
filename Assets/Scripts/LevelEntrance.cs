using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEntrance : MonoBehaviour
{
    public int nextLevel;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveSystem.Instance.SaveScene();
            DungeonManager.Instance.previousScene = SceneManager.GetActiveScene().name;
            DungeonManager.Instance.currLevel = nextLevel;
            if (nextLevel == 1)
            {
                SceneManager.LoadScene("Level1");
            }
            else if (nextLevel == 2)
            {
                SceneManager.LoadScene("Level2");
            }
            else if (nextLevel == 3)
            {
                SceneManager.LoadScene("Level3");
            }
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(transform.position+col.center,col.size);
    }
}
