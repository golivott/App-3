using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }
    
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
}
