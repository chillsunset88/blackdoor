using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject bulletPrefab;      
    public Transform spawnPoint;         

    [Header("Pengaturan Pistol")]
    public float bulletSpeed = 20f;      
    
    [Header("Sistem Amunisi")]
    public int maxAmmo = 7;             // Kapasitas maksimal peluru dalam 1 magasin
    private int currentAmmo;            // Sisa peluru saat ini

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private AudioSource audioSource; 

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        
        // Mengisi peluru penuh saat game pertama kali dimulai
        currentAmmo = maxAmmo;

        grabInteractable.activated.AddListener(Shoot);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.activated.RemoveListener(Shoot);
        }
    }

    public void Shoot(ActivateEventArgs args)
    {
        // Jika peluru habis, pistol mogok tidak bisa menembak
        if (currentAmmo <= 0)
        {
            Debug.Log("Peluru Habis! Masukkan magasin ke socket untuk reload.");
            return;
        }

        // Jalankan tembakan jika peluru masih ada
        GameObject spawnedBullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.linearVelocity = spawnPoint.forward * bulletSpeed;
        }

        Destroy(spawnedBullet, 3f);

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Kurangi peluru setiap kali sukses menembak
        currentAmmo--;
        Debug.Log("Sisa Peluru: " + currentAmmo);
    }

    // ==========================================================
    // FUNGSI UTAMA UNTUK DI-ASSIGN KE SOCKET (MUST BE PUBLIC)
    // ==========================================================
    public void ReloadPistol(SelectEnterEventArgs args)
    {
        currentAmmo = maxAmmo;
        Debug.Log("Reload Berhasil! Peluru kembali penuh: " + currentAmmo);

        // Menghancurkan objek magasin yang masuk ke dalam socket agar terlihat realistis tertelan pistol
        if (args.interactableObject != null)
        {
            Destroy(args.interactableObject.transform.gameObject);
        }
    }
}