using DG.Tweening;
using TMPro;
using UnityEngine;

public class RoundsOver : MonoBehaviour
{
    [SerializeField] private Transform _roundsOverMenu;
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private TextMeshProUGUI _rounds;
    [SerializeField] private TextMeshProUGUI _enemiesKilled;

    void Start()
    {
        GameManager._instance._onRoundsOver += AppearMenu;
    }

    public void AppearMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        _roundsOverMenu.localScale = Vector3.zero;
        _roundsOverMenu.gameObject.SetActive(true);
        _roundsOverMenu.DOScale(Vector3.one, 1);
        int score = GameManager._instance._enemiesKilled * _enemiesSpawner._round * 10;
        int bestScore = score;
        int coins = GameManager._instance._enemiesKilled * _enemiesSpawner._round * 3;
        GameManager._instance.AddCoins(coins);
        _enemiesKilled.text = "Enemies Killed: " + GameManager._instance._enemiesKilled;
        _score.text = "Score: " + score;
        _coins.text = "Coins: " + coins;
        _rounds.text = "Rounds: " + _enemiesSpawner._round;

        if (PlayerPrefs.HasKey("BestScore"))
        {
            if (PlayerPrefs.GetInt("BestScore") < score)
            {
                bestScore = score;
            }
            else
            {
                bestScore = PlayerPrefs.GetInt("BestScore");
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", score);
        }    
        _bestScore.text = "Best Score: " + bestScore;
    }

    public void DisappearMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _roundsOverMenu.DOScale(Vector3.zero, 0.5f);
    }

    void OnDestroy()
    {
        GameManager._instance._onRoundsOver -= AppearMenu;
    }
}
