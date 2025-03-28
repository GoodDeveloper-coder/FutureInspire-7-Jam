using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float _speed = 50f;

    void Update()
    {
        transform.eulerAngles += new Vector3(0, _speed * Time.deltaTime, 0); 
    }
}
