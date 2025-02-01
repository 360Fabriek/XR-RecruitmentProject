using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PoolCueHandler : MonoBehaviour
{
    public XRGrabInteractable backGrip; // The part you will have to hold, back of the cue pole
    public Transform middleHandTransform; // The transform of the other hand
    public float middleHandOffset = 0.5f; // The offset to determine where the middle hand should be placed along the cue

    public Vector3 positionOffset = Vector3.zero; // Editable position offset for fine-tuning

    private XRBaseInteractor backHandInteractor; // The interactor for the back hand

    void Start()
    {
        // Listen for grab and release events on the back grip
        if (backGrip != null)
        {
            backGrip.selectEntered.AddListener(HandleBackHandGrab);
            backGrip.selectExited.AddListener(HandleBackHandRelease);
        }
    }

    void Update()
    {
        // If the back hand is grabbing, update the cue pole orientation and position to reflect the middle hand
        if (backHandInteractor != null)
        {
            UpdateCueOrientationAndPosition();
        }
    }

    // This method handles when the back hand grabs the cue
    private void HandleBackHandGrab(SelectEnterEventArgs args)
    {
        backHandInteractor = args.interactorObject as XRBaseInteractor;
    }

    // This method handles when the back hand releases the cue
    private void HandleBackHandRelease(SelectExitEventArgs args)
    {
        if (backHandInteractor == args.interactorObject as XRBaseInteractor)
        {
            backHandInteractor = null;
        }
    }

    // Update the cue's orientation and position to reflect the middle hand
    private void UpdateCueOrientationAndPosition()
    {
        if (backHandInteractor != null && middleHandTransform != null)
        {
            // Get the positions of the back hand and middle hand
            Vector3 backHandPosition = backHandInteractor.transform.position;
            Vector3 middleHandPosition = middleHandTransform.position;

            // Calculate the direction vector from the back hand to the middle hand
            Vector3 directionToMiddleHand = (middleHandPosition - backHandPosition).normalized;

            // Set the rotation of the cue to make it point towards the middle hand
            Quaternion rotation = Quaternion.LookRotation(directionToMiddleHand, Vector3.up);
            transform.rotation = rotation;

            // Calculate the correct position of the cue
            Vector3 newPosition = backHandPosition - directionToMiddleHand * middleHandOffset;

            // Apply the position offset
            transform.position = newPosition + positionOffset;
        }
    }
}
