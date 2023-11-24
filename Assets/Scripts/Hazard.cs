using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    public bool triggered;
    
    // Add this to the dungeon manager
    private void OnEnable() => DungeonManager.hazards.Add(this);
    private void OnDisable() => DungeonManager.hazards.Remove(this);
    
    public abstract void Trigger();
}
