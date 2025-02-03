using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PoolCueHandler : MonoBehaviour
{
    public CheckBallMovement checkBallMovement;
    [SerializeField] private XRGrabInteractable backGrip; // The part you will have to hold, back of the Pool Cue
    [SerializeField] private Transform middleHandTransform; // The left hand/controller
    [SerializeField] private float middleHandOffset = 0.5f; // The offset to determine where the middle hand should be placed along the Pool Cue
    [SerializeField] private GameObject whiteBall; // Reference to the white ball
    [SerializeField] private float extraForceMultiplier = 10f; // Adjust multiplier for more force
    [SerializeField] private float maxForce = 1000f; // Maximum force to apply to the white ball

    private XRBaseInteractor backHandInteractor; // The interactor for the back hand
    private bool isCueInUse = true; // Bool to track if the Pool Cue can be controlled by the player
    private Vector3 lastCuePosition; // The previous position of the Pool Cue to calculate movement
    private float cueSpeed; // The speed of the Pool Cue based on position changes

    void Start()
    {
        if (backGrip != null)
        {
            backGrip.selectEntered.AddListener(HandleBackHandGrab);
            backGrip.selectExited.AddListener(HandleBackHandRelease);
        }

        // Initialize lastCuePosition with the current position of the Pool Cue
        lastCuePosition = transform.position;
    }

    // This method handles when the right hand grabs the Pool Cue
    private void HandleBackHandGrab(SelectEnterEventArgs args)
    {
        backHandInteractor = args.interactorObject as XRBaseInteractor;
        isCueInUse = true; // Re-enable control when grabbed
    }

    // This method handles when the right hand releases the Pool Cue
    private void HandleBackHandRelease(SelectExitEventArgs args)
    {
        if (backHandInteractor == args.interactorObject as XRBaseInteractor)
        {
            backHandInteractor = null;
            isCueInUse = true; // Re-enable control if released
        }
    }

    void Update()
    {
        if (isCueInUse)
        {
            // If the back hand is grabbing, update the Pool Cue orientation to reflect the middle hand
            if (backHandInteractor != null)
            {
                UpdateCueOrientation();
            }
        }

        // Track the Pool Cue's speed by comparing positions
        float speed = (transform.position - lastCuePosition).magnitude / Time.deltaTime;
        cueSpeed = speed;
        lastCuePosition = transform.position;
    }

    // Update the Pool Cue's orientation to reflect the middle hand
    private void UpdateCueOrientation()
    {
        if (backHandInteractor != null && middleHandTransform != null)
        {
            // Get the positions of the back hand and middle hand
            Vector3 backHandPosition = backHandInteractor.transform.position;
            Vector3 middleHandPosition = middleHandTransform.position;

            // Calculate the direction vector from the back hand to the middle hand
            Vector3 directionToMiddleHand = (middleHandPosition - backHandPosition).normalized;

            // Set the rotation of the Pool Cue to make it point towards the middle hand
            Quaternion targetRotation = Quaternion.LookRotation(directionToMiddleHand, Vector3.up);
            transform.rotation = targetRotation;
        }
    }

    // Detect the Pool Cue hitting the white ball
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WhiteBall")) // Ensure this is the white ball
        {
            // Stop controlling the Pool Cue when it hits the white ball
            isCueInUse = false;

            // Apply force to the white ball based on the Pool Cue's speed and direction
            Rigidbody whiteBallRigidbody = whiteBall.GetComponent<Rigidbody>();
            if (whiteBallRigidbody != null)
            {
                // Calculate the direction of the force (same direction as the Pool Cue's movement)
                Vector3 forceDirection = (transform.position - lastCuePosition).normalized;

                // Apply the force to the white ball (use a stronger multiplier for more impact)
                Vector3 forceToApply = forceDirection * cueSpeed * extraForceMultiplier;

                // Clamp the force to the maximum value
                if (forceToApply.magnitude > maxForce)
                {
                    forceToApply = forceToApply.normalized * maxForce;
                }

                // Apply the clamped force to the white ball
                whiteBallRigidbody.AddForce(forceToApply, ForceMode.Impulse);

                if(forceToApply.magnitude >= 0.1f)
                {
                    // Disable the Pool Cue
                    gameObject.SetActive(false);
                }
                else
                {
                    checkBallMovement.FaultyShotStart();
                }
            }
        }
    }
}
