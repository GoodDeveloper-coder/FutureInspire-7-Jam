using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int _coins { get; private set; }
    public int _enemiesKilled { get; private set;}
    public event Action _onRoundsOver;
    public static GameManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadCoins();
    }

    void Update()
    {
        
    }

    public void KilledEnemy()
    {
        _enemiesKilled++;
    }

    public void RestartKilledEnemiesCounter()
    {
        _enemiesKilled = 0;
    }

    public void OnRoundsOver()
    {
        _onRoundsOver?.Invoke();
    }

    public void AddCoins(int coins)
    {
        _coins += coins;
        SaveCoins();
    }

    public void RemoveCoins(int coins)
    {
        _coins -= coins;
        SaveCoins();
    }

    public int GetCoins()
    {
        return _coins;
    }

    void LoadCoins()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            _coins = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            _coins = 0;
        }
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", _coins);
    }

    void OnDestroy()
    {
        SaveCoins();
    }
}
