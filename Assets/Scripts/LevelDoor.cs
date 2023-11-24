using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    public GameObject closePos;
    public float speed;
    public float level;

    private void FixedUpdate()
    {
        if (DungeonManager.Instance.lowestLevelUnlocked >= level)
        {
            transform.position = Vector3.Lerp(transform.position, closePos.transform.position, Time.deltaTime * speed);
        }
    }
}
