using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    void Update()
    {
        if (_target != null)
        {
            transform.LookAt(_target);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
