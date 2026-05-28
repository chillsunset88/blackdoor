using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Pengaturan UI")]
    // Masukkan Canvas LabUI_Panel kamu ke sini nanti di Inspector
    public GameObject labUIPanel; 

    [Header("Status Game")]
    private int jumlahZombieTersisa;
    private bool semuaZombieMati = false;

    void Update()
    {
        // Jika semua zombie sudah terdeteksi mati, stop mengecek agar game tidak berat
        if (semuaZombieMati) return;

        // Mencari semua objek di dalam map yang memiliki Tag "Zombie"
        GameObject[] daftarZombie = GameObject.FindGameObjectsWithTag("Zombie");
        jumlahZombieTersisa = daftarZombie.Length;

        // Jika jumlah objek bertag Zombie sudah habis/nol
        if (jumlahZombieTersisa == 0)
        {
            MunculkanPromptLevel2();
        }
    }

    void MunculkanPromptLevel2()
    {
        semuaZombieMati = true;
        Debug.Log("Selamat! Semua zombie mati. Menampilkan UI Level 2.");

        // Menyalakan kembali secara otomatis papan UI yang tadi kita sembunyikan
        if (labUIPanel != null)
        {
            labUIPanel.SetActive(true); 
        }
    }
}