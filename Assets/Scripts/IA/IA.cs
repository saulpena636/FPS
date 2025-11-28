using UnityEngine;
using UnityEngine.AI;

public class IA : MonoBehaviour
{
    public NavMeshAgent navMeshagent;
    public Transform[] destinations;
    private int i = 0;
    [Header("---------followPlayer---------------")]
    private GameObject player;
    public bool followPlayer;
    private float distanceToPlayer;
    private float distanceToFollow = 10f;
    public float distanceToFollowPath = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        navMeshagent.destination = destinations[i].transform.position;
        player = FindObjectOfType<PlayerMovement>().gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); 
        if (distanceToPlayer <= distanceToFollow && followPlayer)
        {
            FollowPlayer();
        }
        else
        {
            EnemyPath();
        }
    }

    public void EnemyPath()
    {
        navMeshagent.destination = destinations[i].position;
        if (Vector3.Distance(transform.position, destinations[i].position) <= distanceToFollowPath)
        {
            if (destinations[i] != destinations[destinations.Length - 1])
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }
    }
    public void FollowPlayer()
    {

        navMeshagent.destination = player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                
                PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    Vector3 dir = collision.transform.position - transform.position;
                    // Flatten the direction so they don't fly upwards too much unless intended
                    dir.y = 0; 
                    playerMovement.AddImpact(dir, 50f); // Adjust force as needed
                }
            }
        }
    }
}
