using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject bulletPrefab;      // Masukkan prefab peluru di sini
    public Transform spawnPoint;         // Tempat peluru muncul (ujung laras)

    [Header("Pengaturan Pistol")]
    public float bulletSpeed = 20f;      // Kecepatan peluru

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        // Otomatis mengambil komponen XR Grab Interactable di objek ini
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        
        // Daftarkan fungsi Shoot agar aktif saat trigger controller ditekan
        grabInteractable.activated.AddListener(Shoot);
    }

    void OnDestroy()
    {
        // Bersihkan listener saat objek dihancurkan agar tidak error
        if (grabInteractable != null)
        {
            grabInteractable.activated.RemoveListener(Shoot);
        }
    }

    // Fungsi untuk menembak
    public void Shoot(ActivateEventArgs args)
    {
        // 1. Buat/Spawn peluru di posisi dan rotasi spawnPoint
        GameObject spawnedBullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        // 2. Ambil komponen Rigidbody dari peluru untuk memberinya gaya/dorongan
        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            // Dorong peluru ke arah depan laras pistol
            rb.linearVelocity = spawnPoint.forward * bulletSpeed;
        }

        // 3. Hancurkan peluru otomatis setelah 3 detik agar game tidak lag
        Destroy(spawnedBullet, 3f);
    }
}