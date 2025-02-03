using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int fullBallPoints = 0;
    public int stripeBallPoints = 0;

    [SerializeField] private TMP_Text scoreText;

    void Update()
    {
        scoreText.text = $"Full balls:{fullBallPoints}/7\nStripe balls:{stripeBallPoints}/7";
    }
}
