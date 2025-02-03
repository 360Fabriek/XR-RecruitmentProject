using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    // Singleton instance
    private static GameMode _instance;

    public static GameMode Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameMode>();

                if (_instance == null)
                {
                    // Dynamically create a new GameObject with the correct type
                    GameObject go = new GameObject(typeof(GameMode).Name);
                    _instance = go.AddComponent<GameMode>();
                }
            }
            return _instance;
        }
    }

    // Called when the game mode starts
    public abstract void OnModeStarted();

    // Called when the game mode ends
    public abstract void OnModeEnded();
}
