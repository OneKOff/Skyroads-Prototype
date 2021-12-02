using UnityEngine;
using TMPro;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI asteroidsPassedText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreMessage;

    private bool _newHighScore = false;


    private void Start()
    {
        // Event subscriptions
        Player.Instance.OnGameOver += InitializeWindow;
        GameController.Instance.OnHighScoreChanged += (i) => { _newHighScore = true; };
        GameController.Instance.OnLevelReset += ResetHighScoreFlag;

        gameObject.SetActive(false);
    }


    // OnGameOver event
    private void InitializeWindow()
    {
        // If new high score has been reached, show the corresponding message
        highScoreMessage.gameObject.SetActive(_newHighScore);
        // Get final player stats
        timeText.text = "Total Time: " + (int)GameController.Instance.TimePlayed + " Sec";
        asteroidsPassedText.text = "Asteroids Passed: " + GameController.Instance.AsteroidsPassed;
        scoreText.text = "Final Score: " + (int)GameController.Instance.Score + " Points";
    }
    // OnLevelReset event
    private void ResetHighScoreFlag()
    {
        _newHighScore = false;
    }
}
