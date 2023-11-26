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
    private PlayerMove playerMove;

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
    public GameObject cardMain;
    public TextMeshProUGUI cardTitle;
    public TextMeshProUGUI cardDesc;
    
    [Header("Audio")] 
    public AudioSource footstep;
    public AudioSource heartBeat;
    public AudioSource artifactPickup;

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

        cardMain.SetActive(false);
        playerMove = player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Rigidbody>().velocity.magnitude > 4f)
        {
            footstep.pitch = 1f;
            
            if(player.GetComponent<Rigidbody>().velocity.magnitude > 7f) footstep.pitch = 1.5f;

            if (!footstep.isPlaying && playerMove.grounded)
            {
                footstep.Play();
            }
        }
        else
        {
            footstep.Stop();
        }

        if (!heartBeat.isPlaying)
        {
            heartBeat.pitch = DungeonManager.Instance.currAnger / 15f + 0.5f;
            heartBeat.Play();
        }

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

    public void ShowCard(Card card)
    {
        cardTitle.name = card.cardName;
        cardDesc.name = card.description;
        StartCoroutine(CardAnimation());
    }

    private IEnumerator CardAnimation()
    {
        cardMain.SetActive(true);
        LanguageController.UpdateTextLanguage();
        RectTransform rectTransform = cardMain.GetComponent<RectTransform>();
        Vector2 startPos = new Vector2(rectTransform.anchoredPosition.x, -(rectTransform.rect.height + rectTransform.anchoredPosition.y));
        Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        // Move the card onto the screen over the course of 1 second
        float duration = 1f;
        float time = 0;
        while (time < duration)
        {
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, time / duration);
            rectTransform.anchoredPosition = newPos;
            yield return null;
            time += Time.deltaTime;
        }

        // Ensure the card is exactly at the target position
        rectTransform.anchoredPosition = targetPos;

        // Hold there for 2 seconds
        yield return new WaitForSecondsRealtime(2f);

        // Move the card off the screen over the course of 2 seconds
        time = 0;
        duration = 2f;
        while (time < duration)
        {
            Vector2 newPos = Vector2.Lerp(targetPos, startPos, time / duration);
            rectTransform.anchoredPosition = newPos;
            yield return null;
            time += Time.deltaTime;
        }

        // Ensure the card is exactly at the normal position
        rectTransform.anchoredPosition = targetPos;
        cardMain.SetActive(false);
    }
}
