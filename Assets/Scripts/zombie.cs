using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator anim; // TAMBAH INI

    [Header("Movement")]
    public float moveSpeed = 3.5f;

    [HideInInspector]
    public bool zombieAktif = false;

    void OnEnable()
    {
        GameEvents.OnStartGame += ActivateZombie;
    }

    void OnDisable()
    {
        GameEvents.OnStartGame -= ActivateZombie;
    }

    private void ActivateZombie()
    {
        zombieAktif = true;

        if (agent != null)
        {
            agent.speed = moveSpeed;
        }

        Debug.Log(gameObject.name + " activated by start event! speed=" + moveSpeed);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // TAMBAH INI

        if (agent != null)
        {
            agent.speed = moveSpeed;
        }

        GameObject playerObjek = GameObject.FindGameObjectWithTag("Player");

        if (playerObjek != null)
        {
            player = playerObjek.transform;
        }
        else
        {
            Debug.LogError("Waduh! Objek dengan Tag 'Player' tidak ditemukan di Hierarchy!");
        }
    }

    void Update()
    {
        if (!zombieAktif)
        {
            if (agent != null && agent.isOnNavMesh)
                agent.isStopped = true;

            if (anim != null)
                anim.SetBool("isWalking", false);

            return;
        }

        if (agent != null && player != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            if (anim != null)
            {
                anim.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") ||
            collision.gameObject.name.Contains("Bullet") ||
            collision.gameObject.name.Contains("peluru"))
        {
            return;
        }
    }
}