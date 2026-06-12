using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Pistol : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject bulletPrefab;
    public Transform spawnPoint;

    [Header("Pengaturan Pistol")]
    public float bulletSpeed = 20f;

    [Header("Sistem Amunisi")]
    public int maxAmmo = 7;
    private int currentAmmo;

    [Header("UI")]
    public TMP_Text ammoText;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private AudioSource audioSource;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        currentAmmo = maxAmmo;

        UpdateAmmoUI();

        grabInteractable.activated.AddListener(Shoot);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.activated.RemoveListener(Shoot);
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }

    public void Shoot(ActivateEventArgs args)
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Peluru Habis! Masukkan magasin ke socket untuk reload.");
            return;
        }

        GameObject spawnedBullet = BulletPool.Instance.Get(
            bulletPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();

        if (bulletScript == null)
        {
            Debug.LogWarning("Spawned bullet prefab does not contain Bullet.cs: " + spawnedBullet.name);
        }
        else
        {
            Debug.Log("Spawned bullet has Bullet script: " + spawnedBullet.name);
        }

        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = spawnPoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Spawned bullet prefab does not have Rigidbody: " + spawnedBullet.name);
        }

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        currentAmmo--;

        UpdateAmmoUI();

        Debug.Log("Sisa Peluru: " + currentAmmo);
    }

    public void ReloadPistol(SelectEnterEventArgs args)
    {
        currentAmmo = maxAmmo;

        UpdateAmmoUI();

        Debug.Log("Reload Berhasil! Peluru kembali penuh: " + currentAmmo);

        if (args.interactableObject != null)
        {
            Destroy(args.interactableObject.transform.gameObject);
        }
    }
}