using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Zombie terkena tebasan! Sisa HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Zombie Mati!");
        // Di sini kamu bisa memasang animasi mati atau menghancurkan objeknya
        Destroy(gameObject); 
    }
}