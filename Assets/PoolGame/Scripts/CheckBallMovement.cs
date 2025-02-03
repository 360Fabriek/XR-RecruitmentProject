using System.Collections;
using UnityEngine;

public class CheckBallMovement : MonoBehaviour
{
    public GameObject[] balls; // Array of all balls in the game
    [SerializeField]private GameObject poolCue; // The Pool Cue
    private Vector3 poolCueStartPosition; // Start position of the Pool Cue
    private bool allBallsStopped = false; // To check if balls stopped moving
    [SerializeField]private float stopThreshold = 0.1f; // The threshold to consider a ball as stopped
    private float checkInterval = 0.5f; // Time interval between each velocity check
    private float nextCheckTime = 0f;

    void Start()
    {
        poolCueStartPosition = poolCue.transform.position;
    }
    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            CheckBallsStopped();
            nextCheckTime = Time.time + checkInterval;
        }
    }

    void CheckBallsStopped()
    {
        bool allStopped = true;

        foreach (GameObject ball in balls)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();

            // Check if the ball's velocity is below the threshold
            if (rb.linearVelocity.magnitude > stopThreshold)
            {
                allStopped = false;
                break;
            }
        }

        if (allStopped && !allBallsStopped)
        {
            allBallsStopped = true;
            OnAllBallsStopped(); 
        }
        else if (!allStopped && allBallsStopped)
        {
            allBallsStopped = false;
        }
    }

    public void OnAllBallsStopped()
    {
        // Reset Pool Cue position and rotation
        poolCue.transform.position = poolCueStartPosition;
        poolCue.transform.rotation = Quaternion.identity;
        poolCue.SetActive(true);
    }

    private IEnumerator FaultyShot()
    {
        // If a shot is so soft, the Pool Cue could disappear and not turn on again. This prevents it and turns it on again after 2 seconds
        poolCue.SetActive(false);
        yield return new WaitForSeconds(2);
        OnAllBallsStopped();
    }

    public void FaultyShotStart()
    {
        StartCoroutine(FaultyShot());
    }
}
