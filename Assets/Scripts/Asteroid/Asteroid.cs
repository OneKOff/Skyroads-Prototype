using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float disableDistance = 10f;

    private bool _passed;


    private void Start()
    {
        // Resets _passed value to true, 
        // used by AsteroidSpawner when re-using asteroids
        ResetAsteroid();
    }
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        // Checks when player passes this asteroid
        CheckPass();
        // When asteroid gets behind player, 
        // disable it when the distance between them is greater than disableDistance
        CheckDisable();
    }


    private void CheckPass()
    {
        if (!_passed && Player.Instance.transform.position.z > transform.position.z)
        {
            _passed = true;
            // Function that fires OnAsteroidPassed event that updates game UI.
            GameController.Instance.AsteroidPassed();
        }
    }
    private void CheckDisable()
    {
        if (Player.Instance.transform.position.z - transform.position.z > disableDistance)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetAsteroid()
    {
        _passed = false;
    }
}
