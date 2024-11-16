using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    protected IEnemyState _currentState;
    protected Animator _anim;
    protected NavMeshAgent _agent;
    [SerializeField] float _damageCooldownTime = 0.8f;
    [SerializeField] float _attackCooldownTime = 3.1f;
    [SerializeField] protected LayerMask _targetLayer;
    protected int _currentDamageAmount = 0;

    float _lastDamageTime;
    float _lastAttackTime;

    public bool CanAttack() => Time.time >= _lastAttackTime + _attackCooldownTime;
    public void DidAttack() 
    {
        _lastAttackTime = Time.time;
    }

    bool CanDamage() => Time.time >= _lastDamageTime + _damageCooldownTime;

    void DidDamage()
    {
        _lastDamageTime = Time.time;
    }

    private void Update()
    {
        _currentState.Execute();
    }

    public void SetState(IEnemyState newState)
    {
        _currentState = newState;
        _currentState.EnterState();
    }

    public void SetAnimator(string name, bool value)
    {
        _anim.SetBool(name, value);
    }

    public void TriggerAnimator(string name)
    {
        _anim.SetTrigger(name);
    }


    public void SetAgentDestination(Vector3 destination)
    {
        _agent.destination = destination;
    }

    public void ResetAgentDestination()
    {
        _agent.ResetPath();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(_targetLayer, other.gameObject.layer) && CanDamage())
        {
            IDamagable recieveDamage = other.gameObject.GetComponent<IDamagable>();
            recieveDamage?.Damage(_currentDamageAmount);
            //Debug.Log($"{gameObject.name} le hace daño de {_currentDamageAmount} a {other.name}");
            DidDamage();
        }
    }
}
