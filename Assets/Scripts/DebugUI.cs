using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public GameObject debugUI;
    public TextMeshProUGUI angerValue;
    private bool isOpen;

    private void Start()
    {
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        if (isOpen)
        {
            angerValue.text = DungeonManager.Instance.currAnger.ToString();
        }
    }

    void Close()
    {
        debugUI.SetActive(false);
        isOpen = false;
    }

    void Open()
    {
        debugUI.SetActive(true);
        isOpen = true;
    }
}
