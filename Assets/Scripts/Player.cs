using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject player;
    public Vector3 compassTarget;
    
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
    public bool isSneaking;

    [Header("UI")] 
    public Slider healthBar;
    public Slider staminaBar;
    public TextMeshProUGUI gemCount;
    public Slider gemQueue;
    public GameObject gemQueueFill;
    public TextMeshProUGUI goldCount;
    public Slider goldQueue;
    public GameObject goldQueueFill;
    public TextMeshProUGUI angerBlockCount;
    public TextMeshProUGUI hazardBlockCount;
    public Image compassNeedle;
    public Image fadeToBlack;

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
        
        // UI Init
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        
        staminaBar.maxValue = 100f;
        staminaBar.value = GetComponentInChildren<PlayerMove>().stamina;
        
        fadeToBlack.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Regen tick
        if (health < maxHealth && canHealthRegen)
        {
            health += healthRegenPerSecond * healthRegenMultiplier * Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth);
        }
        
        // UI
        healthBar.value = health;
        healthBar.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Round(health).ToString();
        
        staminaBar.value = GetComponentInChildren<PlayerMove>().stamina;
        staminaBar.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Round(GetComponentInChildren<PlayerMove>().stamina).ToString();
        
        gemCount.text = DungeonManager.Instance.currGems.ToString();
        gemQueue.value = DungeonManager.Instance.gemQueue;
        gemQueueFill.SetActive(gemQueue.value != 0f);
        
        goldCount.text = DungeonManager.Instance.currGold.ToString();
        goldQueue.value = DungeonManager.Instance.goldQueue;
        goldQueueFill.SetActive(goldQueue.value != 0f);
        
        angerBlockCount.text = DungeonManager.Instance.angerBlock.ToString();
        hazardBlockCount.text = DungeonManager.Instance.hazardBlock.ToString();
        
        float playerRotation = player.GetComponent<PlayerMove>().orientation.transform.eulerAngles.y - 180f;
        Vector3 directionToTarget = player.transform.position - compassTarget;
        directionToTarget.y = 0;
        float angleToTarget = Vector3.SignedAngle(Vector3.forward, directionToTarget.normalized, Vector3.up);
        float angle = playerRotation - angleToTarget;
        compassNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // This damages the player
    public void Damage(float damage)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            this.Invoke(() => canTakeDamage = true, invulTimeSeconds);
            canHealthRegen = false;
            this.Invoke(() => canHealthRegen = true, healthRegenDelaySeconds);
            health = health - damage * damageMultiplier;
            if (health <= 0) KillPlayer();
        }
    }

    public void KillPlayer()
    {
        StartCoroutine(FadeToBlack());
    }
    
    private IEnumerator FadeToBlack()
    {
        Color color = fadeToBlack.color;
        float fadeSpeed = 0.1f;

        while (color.a < 1.0f)
        {
            canHealthRegen = false;
            health = 0;
            fadeToBlack.gameObject.SetActive(true);
            color.a += fadeSpeed * Time.deltaTime * 10f;
            fadeToBlack.color = color;
            yield return null;
        }

        SceneManager.LoadScene("GameOver");
    }
}
