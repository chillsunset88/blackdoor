using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    private Animator anim;

    public int health = 3;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (!isDead && playerTransform != null && agent != null)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("KENA SESUATU");

    if (other.CompareTag("Bullet"))
    {
        Debug.Log("Zombie Tertembak!");

        health--;

        Destroy(other.gameObject);

        if (health <= 0)
        {
            Die();
        }
    }
}

    void Die()
    {
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }

        if (anim != null)
        {
            anim.Play("Z_FallingBack");
        }

        Destroy(gameObject, 3f);
    }
}