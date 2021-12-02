using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Spawn Data")]
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private int poolSize = 15;
    [SerializeField] private float spawnOffset = 60f;
    [Header("Spawn Timer Data")]
    [SerializeField] private Timer spawnTimer;
    [SerializeField] private float maxSpawnDelay = 3f;
    [SerializeField] private float delayReduction = 0.1f;
    [SerializeField] private float minSpawnDelay = 0.6f;

    private Queue<Asteroid> _asteroids = new Queue<Asteroid>();

    private float _currentSpawnDelay;


    private void Start()
    {
        // Event subscriptions
        GameController.Instance.OnLevelReset += ResetAsteroids;
        GameController.Instance.OnGameStarted += GameStart;
    }


    // OnGameStart event
    private void GameStart()
    {
        _currentSpawnDelay = maxSpawnDelay;
        spawnTimer.ResetTime(_currentSpawnDelay);
        spawnTimer.OnTimerEnded += SpawnObjectAndReduceDelay;

        for (int i = 0; i < poolSize; i++)
        {
            Asteroid asteroid = Instantiate(asteroidPrefab, Vector3.up * 1.5f, Quaternion.identity);
            asteroid.gameObject.SetActive(false);
            _asteroids.Enqueue(asteroid);
        }
    }
    // OnLevelReset event
    private void ResetAsteroids()
    {
        _currentSpawnDelay = maxSpawnDelay;
        spawnTimer.ResetTime(_currentSpawnDelay);

        foreach (Asteroid asteroid in _asteroids)
        {
            asteroid.gameObject.SetActive(false);
        }
    }
    
    // OnTimerEnded event
    private void SpawnObjectAndReduceDelay()
    {
        // Get Asteroid from Queue
        Asteroid asteroid = _asteroids.Dequeue();
        // Activate it
        asteroid.gameObject.SetActive(true);
        asteroid.ResetAsteroid();
        // Set position further down the road, on a random X coordinate in the range of road width
        asteroid.transform.SetPosZ( Player.Instance.transform.position.z + spawnOffset );
        asteroid.transform.SetPosX( Random.Range(-GameController.Instance.RoadWidth, GameController.Instance.RoadWidth) );
        // Put asteroid at the end of queue to use it later if needed
        _asteroids.Enqueue(asteroid);

        // Reduce spawn delay by value of delay reduction, but not less than minSpawnDelay
        _currentSpawnDelay -= delayReduction;
        _currentSpawnDelay = _currentSpawnDelay < minSpawnDelay ? minSpawnDelay : _currentSpawnDelay;
        spawnTimer.ResetTime(_currentSpawnDelay);
    }
}
