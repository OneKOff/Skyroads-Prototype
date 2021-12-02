using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action OnTimerEnded;
    public event Action OnTimerChanged;

    [SerializeField] private float startingTime;
    public float StartingTime => startingTime;

    public float TimeLeft { get; private set; }
    private bool _stopped = false;


    private void Start()
    {
        ResetTime();
    }
    private void Update()
    {
        if (_stopped) return;

        if (TimeLeft > 0f)
        {
            TimeLeft -= Time.deltaTime;
            OnTimerChanged?.Invoke();
        }
        else
        {
            _stopped = true;
            OnTimerEnded?.Invoke();
        }
    }

    public bool IsElapsed() { return TimeLeft <= 0; }
    public void ResetTime()
    {
        TimeLeft = startingTime;
        _stopped = false;
    }
    public void ResetTime(float newTime)
    {
        startingTime = newTime;
        TimeLeft = startingTime;
        _stopped = false;
    }
}
