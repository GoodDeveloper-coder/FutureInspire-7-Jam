using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image _fade;

    void Start()
    {
        _fade.DOFade(0, 1);
    }

    public void FadeIn(float time)
    {
        _fade.DOFade(1, 1);
    }

    public void FadeOut(float time)
    {
        _fade.DOFade(1, 1);
    }
}
