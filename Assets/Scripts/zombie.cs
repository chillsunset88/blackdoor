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
        // Pengecekan 1: Deteksi berdasarkan TAG peluru
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Mati(collision.gameObject);
            return;
        }

        // Pengecekan 2: Jalur alternatif jika pelurumu menggunakan nama objek "Bullet"
        if (collision.gameObject.name.Contains("Bullet") || collision.gameObject.name.Contains("peluru"))
        {
            Mati(collision.gameObject);
        }
    }

    void Mati(GameObject peluru)
    {
        Debug.Log("Zombie tewas terkena peluru!");
        Destroy(peluru); // Hancurkan peluru
        Destroy(gameObject); // Hancurkan zombie ini
    }
}