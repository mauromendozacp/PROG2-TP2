using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    bool _isTimerComplete;
    public bool IsTimerComplete => _isTimerComplete;
    public IEnumerator StartTimer(float durationInSeconds)
    {
        _isTimerComplete = false;
        yield return new WaitForSeconds(durationInSeconds);
        _isTimerComplete = true;
    }
}
