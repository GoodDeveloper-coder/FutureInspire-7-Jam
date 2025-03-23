using DG.Tweening;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Transform _energyBar;
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _playerMovement._startRunning += Appear;
        _playerMovement._stopRunning += Disappear;
    }

    void Update()
    {
        _energyBar.transform.localScale = new Vector3(_playerMovement._energy / 10f, 1, 1);
    }

    void Appear()
    {
        _canvasGroup.DOFade(1, 0.5f);
    }

    void Disappear()
    {
        _canvasGroup.DOFade(0, 0.5f);
    }

    void OnDestroy()
    {
        _playerMovement._startRunning -= Appear;
        _playerMovement._stopRunning -= Disappear;
    }
}
