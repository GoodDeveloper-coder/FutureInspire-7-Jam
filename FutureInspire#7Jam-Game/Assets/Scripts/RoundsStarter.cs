using UnityEngine;
using DG.Tweening;

public class RoundsStarter : MonoBehaviour
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private Transform _gates;
    [SerializeField] private Transform _gatesClosedPos;
    private Vector3 _gatesDeffaultPos;
    private bool _entered = false;

    void Start()
    {
        _gatesDeffaultPos = _gates.position;
        _enemiesSpawner._onStoppedRounds += OpenGates;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_enemiesSpawner._roundIsInproces)
            return;

        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            if (!_entered)
            {
                _entered = true;
                _gates.DOMove(_gatesClosedPos.position, 1f);
                _enemiesSpawner.StartRounds();
            }
            else
            {
                _entered = false;
            }
        }
    }

    void OpenGates()
    {
        _gates.DOMove(_gatesDeffaultPos, 1f);
    }

    void OnDestroy()
    {
        _enemiesSpawner._onStoppedRounds -= OpenGates;
    }
}
