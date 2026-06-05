using UnityEngine;

// Attach this to the XR Ray Interactor (or any controller) and wire its Activate event
// to call the Activate() method. The script will raycast forward and call Activate()
// on any UIScreenActivator hit.
public class XRActivateOnRay : MonoBehaviour
{
    public Transform rayOrigin; // usually the controller or interactor transform
    public float maxDistance = 10f;
    public LayerMask interactableMask = ~0; // default everything

    public void Activate()
    {
        if (rayOrigin == null) rayOrigin = this.transform;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableMask))
        {
            var screen = hit.collider.GetComponent<UIScreenActivator>();
            if (screen != null)
            {
                screen.Activate();
                return;
            }

            // fallback: try MonitorActivator
            var monitor = hit.collider.GetComponent<MonitorActivator>();
            if (monitor != null)
            {
                monitor.Activate();
                return;
            }
        }
    }
}
