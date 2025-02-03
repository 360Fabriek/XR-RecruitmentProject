using UnityEngine;
using TMPro;

public class CheckPoints : MonoBehaviour
{
    public Score score;

    [SerializeField] private Transform whiteBall; // Cue Ball
    private Vector3 whiteBallStartPosition;
    [SerializeField] private GameObject endCanvas;
    [SerializeField]private TMP_Text endText;
    void Start()
    {
        whiteBallStartPosition = whiteBall.position;
    }


    // All the states on how a game can end
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "StripeBall")
        {
            score.stripeBallPoints++;
        }
        else if(other.tag == "FullBall")
        {
            score.fullBallPoints++;
        }
        else if(other.tag == "BlackBall")
        {
            if(score.stripeBallPoints >= 7 || score.fullBallPoints >= 7)
            {
                endCanvas.SetActive(true);
                endText.text = "Stripe ball won!";
            }
            else if(score.fullBallPoints >= 7)
            {
                endCanvas.SetActive(true);
                endText.text = "Full ball won!";
            }
            else
            {
                endCanvas.SetActive(true);
                endText.text = "Black ball went in early";
            }
        } else if(other.tag == "WhiteBall")
        {
            whiteBall.position = whiteBallStartPosition;
        }
    }
}
