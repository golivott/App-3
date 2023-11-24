using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public GameObject closePos;
    public float speed;

    private void FixedUpdate()
    {
        if (DungeonManager.Instance.unlockExit)
        {
            transform.position = Vector3.Lerp(transform.position, closePos.transform.position, Time.deltaTime * speed);
        }
    }
}
