using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    public string id;
    public Vector3 position;
    public bool triggered;

    private void Awake()
    {
        id = transform.position.sqrMagnitude + "-" + name;
    }

    public void SaveState()
    {
        position = transform.position;
        
        // For saving hazard state
        Hazard script = GetComponent<Hazard>(); 
        if (script != null)
        {
            triggered = script.triggered;
        }
    }

    public void LoadState()
    {
        transform.position = position;
        
        // For loading hazard state
        Hazard script = GetComponent<Hazard>(); 
        if (script != null)
        {
            script.triggered = triggered;
        }
    }
}
