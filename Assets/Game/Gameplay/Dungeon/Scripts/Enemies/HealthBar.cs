using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform _mainCamera;
    Vector3 _originalScale;
    [SerializeField] GameObject _CurrentHealthBar;
    [SerializeField] EnemyHealth _healtheable;
    HealthBarColor _healthBarColor;
    Renderer _renderer;
    

    private enum HealthBarColor { Green, Yellow, Red }

    private void Awake()
    {
        _originalScale = _CurrentHealthBar.transform.localScale;
        _renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        _mainCamera = GameObject.FindWithTag("MainCamera").transform;
        _healthBarColor = HealthBarColor.Green;
        _CurrentHealthBar.GetComponent<SpriteRenderer>().color = Color.green;
    }

    void Update()
    {
        transform.LookAt(_mainCamera);
    }

    public void UpdateHealthBar()
    {
        float value = ((float)_healtheable.GetCurrentHealth()) / _healtheable.GetMaxHealth();
        _CurrentHealthBar.transform.localScale = new Vector3(_originalScale.x * value, _originalScale.y, _originalScale.z);
        if (value >= 0.80f && _healthBarColor != HealthBarColor.Green)
        {
            _healthBarColor = HealthBarColor.Green;
            _CurrentHealthBar.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (value >= 0.25f && value < 0.8f && _healthBarColor != HealthBarColor.Yellow)
        {
            _healthBarColor = HealthBarColor.Yellow;
            _CurrentHealthBar.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        else if (value < 0.25 && _healthBarColor != HealthBarColor.Red)
        {
            _healthBarColor = HealthBarColor.Red;
            _CurrentHealthBar.GetComponent<SpriteRenderer>().color = Color.red;
        } 
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        if(!_healtheable.IsDead()) gameObject.SetActive(true);
    }
}
