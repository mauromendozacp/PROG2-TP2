using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    private Rigidbody _rb;
    private bool _isGrounded = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        float horizontal = - Input.GetAxis("Horizontal");
        float vertical = - Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical).normalized * _moveSpeed;
        _rb.velocity = new Vector3(movement.x, _rb.velocity.y, movement.y);

        if (movement != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _moveSpeed);
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isGrounded = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
}
