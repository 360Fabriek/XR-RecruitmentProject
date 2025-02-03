using UnityEngine;
using UnityEngine.Events;

public enum BallType { Cue, Solid, Striped, Eight }

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public BallType ballType = BallType.Solid; // Solid, Striped, Cue, or Eight
    public int ballNumber;    // 1-15 (0 for cue ball)
    //Public Unity Event Delegate to call when the ball is pocketed or stopped moving
    public UnityEvent OnBallPocketed;
    public UnityEvent OnBallStartMoving;
    public UnityEvent OnBallStoppedMoving;

    [SerializeField]
    private float ballMass = 0.16f;

    [SerializeField]
    private float currentVelocity;

    [SerializeField]
    private float previousFrameVelocity;

    [SerializeField]
    public bool isMoving;

    [SerializeField]
    Vector3 startPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //null check
        if (rb != null)
        {
            rb.mass = ballMass;
        }

        OnBallPocketed.AddListener(PocketBall);
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        currentVelocity = rb.linearVelocity != Vector3.zero ? rb.linearVelocity.magnitude : 0;

        if (currentVelocity > 0)
        {
            if (currentVelocity > previousFrameVelocity)
            {
                previousFrameVelocity = currentVelocity;
                HandleStartMoving();
            }

            //slow the ball down gradually
            rb.linearVelocity = rb.linearVelocity * 0.99f;

            if (rb.linearVelocity.magnitude < 0.05f)
            {

                HandleStopMoving();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                previousFrameVelocity = 0;
            }
        }
    }

    private void Update()
    {
        if(transform.position.y < startPosition.y - 0.4f)
        {
            transform.position = startPosition;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void HandleStopMoving()
    {
        if (isMoving == true)
        {
            isMoving = false;
            OnBallStoppedMoving.Invoke();
        }
    }

    private void HandleStartMoving()
    {
        if (isMoving == false)
        {
            isMoving = true;
            OnBallStartMoving.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pocket"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket"))
        {
            OnBallPocketed.Invoke();
        }
    }

    private void PocketBall()
    {
        PoolGameMode poolGameMode = GameMode.Instance as PoolGameMode;
        poolGameMode.PotBall(this);

        if(ballType != BallType.Cue)
            gameObject.SetActive(false);
    }
}
