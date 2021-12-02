using UnityEngine;

public class ParticleSpeedBooster : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private float boostMultiplier = 2f;

    private float _basicParticleSpeed;


    private void Start()
    {
        // Event subscription
        Player.Instance.PlayerMovement.OnSpeedBoostChanged += SpeedBoostParticles;

        // Get start speed of particles from particle system for reference
        _basicParticleSpeed = ps.main.startSpeedMultiplier;
    }


    // OnSpeedBoostChanged event
    private void SpeedBoostParticles(bool value)
    {
        var main = ps.main;

        // If speed boost is on, the particles fly faster, and vice versa
        main.startSpeed = _basicParticleSpeed;
        if (value)
        {
            main.startSpeed = _basicParticleSpeed * boostMultiplier;
        }
    }
}
