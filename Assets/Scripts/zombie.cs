using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform playerTarget; // Drag objek Main Camera / Player kamu ke sini di Inspector
    public string playerTag = "MainCamera"; // Alternatif jika target tidak di-drag, akan dicari lewat Tag

    [Header("Zombie Settings")]
    public float attackDistance = 1.5f; // Jarak minimal zombie untuk mulai menyerang player
    public float stoppingDistance = 0.5f; // Jarak berhenti dari player
    public float walkSpeed = 3.5f; // Kecepatan berjalan zombie
    public bool zombieAktif = false; // Aktifkan zombie saat game dimulai

    // Komponen internal
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        // Mengambil komponen yang ada di tubuh zombie
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Validasi NavMeshAgent
        if (agent == null)
        {
            Debug.LogError("ZombieAI: NavMeshAgent tidak ditemukan! Tambahkan NavMeshAgent component ke Zombie gameobject.");
        }
        else
        {
            // Atur kecepatan berjalan zombie
            agent.speed = walkSpeed;
        }

        if (anim == null)
        {
            Debug.LogWarning("ZombieAI: Animator tidak ditemukan pada Zombie!");
        }

        // Jika playerTarget kosong di Inspector, cari otomatis berdasarkan Tag
        if (playerTarget == null)
        {
            GameObject playerObj = GameObject.FindWithTag(playerTag);
            if (playerObj != null)
            {
                playerTarget = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("ZombieAI: Objek dengan Tag '" + playerTag + "' tidak ditemukan di Scene!");
            }
        }
    }

    void Update()
    {
        // Hanya jalankan zombie jika sudah diaktifkan oleh GameManager
        if (!zombieAktif)
        {
            StopZombie();
            return;
        }

        // Jika NavMeshAgent tidak ada, skip
        if (agent == null)
        {
            return;
        }

        // Jika tidak ada target player, zombie diam
        if (playerTarget == null) 
        {
            StopZombie();
            return;
        }

        // Hitung jarak antara zombie dan player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= attackDistance)
        {
            // KONDISI 1: Jarak dekat -> Berhenti jalan dan serang player
            StopZombie();
            TriggerAttack();
        }
        else
        {
            // KONDISI 2: Jarak jauh -> Kejar player
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            Debug.LogWarning("ZombieAI: NavMeshAgent tidak siap untuk bergerak!");
            return;
        }

        // Perintahkan NavMesh untuk berjalan ke posisi player
        agent.isStopped = false;
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(playerTarget.position);

        // Atur parameter animasi
        if (anim != null)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }

        Debug.Log("Zombie mengejar player. Jarak: " + Vector3.Distance(transform.position, playerTarget.position));
    }

    void StopZombie()
    {
        if (agent == null || !agent.enabled) return;

        // Hentikan pergerakan NavMesh
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // Matikan animasi jalan
        if (anim != null)
        {
            anim.SetBool("isWalking", false);
        }
    }

    void TriggerAttack()
    {
        // Nyalakan animasi menyerang
        if (anim != null)
        {
            anim.SetBool("isAttacking", true);
        }

        Debug.Log("Zombie menyerang!");
    }
}
