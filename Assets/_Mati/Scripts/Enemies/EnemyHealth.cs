using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : MonoBehaviour, IDamagable, IHealtheable
{
    [SerializeField] int _maxHealth = 50;
    [SerializeField] HealthBar _healthBar;
    int _currentHealth;
    Animator _anim;
    Rigidbody _rb;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _healthBar.Disable();
    }

    public void Damage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            StartCoroutine(Die());
        }
        _healthBar.UpdateHealthBar();
    }

    public bool IsDead() => _currentHealth <= 0;

    IEnumerator Die()
    {
        _anim.SetTrigger("Die");
        _healthBar.Disable();
        //_rb.isKinematic = true;
        //_rb.detectCollisions = false;
        _rb.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public int GetCurrentHealth() => _currentHealth;
    public int GetMaxHealth() => _maxHealth;

    public void EnableHealthBar() => _healthBar.Enable();
    public void DisableHealthBar() => _healthBar.Disable();
}
