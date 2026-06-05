using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    
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
        Debug.Log(gameObject.name + " activated by start event!");
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // PERBAIKAN: Cara mencari dan mengikat komponen transform Player yang benar
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
        // Jika belum ditekan START GAME, diam di tempat
        if (!zombieAktif)
        {
            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
            return;
        }

        // Jika sudah aktif, kejar player
        if (agent != null && player != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    // (Fungsi OnCollisionEnter ada di bawah di bagian Perbaikan 2)


    // Sambungan script ZombieAI di atas...
    private void OnCollisionEnter(Collision collision)
    {
        // Previously this method destroyed the zombie directly when a bullet collided.
        // Damage and death are now handled by `ZombieHealth`, so ignore bullet collisions here
        // to avoid double-destroy and duplicate logs.
        if (collision.gameObject.CompareTag("Bullet") ||
            collision.gameObject.name.Contains("Bullet") ||
            collision.gameObject.name.Contains("peluru"))
        {
            // Let Bullet.cs and ZombieHealth handle the damage and destruction.
            return;
        }

        // Other collision handling (e.g., player contact) can go here.
    }
}