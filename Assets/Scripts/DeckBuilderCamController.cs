using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilderCamController : MonoBehaviour
{
    public Slider scrollbar;
    public float scrollDist;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.position = startPos + new Vector3(0f, 0f,-scrollDist) * scrollbar.value;
    }
}
