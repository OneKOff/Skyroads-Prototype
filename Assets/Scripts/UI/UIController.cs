using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI asteroidsPassedText;
    [Header("Pop-up Windows")]
    [SerializeField] private GameObject gameStartWindow;
    [SerializeField] private GameObject gameOverWindow;

   
    private void Start()
    {
        // Event subscriptions
        GameController.Instance.OnGameStarted += GameStart;
        GameController.Instance.OnLevelReset += ResetUI;
        Player.Instance.OnGameOver += GameOver;
        // Event subscriptions for gameplay UI
        GameController.Instance.OnScoreChanged += (score) => { scoreText.text = "Score: " + score; };
        GameController.Instance.OnHighScoreChanged += (highScore) => { highScoreText.text = "High Score: " + highScore; };
        GameController.Instance.OnTimeChanged += (time) => { timeText.text = "Time: " + time + "s"; };
        GameController.Instance.OnAsteroidPassed += (amount) => { asteroidsPassedText.text = "Asteroids Passed: " + amount; };

        // Enables/disables gameplay UI: score, high score, play time, and amount of asteroids passed
        // Disable gameplay UI until the game starts
        GameUISetActive(false);
    }


    // OnGameStarted event
    private void GameStart()
    {
        gameStartWindow.SetActive(false);
        ResetUI();
    }
    // OnLevelReset event
    private void ResetUI()
    {
        gameOverWindow.SetActive(false);
        GameUISetActive(true);

        scoreText.text = "Score: 0";
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
        timeText.text = "Time: 0s";
        asteroidsPassedText.text = "Asteroids Passed: 0";
    }
    // OnGameOver event
    private void GameOver()
    {
        gameOverWindow.SetActive(true);
        GameUISetActive(false);
    }

    private void GameUISetActive(bool value)
    {
        timeText.gameObject.SetActive(value);
        asteroidsPassedText.gameObject.SetActive(value);
        scoreText.gameObject.SetActive(value);
        highScoreText.gameObject.SetActive(value);
    }
}
