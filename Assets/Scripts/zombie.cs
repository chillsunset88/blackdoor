using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator anim; // Komponen pengontrol animasi
    
    [HideInInspector]
    public bool zombieAktif = false; 
    private bool sudahMati = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // Ambil komponen Animator di tubuh zombie

        GameObject playerObjek = GameObject.FindGameObjectWithTag("Player");
        if (playerObjek != null)
        {
            player = playerObjek.transform; 
        }
    }

    void Update()
    {
        if (sudahMati) return; // Jika sudah mati, stop semua logika di bawah

        // JIKA GAME BELUM MULAI / ZOMBIE DIAM
        if (!zombieAktif)
        {
            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
            
            // Beritahu Animator untuk memainkan animasi IDLE (isWalking = false)
            if (anim != null) anim.SetBool("isWalking", false);
            return;
        }

        // JIKA GAME SUDAH MULAI & ZOMBIE MENGEJAR PLAYER
        if (agent != null && player != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Beritahu Animator untuk memainkan animasi WALK (isWalking = true)
            if (anim != null) anim.SetBool("isWalking", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (sudahMati) return;

        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.name.Contains("Bullet"))
        {
            Mati(collision.gameObject);
        }
    }

    void Mati(GameObject peluru)
    {
        sudahMati = true;
        Debug.Log("Zombie tewas!");

        // 1. Jalankan animasi mati lewat parameter trigger 'die'
        if (anim != null) anim.SetTrigger("die");

        // 2. Matikan NavMeshAgent agar mayat zombie tidak ngejar player lagi secara gaib
        if (agent != null) agent.isStopped = true;

        Destroy(peluru); // Hancurkan peluru

        // 3. JANGAN langsung Destroy(gameObject). Kasih jeda 3 detik agar dosen bisa melihat animasi jatuh matinya selesai!
        Destroy(gameObject, 3f); 
    }
}