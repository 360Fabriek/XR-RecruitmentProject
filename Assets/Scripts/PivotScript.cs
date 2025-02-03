using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PivotScript : MonoBehaviour
{
    [SerializeField]
    XRGrabInteractable cueobject;
    [SerializeField]
    Transform cue;
    [SerializeField]
    float offset = 270;
    [SerializeField]
    bool isInAimMode = true;
    [SerializeField]
    Vector3 previousControllerPosition;
    [SerializeField]
    public Vector3 localCuePosition;

    public float movementSpeed;

    [SerializeField]
    Transform tableRefrence;
    [SerializeField]
    float talbeHeightOffset = 0.05f,tableHeightOffsetToBreak = 0.20f;

    [SerializeField]
    Transform cueTip;

    [SerializeField]
    bool enteredShootMode = false;

    [SerializeField]
    bool isInWaitMode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousControllerPosition = cueobject.attachTransform.position;
        localCuePosition = cue.localPosition;

        if (cueTip == null)
            cueTip = GameObject.FindWithTag("CueTip").transform;

        //Disable all meshrenderers in children
        ToggleMeshes(false);
    }

    public void ToggleCollisionForCue(bool newStatus)
    {
        Rigidbody[] rigidbodies = cueobject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb.isKinematic == !newStatus)
                rb.isKinematic = newStatus;
        }

        Collider[] colliders = cueobject.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            if (col.enabled == !newStatus)
                col.enabled = newStatus;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInWaitMode)
        {
            if (cueTip.position.y > tableRefrence.position.y + talbeHeightOffset)
            {
                isInAimMode = true;
            }
            else
            {
                isInAimMode = false;
            }

            if (cueobject.isSelected)
            {
                ToggleCollisionForCue(false);
                
            }
            else
            {
                ToggleCollisionForCue(true);
            }

            if (isInAimMode)
            {
                ToggleMeshes(false);

                if (cue.localPosition != localCuePosition)
                    cue.localPosition = localCuePosition;
                RotateTowardsObject();


            }
            else
            {
                ToggleMeshes(true);

                if (enteredShootMode == false)
                {
                    cue.localPosition = localCuePosition;
                    enteredShootMode = true;

                }
                else
                {
                    ShootMode();

                    if (cueTip.position.y > tableRefrence.position.y + tableHeightOffsetToBreak)
                    {
                        enteredShootMode = false;
                        isInAimMode = true;

                        //Renable mesh
                        ToggleMeshes(false);
                    }
                }

            }
        }
        else
        {
            //When in wait mode we need to move the cue back up to reable aim mode
            if (cueTip.position.y > tableRefrence.position.y + talbeHeightOffset)
            {
                isInWaitMode = false;
            }

            if (!cueobject.isSelected)
            {
                ToggleCollisionForCue(true);
                ToggleMeshes(false);
                isInWaitMode = false;
                isInAimMode = false;
                enteredShootMode = false;
            }
        }
    }

    private void ToggleMeshes(bool value)
    {
        //Enable all meshrenderers in children
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = value;
        }
    }

    public void HasFired()
    {
        ToggleMeshes(false);
        enteredShootMode = false;
        isInAimMode = false;
        isInWaitMode = true;
        cue.localPosition = localCuePosition;
    }

    void RotateTowardsObject()
    {
        if (cueobject != null)
        {
            //Rotate towards the object on the Y axis
            transform.LookAt(cueobject.attachTransform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + offset, 0);
        }
    }

    public void ShootMode()
    {
        if (cueobject.attachTransform != null)
        {
            // Calculate movement delta
            Vector3 controllerMovement = cueobject.attachTransform.position - previousControllerPosition;

            // Project movement onto cue's forward vector
            float forwardMovement = Vector3.Dot(controllerMovement, cue.forward); // Gets signed movement amount

            //Calculate movement speed
            movementSpeed = Mathf.Abs(forwardMovement) / Time.deltaTime;

            // Move cue along its forward vector
            cue.position += cue.forward * forwardMovement;

            // Store previous position for next frame
            previousControllerPosition = cueobject.attachTransform.position;
        }
    }
}
