using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<bool> OnSpeedBoostChanged;

    [SerializeField] private Transform playerBody;
    [Header("Speed")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private float sideSpeed = 10f;
    [Header("Ship Rotation")]
    [SerializeField] private float maxRotationAngle = 30f;
    [SerializeField] private float rotationSpeed = 60f;

    private float _currentForwardSpeed;
    private float _currentSideSpeed;
    private float _currentRotationAngle;

    private bool _speedBoostFlag = false;


    private void Update()
    {
        SpeedBoostInput();
        HorizontalInput();
    }
    private void FixedUpdate()
    {
        MoveForward();
        MoveSide();
        RotatePlayerBody();
    }


    private void SpeedBoostInput()
    {
        // Check if player changed the speed boost state (pressed or released Spacebar)
        bool _newSpeedBoost = Input.GetKey(KeyCode.Space);
        if (_speedBoostFlag != _newSpeedBoost)
        {
            OnSpeedBoostChanged?.Invoke(_newSpeedBoost);
        }
        _speedBoostFlag = _newSpeedBoost;

        // Increases forward speed if Spacebar if pressed
        _currentForwardSpeed = forwardSpeed;
        if (_speedBoostFlag)
        {
            _currentForwardSpeed *= speedBoostMultiplier;
        }
    }
    private void HorizontalInput()
    {
        _currentSideSpeed = Input.GetAxis("Horizontal") * sideSpeed;
    }

    private void MoveForward()
    {
        transform.Translate(0, 0, _currentForwardSpeed * Time.deltaTime);
    }
    private void MoveSide()
    {
        transform.Translate(_currentSideSpeed * Time.deltaTime, 0, 0);
        transform.SetPosX(Mathf.Clamp(transform.position.x, -GameController.Instance.RoadWidth, GameController.Instance.RoadWidth));
    }
    private void RotatePlayerBody()
    {
        _currentRotationAngle = Mathf.LerpAngle(_currentRotationAngle,
           -_currentSideSpeed / sideSpeed * maxRotationAngle,
           rotationSpeed * Time.deltaTime);
        playerBody.SetRotZ(_currentRotationAngle);
    }
}