using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerController : MonoBehaviour
{
    private FakePlayerMovement _movement;
    private FakePlayerAttack _attack;
    private FakePlayerHealth _health;

    private void Awake()
    {
        _movement = GetComponent<FakePlayerMovement>();
        _attack = GetComponent<FakePlayerAttack>();
        _health = GetComponent<FakePlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _attack.Attack();
        }
    }
}
