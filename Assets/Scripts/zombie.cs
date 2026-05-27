using UnityEngine;
using UnityEngine.AI; // Wajib dimasukkan untuk mengontrol NavMesh

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Mencari objek XR Origin/Player secara otomatis di dalam map menggunakan Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Jika pemain ditemukan, perintahkan NavMesh untuk terus berjalan ke posisi koordinat pemain
        if (playerTransform != null && agent != null)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    // Fungsi tambahan jika zombie tertembak oleh peluru pistol kamu
    private void OnCollisionEnter(Collision collision)
    {
        // Jika objek yang menabrak badan zombie memiliki tag "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Zombie Tertembak!");
            Destroy(collision.gameObject); // Hancurkan peluru
            Destroy(gameObject);           // Hancurkan zombie (mati)
        }
    }
}