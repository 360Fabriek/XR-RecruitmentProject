using UnityEngine;
using UnityEngine.Events;

public class PoolGameMode : GameMode
{
    public GameInitializer gameInitializer;
    public int SolidBallsPotted { get; private set; }
    public int StripedBallsPotted { get; private set; }

    [SerializeField]
    UnityEvent<string> OnSolidBallsPottedChanged;
    [SerializeField]
    UnityEvent<string> OnStripedBallsPottedChanged;

    public override void OnModeStarted()
    {
       
        if (gameInitializer != null)
        {
            gameInitializer.SpawnBalls();
            StripedBallsPotted = 0;
            SolidBallsPotted = 0;

            OnSolidBallsPottedChanged.Invoke(SolidBallsPotted.ToString());
            OnStripedBallsPottedChanged.Invoke(StripedBallsPotted.ToString());

            Debug.Log("Pool game mode started!");
        }
        else
        {
            Debug.LogError("GameInitializer not assigned!");
        }
    }

    public void PotBall(Ball ball)
    {
        switch (ball.ballType)
        {
            case BallType.Solid:
                SolidBallsPotted++;
                OnBallPocketed();
                OnSolidBallsPottedChanged.Invoke(SolidBallsPotted.ToString());
                break;
            case BallType.Striped:
                StripedBallsPotted++;
                OnBallPocketed();
                OnStripedBallsPottedChanged.Invoke(StripedBallsPotted.ToString());
                break;
            case BallType.Eight:
                //End the game
                case BallType.Cue:
                gameInitializer.ResetCueBall();
                break;
        }
    }

    private void OnBallPocketed()
    {
        if (SolidBallsPotted == 7 || StripedBallsPotted == 7)
        {
            Debug.Log("Game Over!");
            OnModeEnded();
        }
    }

    public override void OnModeEnded()
    {
        gameInitializer.ClearBalls();
        //Restart the game
        OnModeStarted();
    }
}