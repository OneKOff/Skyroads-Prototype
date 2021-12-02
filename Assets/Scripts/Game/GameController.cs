using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton for easier event subscription
    public static GameController Instance { get; private set; }

    public event Action OnGameStarted;
    public event Action OnLevelReset;

    public event Action<int> OnScoreChanged;
    public event Action<int> OnHighScoreChanged;
    public event Action<int> OnTimeChanged;
    public event Action<int> OnAsteroidPassed;

    [SerializeField] private Vector3 playerStartPosition = new Vector3(0, 1, 0);
    [SerializeField] private float scoreBoostMultiplier = 2f;

    [SerializeField] private float roadWidth = 5f;
    public float RoadWidth => roadWidth;

    public float Score { get; private set; }
    public int HighScore { get; private set; }
    public float TimePlayed { get; private set; }
    public int AsteroidsPassed { get; private set; }

    private bool _gameStarted = false;
    private bool _gameOver = false;

    private bool _speedBoost = false;


    // Singleton check for GameController copies
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        // Event subscriptions
        Player.Instance.OnGameOver += () => { _gameOver = true; };
        Player.Instance.PlayerMovement.OnSpeedBoostChanged += (speedBoost) => { _speedBoost = speedBoost; };

        // Get High Score from PlayerPrefs, or create a key for it if there's none
        if (PlayerPrefs.HasKey("HighScore"))
        {
            HighScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", 0);
            HighScore = 0;
        }

        LevelReset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        // Press any key to start the game
        if (!_gameStarted && Input.anyKeyDown) { GameStart(); } 
        // Update player data if the game already started and isn't over yet
        if (!_gameStarted || _gameOver) { return; }

        ChangePlayerStats();
    }


    // Event invoking functions
    private void GameStart()
    {
        _gameStarted = true;
        OnGameStarted?.Invoke();
    }
    public void LevelReset()
    {
        _gameOver = false;
        TimePlayed = 0f;
        Score = 0f;
        AsteroidsPassed = 0;

        Player.Instance.transform.position = playerStartPosition;
        Player.Instance.gameObject.SetActive(true);

        OnLevelReset?.Invoke();
    }
    private void ChangePlayerStats()
    {
        TimePlayed += Time.deltaTime;
        OnTimeChanged?.Invoke((int)TimePlayed);

        if (_speedBoost)
        {
            Score += Time.deltaTime * scoreBoostMultiplier;
        }
        else
        {
            Score += Time.deltaTime;
        }
        OnScoreChanged?.Invoke((int)Score);

        if ((int)Score > HighScore)
        {
            HighScore = (int)Score;
            PlayerPrefs.SetInt("HighScore", HighScore);
            OnHighScoreChanged?.Invoke(HighScore);
        }
    }
    public void AsteroidPassed()
    {
        AsteroidsPassed++;
        Score += 5;
        OnAsteroidPassed?.Invoke(AsteroidsPassed);
    }
}
