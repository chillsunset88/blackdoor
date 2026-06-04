using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log("Zombie terkena damage! Damage: " + amount + " | Sisa HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Zombie Mati!");

        // Hentikan gerakan dan collision agar zombie tidak terus bergerak/menerima damage.
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
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

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("die", true);
            anim.SetBool("isDead", true);
        }

        Destroy(gameObject, 2f);
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
