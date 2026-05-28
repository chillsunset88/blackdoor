using UnityEngine;
using UnityEngine.AI; // Jika menggunakan NavMesh

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    
    // Variabel pengontrol apakah zombie boleh jalan atau tidak
    [HideInInspector]
    public bool zombieAktif = false; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // JIKA ZOMBIE BELUM AKTIF, JANGAN JALAN (BERHENTI DI SINI)
        if (!zombieAktif)
        {
            if(agent != null) agent.isStopped = true;
            return;
        }

        // Jika sudah aktif, zombie berjalan mengejar player
        if (agent != null && player != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }
}