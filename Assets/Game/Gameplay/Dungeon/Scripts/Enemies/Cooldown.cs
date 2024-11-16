
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    private float _cooldownTime;
    private float _nextAvailableAttack;

    public Cooldown(float cooldown)
    {
        _cooldownTime = cooldown;
    }

    public bool CanDo()
    {
        return Time.time >= _nextAvailableAttack;
    }

    public void Did()
    {
        _nextAvailableAttack = Time.time + _cooldownTime;
    }
}
