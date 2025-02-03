using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private Transform cueBallSpawn, rackSpawn;
    [SerializeField]
    private float spacing = 0.052f; // 52mm ball size
    [SerializeField]
    private RackDirection rackDirection = RackDirection.Forward;

    [SerializeField]
    private List<GameObject> spawnedBalls = new List<GameObject>();
    [SerializeField]
    private int[] ballOrder = { 1, 9, 2, 10, 3, 8, 3, 11, 7, 14, 4, 5, 13, 15, 12 }; // Standard 8-ball rack order

    [SerializeField]
    private List<Material> materials = new List<Material>(16);

    [SerializeField]
    private GameObject cueBall;

    [SerializeField]
    private Transform ballParent;

    private void Start()
    {
        InitTable();
    }

    public void InitTable()
    {
        ClearBalls();
        SpawnBalls();
    }

    private Vector3 GetRackDirection()
    {
        switch (rackDirection)
        {
            case RackDirection.Forward:
                return Vector3.forward;
            case RackDirection.Backward:
                return Vector3.back;
            case RackDirection.Left:
                return Vector3.left;
            case RackDirection.Right:
                return Vector3.right;
            default:
                return Vector3.forward;
        }
    }

    public void SpawnBalls()
    {
        Vector3 direction = GetRackDirection(); // Get rack orientation
        Vector3 startPos = rackSpawn.position;

        // Cue Ball
        SpawnCueBall();

        //Spawn the balls in a rack formation 5 rows
        //Row 1: 1 ball
        //Row 2: 2 balls
        //Row 3: 3 balls
        //Row 4: 4 balls
        //Row 5: 5 balls
        //Result should be a triangle formation of balls with the 8-ball in the center
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Vector3 position = startPos + direction * i * spacing + Vector3.right * (j - i / 2f) * spacing;
                int ballIndex = i * (i + 1) / 2 + j;
                SpawnBall(ballIndex, position);
            }
        }
    }

    private void SpawnCueBall()
    {
        cueBall = Instantiate(ballPrefab, cueBallSpawn.position, Quaternion.identity);
        if (ballParent != null)
            cueBall.transform.SetParent(ballParent);

        Ball cueBallScript = cueBall.GetComponent<Ball>();
        cueBallScript.ballType = BallType.Cue;
        cueBallScript.ballNumber = 0;
        ForceApplier forceApplier = cueBall.AddComponent<ForceApplier>();
        forceApplier.CueBall = cueBallScript;
        cueBall.name = "Ball_CueBall";
        cueBall.tag = "CueBall";
        SetMaterialForBall(cueBallScript);
        spawnedBalls.Add(cueBall);
    }

    public void ResetCueBall()
    {
        cueBall.transform.position = cueBallSpawn.position;
        cueBall.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        cueBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void SpawnBall(int ballIndex, Vector3 position)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        if (ballParent != null)
            ball.transform.SetParent(ballParent);
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.ballNumber = ballOrder[ballIndex];
        ballScript.ballType = ballOrder[ballIndex] == 8 ? BallType.Eight : ballOrder[ballIndex] % 2 == 0 ? BallType.Solid : BallType.Striped;
        ball.name = "Ball_" + ballScript.ballNumber;
        SetMaterialForBall(ballScript);
        spawnedBalls.Add(ball);
    }

    private void SetMaterialForBall(Ball ball)
    {
        MeshRenderer meshRenderer = ball.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material material = materials[(int)ball.ballNumber];
            meshRenderer.material = material;
        }
    }

    public void ClearBalls()
    {
        foreach (GameObject ball in spawnedBalls)
        {
            Destroy(ball);
        }
        spawnedBalls.Clear();
    }
}

public enum RackDirection
{
    Forward,
    Backward,
    Left,
    Right
}
