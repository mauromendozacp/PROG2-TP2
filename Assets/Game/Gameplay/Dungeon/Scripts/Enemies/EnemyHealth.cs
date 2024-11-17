using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour, IDamagable, IHealtheable
{
    [SerializeField] int _maxHealth = 50;
    [SerializeField] HealthBar _healthBar;
    int _currentHealth;
    [SerializeField] Animator _anim;
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _rootGameObject;
    [SerializeField] float _damageCooldown = 1f;
    [SerializeField, Range(0f, 100f)] float _dropChance = 40f;
    Collider _damageCollider;
    Enemy _controller;
    float _lastDamageTime;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        if(_anim == null) _anim = GetComponent<Animator>();
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_rootGameObject == null) _rootGameObject = gameObject;
        _damageCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        _healthBar.Disable();
        _controller = _rootGameObject.GetComponent<Enemy>();

    }

    public void Damage(int damage)
    {
        if (Time.time >= _lastDamageTime + _damageCooldown)
        {
            //Debug.Log($"El player le da un daño de {damage} a {gameObject.name}");
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                StartCoroutine(Die());
            }
            _healthBar.UpdateHealthBar();
            _lastDamageTime = Time.time;
        }     
    }

    public bool IsDead() => _currentHealth <= 0;

    IEnumerator Die()
    {
        //_anim.SetTrigger("Die");
        _healthBar.Disable();
        _controller.SetState(new EnemyDeathState(_controller));
        //_rb.isKinematic = true;
        //_rb.detectCollisions = false;
        _rb.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(3f);
        TrySpawnLootItem();
        Destroy(_rootGameObject);
    }

    public int GetCurrentHealth() => _currentHealth;
    public int GetMaxHealth() => _maxHealth;

    public void EnableHealthBar() => _healthBar.Enable();
    public void DisableHealthBar() => _healthBar.Disable();

    public void DisableDamage() 
    {
        _damageCollider.enabled = false;
    }

    public void TrySpawnLootItem()
    {
        if (Random.Range(0f, 100f) < _dropChance)
        {
            int itemID = ItemManager.Instance.GetRandomItemID();
            ItemManager.Instance.GenerateItemInWorldSpace(itemID, 1, transform.position);
        }
    }
}
