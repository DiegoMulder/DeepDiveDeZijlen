using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapToHandAtStart : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;  // The object to grab
    public ActionBasedController controller;      // The controller or input device used for grabbing

    private XRBaseInteractor currentInteractor;   // The interactor that grabbed the object

    private void Start()
    {
        if (grabInteractable != null)
        {
            // Subscribe to the selectEntered event (this is triggered when the object is grabbed)
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
        else
        {
            Debug.LogError("XRGrabInteractable is not assigned!");
        }
    }

    // This method will be triggered when the object is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactor; // Store the interactor that grabbed the object

        // Disable the grab ability for this object once it's picked up
        DisableGrabAbility();
    }

    // Disable the grab ability by making the object non-interactable
    private void DisableGrabAbility()
    {
        if (grabInteractable != null && currentInteractor != null)
        {
            // Disable interaction with the object for this specific interactor
            grabInteractable.selectEntered.RemoveListener(OnGrab);

            // Disable the interaction layer for this specific interactor (so it can't be grabbed again)
            grabInteractable.interactionLayers = InteractionLayerMask.GetMask();

            // Optionally, disable the ability to release the object by locking the Rigidbody
            Rigidbody rb = grabInteractable.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;  // Prevent physics from affecting it once grabbed
            }

            Debug.Log("Grab ability disabled after pickup!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the selectEntered event when the object is destroyed
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}
