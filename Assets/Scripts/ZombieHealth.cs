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

        // Notify the game manager that one zombie has died.
        GameEvents.RaiseZombieDied();

        // Hentikan gerakan dan collision agar zombie tidak terus bergerak/menerima damage.
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            // Only stop the agent if it's placed on a NavMesh to avoid errors.
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
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
            // Set animation parameters only if they exist to avoid warnings.
            if (HasAnimatorParameter(anim, "die")) anim.SetBool("die", true);
            if (HasAnimatorParameter(anim, "isDead")) anim.SetBool("isDead", true);
        }

        Destroy(gameObject, 2f);
    }

    private bool HasAnimatorParameter(Animator animator, string paramName)
    {
        foreach (var p in animator.parameters)
        {
            if (p.name == paramName) return true;
        }
        return false;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
