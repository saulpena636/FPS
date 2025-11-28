using UnityEngine;
using UnityEngine.AI;

public class IA : MonoBehaviour
{
    public NavMeshAgent navMeshagent;
    public Transform[] destinations;
    private int i = 0;
    
    [Header("--------- Animation ---------------")]
    public Animator animator;

    [Header("--------- Follow Player ---------------")]
    public bool followPlayer;
    private GameObject player;
    private float distanceToPlayer;
    
    [Tooltip("Distancia a la que el enemigo empieza a perseguirte")]
    public float distanceToFollow = 10f;
    
    [Tooltip("Distancia mínima para llegar a un punto de patrulla")]
    public float distanceToFollowPath = 2f;

    [Header("--------- Combat ---------------")]
    [Tooltip("Distancia a la que el enemigo se detiene para golpear")]
    public float distanceToAttack = 2.5f;
    
    [Tooltip("Tiempo de espera entre golpes (segundos)")]
    public float attackCooldown = 1.5f; 
    private float lastAttackTime;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        
        navMeshagent.destination = destinations[i].transform.position;
        
        PlayerMovement pMovement = FindObjectOfType<PlayerMovement>();
        if (pMovement != null) player = pMovement.gameObject;
    }

    void Update()
    {
        if (player == null) return;

        // Actualizamos la animación de movimiento (0 si está parado, + si se mueve)
        // Usamos velocity.magnitude para saber si el NavMesh se está moviendo realmente
        animator.SetFloat("Speed", navMeshagent.velocity.magnitude);

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // LÓGICA PRINCIPAL
        if (followPlayer && distanceToPlayer <= distanceToFollow)
        {
            // Si está lo suficientemente cerca para atacar...
            if (distanceToPlayer <= distanceToAttack)
            {
                PerformAttack();
            }
            else
            {
                // Si está cerca para seguir, pero lejos para atacar, sigue corriendo
                FollowPlayer();
            }
        }
        else
        {
            // Si el jugador está lejos, patrulla
            EnemyPath();
        }
    }

    public void EnemyPath()
    {
        // Nos aseguramos de que el agente pueda moverse
        if (navMeshagent.isStopped) navMeshagent.isStopped = false;

        if (navMeshagent.isOnNavMesh)
        {
            navMeshagent.destination = destinations[i].position;
        }

        if (!navMeshagent.pathPending && navMeshagent.remainingDistance <= distanceToFollowPath)
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
        // Reactivamos el movimiento si estaba detenido por atacar
        if (navMeshagent.isStopped) navMeshagent.isStopped = false;

        if (navMeshagent.isOnNavMesh)
        {
            navMeshagent.destination = player.transform.position;
        }
    }

    public void PerformAttack()
    {
        // 1. Detener al personaje para que no se deslice mientras golpea
        navMeshagent.isStopped = true;

        // 2. Mirar al jugador (Importante: El NavMesh no rota si está parado)
        RotateTowardsPlayer();

        // 3. Revisar el tiempo (Cooldown) para no spamear ataques
        if (Time.time - lastAttackTime > attackCooldown)
        {
            // Disparar la animación
            animator.SetTrigger("Attack");
            
            // Aquí podrías poner lógica de daño (ej: player.TakeDamage(10))
            Debug.Log("¡Golpe lanzado!");

            // Resetear el temporizador
            lastAttackTime = Time.time;
        }
    }

    // Pequeña función auxiliar para rotar suavemente hacia el jugador al atacar
    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
