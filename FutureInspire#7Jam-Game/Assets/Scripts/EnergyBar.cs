using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [Header("Bar Scale")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Transform _energyBar;

    [Header("Bar Fill Amount")]
    [SerializeField] private bool _barFillAmount = false;
    [SerializeField] private Image _barImage;

    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _playerMovement._startRunning += Appear;
        _playerMovement._stopRunning += Disappear;
    }

    void Update()
    {
        if (!_barFillAmount)
            _energyBar.transform.localScale = new Vector3(_playerMovement._energy / 10f, 1, 1);
        else
            _barImage.fillAmount = _playerMovement._energy / 10f;
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
