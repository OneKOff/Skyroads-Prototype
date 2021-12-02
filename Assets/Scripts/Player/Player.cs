using System;
using UnityEngine;

// Required to register collisions with asteroids
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // Singleton to easily get player position and playerMovement's data (OnSpeedBoostChanged event)
    public static Player Instance { get; private set; }

    public event Action OnGameOver;

    [SerializeField] private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;


    private void Awake()
    {
        // Singleton check for Player copies
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        // Event subscription
        GameController.Instance.OnGameStarted += () => { gameObject.SetActive(true); };
        // Disabled until the game is started
        gameObject.SetActive(false);
    }

    // When Player collides with an asteroid, invokes OnGameOver event and disables Player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Asteroid>())
        {
            OnGameOver?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
