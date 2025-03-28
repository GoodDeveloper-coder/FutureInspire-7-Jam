using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Fade _fade;

    public void LoadScene(int sceneIndex)
    {
        _fade.FadeIn(1);
        StartCoroutine(LoadSceneWithDelay(sceneIndex, 1));
    }

    private IEnumerator LoadSceneWithDelay(int sceneIndex, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneIndex);
    }
}
