using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Header("Offset Data")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float offsetBoostMultiplier = 2f;
    [SerializeField] private float cameraSpeed = 3f;

    private Vector3 _currentOffset;
    private Vector3 _targetOffset;


    private void Start()
    {
        // Event subscription
        Player.Instance.PlayerMovement.OnSpeedBoostChanged += SpeedBoostZoom;

        _currentOffset = transform.position;
        _targetOffset = offset;
    }
    private void Update()
    {
        // Smoothly changes the current camera offset to the target offset
        _currentOffset.x = Mathf.Lerp(_currentOffset.x, _targetOffset.x, Mathf.Abs(cameraSpeed * offset.normalized.x) * Time.deltaTime);
        _currentOffset.y = Mathf.Lerp(_currentOffset.y, _targetOffset.y, Mathf.Abs(cameraSpeed * offset.normalized.y) * Time.deltaTime);
        _currentOffset.z = Mathf.Lerp(_currentOffset.z, _targetOffset.z, Mathf.Abs(cameraSpeed * offset.normalized.z) * Time.deltaTime);
    }
    private void LateUpdate()
    {
        // Set camera position to follow player with current offset
        transform.position = target.position + _currentOffset;
    }


    // OnSpeedBoostChanged event
    private void SpeedBoostZoom(bool speedBoost)
    {
        // Reduces camera's target offset if boost is used
        _targetOffset = offset;
        if (speedBoost)
        {
            _targetOffset /= offsetBoostMultiplier;
        }
    }
}
