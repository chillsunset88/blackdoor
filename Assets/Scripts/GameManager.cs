using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Pengaturan UI & Objek")]
    public GameObject uiStartGame;       // Seret Canvas Start Game ke sini
    public GameObject komputerPrimitive; // Seret objek Cube Komputer_Fisik ke sini

    [Header("Status Game")]
    private bool gameSudahMulai = false;
    private bool semuaZombieMati = false;

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

    void Update()
    {
        // Jika game belum mulai, atau jika semua zombie sudah terdeteksi mati, stop update
        if (!gameSudahMulai || semuaZombieMati) return;

        // Hitung jumlah zombie aktif bertag "Zombie" di map
        GameObject[] daftarZombie = GameObject.FindGameObjectsWithTag("Zombie");

        // Jika semua zombie sudah habis terbunuh
        if (daftarZombie.Length == 0)
        {
            MunculkanKomputerLevel2();
        }
    }

    void MunculkanKomputerLevel2()
    {
        semuaZombieMati = true;
        Debug.Log("Zombie Habis! Menyalakan komputer di luar ruangan.");

        // Otomatis memunculkan objek Cube beserta Canvas komputer di luar ruangan
        if (komputerPrimitive != null)
        {
            komputerPrimitive.SetActive(true);
        }
        // Notify that tutorial/first-level objectives are completed
        GameEvents.RaiseTutorialCompleted();
    }

    // FUNGSI 2: Dipanggil saat tombol di Komputer Primitive ditekan lewat laser VR
    public void PindahKeLevel2()
    {
        // Ganti nama di dalam tanda petik sesuai nama scene level 2 kamu yang asli
        SceneManager.LoadScene("Level2"); 
    }
}