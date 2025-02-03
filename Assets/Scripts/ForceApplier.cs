using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    //Public enum to select the Vector3 direction of the force
    [SerializeField]
    private Transform pivotPoint = null;

    [SerializeField]
    private Rigidbody rb;

    public Ball CueBall;
    public float Force = 1.5f;
    public float factor
    {
        get
        {
            return Force / 15f;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the Rigidbody component of the GameObject

        rb = GetComponent<Rigidbody>();

        pivotPoint = GameObject.FindWithTag("PivotPoint").transform;

        if (pivotPoint == null)
        {
            //Error message if the pivot point is not found
            Debug.LogError("Pivot Point not found");
        }

        pivotPoint.position = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        pivotPoint.position = transform.position;

        //Draw a line from the pivot point to the direction of the force
        Debug.DrawRay(pivotPoint.position + new Vector3(0, .1f, 0), -pivotPoint.right * 2, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        // This will be called when the object with a collider enters a trigger zone
        Debug.Log("Triggered by: " + other.name);
        PivotScript pivotScript = other.gameObject.GetComponentInParent<PivotScript>();
        if (pivotScript)
        {
            Debug.Log("Speed: " + pivotScript.movementSpeed);
            float strength = (pivotScript.movementSpeed * factor);
            Debug.Log("Shot Strength: " + strength.ToString());
            rb.AddForce(-pivotPoint.right * strength, ForceMode.Impulse);

            pivotScript.HasFired();

        }
    }

}
