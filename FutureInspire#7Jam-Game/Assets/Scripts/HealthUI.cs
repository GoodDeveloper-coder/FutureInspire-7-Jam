using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private Image _healthBar;

    void Start()
    {
        _healthManager._onTakeDamage += (float damage)=> UpdateHealthUI();
        _healthManager._onAddHealth += (float health)=> UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        _healthBar.fillAmount = _healthManager.GetHealth() / _healthManager._maxHealth;
    }

    void OnDestroy()
    {
        _healthManager._onTakeDamage -= (float damage)=> UpdateHealthUI();
        _healthManager._onAddHealth -= (float health)=> UpdateHealthUI();
    }
}
