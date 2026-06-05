using UnityEngine;

// Attach this to the monitor object that appears after tutorial completion.
// The monitor can be activated either by an XR interaction calling Activate(),
// or by physically pressing a remote object (tagged Remote) into its trigger.
public class MonitorActivator : MonoBehaviour
{
    public GameManager gameManager; // assign in inspector (optional)
    public string remoteTag = "Remote";

    public void Activate()
    {
        Debug.Log("MonitorActivator: Activated");
        if (gameManager != null)
        {
            gameManager.PindahKeLevel2();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(remoteTag))
        {
            Activate();
        }
    }
}
