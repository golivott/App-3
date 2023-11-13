using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [HideInInspector] public float health;
    public float maxHealth;
    public float healthRegenPerSecond;
    public float healthRegenMultiplier;
    public float healthRegenDelaySeconds;
    private bool canHealthRegen = true;
    
    [Header("Damage Received")]
    public float damageMultiplier;
    public float invulTimeSeconds;
    private bool canTakeDamage = true;

    [Header("Movement")] 
    public float moveSpeedMulitpier;
    public float jumpMulitpier;

    public static Player Instance { get; private set; }
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
        
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Regen tick
        if (health < maxHealth && canHealthRegen)
        {
            canHealthRegen = false;
            this.Invoke(() => canHealthRegen = true, healthRegenDelaySeconds);
            health += healthRegenPerSecond * healthRegenMultiplier * Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth);
        }
    }

    // This damages the player
    public void Damage(float damage)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            this.Invoke(() => canTakeDamage = true, invulTimeSeconds);
            health = health - damage * damageMultiplier;    
        }
    }
}
