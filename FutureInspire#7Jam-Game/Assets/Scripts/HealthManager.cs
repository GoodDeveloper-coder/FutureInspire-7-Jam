using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float _health;
    public float _maxHealth { get; private set; }

    public event Action<float> _onTakeDamage;
    public event Action<float> _onAddHealth;
    public event Action _onDie;

    void Start()
    {
        _maxHealth = _health;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _onTakeDamage?.Invoke(damage);
        Camera.main.DOShakeRotation(1f, 0.35f);

        if (_health <= 0)
        {
            _onDie?.Invoke();
        }
    }

    public void AddHealth(float health)
    {
        _health -= health;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _onAddHealth?.Invoke(health);
    }

    public void Heal()
    {
        _health = _maxHealth;
        _onAddHealth?.Invoke(_maxHealth);
    }

    public float GetHealth()
    {
        return _health;
    }
}
