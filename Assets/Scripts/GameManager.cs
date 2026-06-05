using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Pengaturan UI & Objek")]
    public GameObject uiStartGame;        // Seret UI Start Menu ke sini (tampilan sebelum game mulai)
    public GameObject mainmenuAI;         // Seret Main Menu panel ke sini (tampilan setelah semua zombie mati)

    [Header("Status Game")]
    private bool gameSudahMulai = false;
    private bool semuaZombieMati = false;
    private int zombieCount = 0;

    void OnEnable()
    {
        GameEvents.OnZombieDied += HandleZombieDeath;
    }

    void OnDisable()
    {
        GameEvents.OnZombieDied -= HandleZombieDeath;
    }

    void Start()
    {
        // Ensure the end-level menu is hidden until all zombies are killed.
        if (mainmenuAI != null)
        {
            mainmenuAI.SetActive(false);
        }

        zombieCount = CountZombies();
        UnityEngine.Debug.Log("Initial zombie count: " + zombieCount);
    }

    // FUNGSI 1: Dipanggil saat tombol START GAME ditekan
    public void TekanStartGame()
    {
        gameSudahMulai = true;

        // 1. Sembunyikan papan UI Start Game agar tidak menghalangi ruangan
        if (uiStartGame != null) uiStartGame.SetActive(false);

        // 2. Broadcast start event so interested systems (zombies, UI) can react
        UnityEngine.Debug.Log("Game Dimulai! Broadcasting start event.");
        GameEvents.RaiseStartGame();
    }

    private int CountZombies()
    {
        return GameObject.FindGameObjectsWithTag("Zombie").Length;
    }

    private void HandleZombieDeath()
    {
        if (!gameSudahMulai || semuaZombieMati) return;

        zombieCount = Mathf.Max(0, zombieCount - 1);
        UnityEngine.Debug.Log("Zombie died. Remaining count: " + zombieCount);

        if (zombieCount == 0)
        {
            ShowMainMenuLevel();
        }
    }

    void ShowMainMenuLevel()
    {
        semuaZombieMati = true;
        UnityEngine.Debug.Log("All zombies are dead! Showing main menu UI for next transition.");

        if (mainmenuAI != null)
        {
            mainmenuAI.SetActive(true);
        }
        GameEvents.RaiseTutorialCompleted();
    }

    // FUNGSI 2: Dipanggil saat tombol di Main Menu ditekan untuk pindah level
    public void PindahKeLevel2()
    {
        // Hide main menu before transitioning
        if (mainmenuAI != null)
        {
            mainmenuAI.SetActive(false);
        }
        // Ganti nama di dalam tanda petik sesuai nama scene level 2 kamu yang asli
        SceneManager.LoadScene("Level2"); 
    }
}