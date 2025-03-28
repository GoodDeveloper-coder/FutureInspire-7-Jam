using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private HealthManager _healthManager;

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        _healthManager._onDie += OnDie;
    }

    void OnDie()
    {
        _playerMovement.StartCoroutine(_playerMovement.DisableMovementForTime(2f));
        _healthManager.Heal();
    }

    void OnDestroy()
    {
        _healthManager._onDie -= OnDie;
    }
}
