using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Pengaturan Peluru")]
    public float damageAmount = 25f; // Damage yang diberikan per tembakan
    private bool hasHit = false; // Cegah damage multiple times
    public float lifeTime = 3f; // waktu sebelum peluru dikembalikan ke pool
    private float lifeTimer = 0f;

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
            ReturnToPool(); // kembalikan peluru ke pool setelah kena zombie
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
        ReturnToPool();
    }

    private void OnEnable()
    {
        hasHit = false;
        lifeTimer = 0f;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // stop any physics motion
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        BulletPool.Instance.ReturnToPool(gameObject);
    }
}

