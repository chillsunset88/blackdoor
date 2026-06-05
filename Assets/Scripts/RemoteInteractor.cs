using UnityEngine;

// Simple helper for remote objects: call TryActivate with the target monitor GameObject
// e.g. from an XR interactor's Select/Activate UnityEvent pass the selected object.
public class RemoteInteractor : MonoBehaviour
{
    public string monitorTag = "Monitor"; // optional: use tags to identify monitor

    public void TryActivate(GameObject target)
    {
        if (target == null) return;
        // Support both legacy MonitorActivator and the new UIScreenActivator
        var screen = target.GetComponent<UIScreenActivator>();
        if (screen != null)
        {
            screen.Activate();
            return;
        }

        var monitor = target.GetComponent<MonitorActivator>();
        if (monitor != null)
        {
            monitor.Activate();
            return;
        }
    }
}
