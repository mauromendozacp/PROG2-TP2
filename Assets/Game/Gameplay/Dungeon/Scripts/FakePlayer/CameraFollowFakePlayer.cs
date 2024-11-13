using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowFakePlayer : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Vector3 _offset;
    [SerializeField] float _zoomSpeed = 10f;
    [SerializeField] float _minZoom = 5f;
    [SerializeField] float _maxZoom = 20f;

    float _currentZoom = 10f;

    void Update()
    {
        _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
    }

    void LateUpdate()
    {
        transform.position = _player.position + _offset * _currentZoom;
        transform.LookAt(_player);
    }
}
