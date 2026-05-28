using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk pindah level

public class SceneLoader : MonoBehaviour
{
    public void StartGame()
    {
        // Ganti "Level2" dengan nama Scene map laboratorium kamu yang sebenarnya
        SceneManager.LoadScene("level2"); 
    }
}