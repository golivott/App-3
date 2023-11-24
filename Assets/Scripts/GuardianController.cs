using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GuardianController : MonoBehaviour
{
    public float huntSpeed;
    public float wanderSpeed;
    public LayerMask playerMask;
    public float wanderRange;
    public Light sLight;
    public float damage;
    private NavMeshAgent nav;
    private Vector3 spawnPos = Vector3.zero;
    private bool hunting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        spawnPos = transform.position;
        nav.avoidancePriority = Random.Range(1, 51);
    }

    // Update is called once per frame
    void Update()
    {
        // adjust speed if hunting
        nav.speed = hunting ? huntSpeed : wanderSpeed;
        sLight.color = hunting ? Color.red : Color.white;
        sLight.intensity = hunting ? 20 : 2;
        
        // Calculate current and destination positions excluding y coord
        Vector3 currPos = transform.position;
        currPos.y = 0;
        Vector3 destPos = nav.destination;
        destPos.y = 0;
        
        // We are hunting
        if (hunting && currPos == destPos) hunting = false;

        // We are wandering
        if (!hunting && currPos == destPos)
        {
            Vector2 randomPos = Random.insideUnitCircle * wanderRange;
            Vector3 targetPos = new Vector3(randomPos.x, 0, randomPos.y);
            targetPos += spawnPos;
            nav.destination = targetPos;
        }
        
        // Determine rotate toward targetPos
        Vector3 targetDirection = nav.velocity;
        targetDirection.y = 0f;
        float singleStep = 4f * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection.normalized, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void moveToPlayer()
    {
        Vector3 playerPos = Player.Instance.player.transform.position;
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        
        Ray ray = new Ray(currentPos, playerPos - currentPos);
        Debug.DrawRay(currentPos, playerPos - currentPos, Color.red);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 10000f, playerMask ))
        {
            if (raycastHit.transform.CompareTag("Player"))
            {
                nav.destination = playerPos + Player.Instance.player.GetComponent<Rigidbody>().velocity.normalized;
                hunting = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRB = other.gameObject.GetComponent<Rigidbody>();
            Vector3 forceDir = Player.Instance.player.transform.position - transform.position;
            forceDir.y = 0;
            playerRB.AddForce(forceDir.normalized * 20f + Vector3.up * 10f, ForceMode.Impulse);
            Player.Instance.Damage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(spawnPos == Vector3.zero ? transform.position : spawnPos, wanderRange);
    }
}
