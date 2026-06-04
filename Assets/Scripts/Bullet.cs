using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Pengaturan Peluru")]
    public float damageAmount = 25f; // Damage yang diberikan per tembakan
    private bool hasHit = false; // Cegah damage multiple times

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Bullet OnTriggerEnter: " + collision.name + " tag=" + collision.tag);
        HandleCollision(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet OnCollisionEnter: " + collision.gameObject.name + " tag=" + collision.gameObject.tag);
        HandleCollision(collision.collider);
    }

    private void HandleCollision(Collider collision)
    {
        // Jangan tembak diri sendiri
        if (hasHit) return;

        // Cek apakah mengenai zombie.
        // Gunakan GetComponentInParent agar peluru tetap bisa mengenai child collider pada zombie.
        ZombieHealth zombieHealth = collision.GetComponentInParent<ZombieHealth>();
        if (zombieHealth != null)
        {
            hasHit = true;
            Debug.Log("Peluru mengenai Zombie! Damage: " + damageAmount);
            zombieHealth.TakeDamage(damageAmount);
            Destroy(gameObject); // Hancurkan peluru setelah kena zombie
            return;
        }

        // Cek apakah mengenai dinding atau objek lain (selain zombie)
        if (collision.CompareTag("Player") || collision.CompareTag("MainCamera"))
        {
            return; // Jangan hancurkan jika mengenai player
        }

        // Hancurkan peluru jika mengenai objek lain
        hasHit = true;
        Debug.Log("Peluru mengenai: " + collision.name);
        Destroy(gameObject);
    }
}

