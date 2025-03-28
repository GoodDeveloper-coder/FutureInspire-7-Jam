using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private HealthManager _playerHealthManager;
    [SerializeField] private List<Transform> _spawnEnemiesPos;
    [SerializeField] private TextMeshProUGUI _roundText;
    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    public bool _roundIsInproces { get; private set;}
    private int _countOfEnemies = 0;
    public int _round { get; private set; }
    public event Action _onStoppedRounds;

    void Start()
    {
        _roundIsInproces = false;
        _playerHealthManager._onDie += StopRounds;
    }

    public void StartRounds()
    {
        _round = 1;
        _roundIsInproces = true;
        GameManager._instance.RestartKilledEnemiesCounter();
        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        if (!_roundIsInproces)
            yield break;

        _roundText.text = "Round " + _round;
        _roundText.DOFade(1, 1).onComplete += ()=> _roundText.DOFade(0, 1);
        yield return new WaitForSeconds(3f);
        int countOfEnemies = _round + UnityEngine.Random.Range(3, 6);
        for (int i = 0; i < countOfEnemies; i++)
        {
            GameObject spawnedEnemy = Instantiate(_enemy, _spawnEnemiesPos[UnityEngine.Random.Range(0, _spawnEnemiesPos.Count)].position, Quaternion.identity);
            Enemy enemy = spawnedEnemy.GetComponent<Enemy>();
            enemy.SetTarget(_playerHealthManager.transform);
            enemy.SetSpawner(this);
            _spawnedEnemies.Add(spawnedEnemy);
            _countOfEnemies++;
        }

        yield return new WaitUntil(()=> _countOfEnemies == 0);
        _round++;
        StartCoroutine(StartRound());
    }

    void StopRounds()
    {
        foreach (GameObject enemy in _spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        _spawnedEnemies.Clear();
        GameManager._instance.OnRoundsOver();
        _roundIsInproces = false;
        _onStoppedRounds?.Invoke();
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _spawnedEnemies.Remove(enemy);
        _countOfEnemies--;
    }

    void OnDestroy()
    {
        _playerHealthManager._onDie += StopRounds;
    }
}
