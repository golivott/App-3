using UnityEngine;

public class AngerGenerator : MonoBehaviour
{
    public float triggerCD;
    public float angerAmount;
    public AudioSource angerSound;
    
    // Add this to the dungeon manager
    private void OnEnable() => DungeonManager.angerGenerators.Add(this);
    private void OnDisable() => DungeonManager.angerGenerators.Remove(this);

    private bool triggered = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !Player.Instance.isSneaking && !triggered)
        {
            triggered = true;
            // Reckless charge buff
            if (DungeonManager.Instance.recklessCharge)
            {
                DungeonManager.Instance.AddGems(8);
            }
            angerSound.Play();
            DungeonManager.Instance.AddAnger(angerAmount);
            Invoke("ResetTrigger", triggerCD);
        }
    }

    private void ResetTrigger()
    {
        triggered = false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
    #endif
}
