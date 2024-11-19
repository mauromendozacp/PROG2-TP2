using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    protected EnemyAnimation _animation;
    protected EnemyNavigation _navigation;

    protected IEnemyState _currentState;
    protected IEnemyState _previousState = null;
    protected int _previousAnimStateHash;

    [SerializeField] float _damageCooldownTime = 0.8f;
    [SerializeField] float _attackCooldownTime = 3.1f;
    [SerializeField] protected LayerMask _targetLayer;
    protected int _currentDamageAmount = 0;

    float _lastDamageTime;
    float _lastAttackTime;

    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _runSpeed = 2.5f;

    public float MoveSpeed => _moveSpeed;
    public float RunSpeed => _runSpeed;

    protected virtual void Awake()
    {
        _animation = GetComponent<EnemyAnimation>();
        _navigation = GetComponent<EnemyNavigation>();
    }

    private void OnDisable()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
    }
    protected virtual void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
    }

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

    
   
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(_targetLayer, other.gameObject.layer) && CanDamage())
        {
            IDamagable recieveDamage = other.gameObject.GetComponent<IDamagable>();
            recieveDamage?.Damage(_currentDamageAmount);
            //Debug.Log($"{gameObject.name} le hace da�o de {_currentDamageAmount} a {other.name}");
            DidDamage();
        }
    }

    public void VictoryAgainstPlayer()
    {
        SetState(new EnemyVictoryState(this, _animation));
    }

    public void Die()
    {
        SetState(new EnemyDeathState(this, _animation, _navigation));
    }

    public void TogglePause()
    {
        if(_previousState == null)
        {
            _previousState = _currentState;
            _previousAnimStateHash = _animation.GetCurrentAnimationStateHash();
            SetState(new EnemyPauseState(this, _animation, _navigation));
        }
        else
        {
            _animation.Play(_previousAnimStateHash);
            SetState(_previousState);
            _previousState = null;
        }
    }
}
