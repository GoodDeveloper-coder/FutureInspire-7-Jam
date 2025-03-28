using UnityEngine;
using UnityEngine.Events;

public class SwordAnimationHelper : MonoBehaviour
{
    [SerializeField] private UnityEvent _onAttack;

    public void Attack()
    {
        _onAttack?.Invoke();
    }
}
