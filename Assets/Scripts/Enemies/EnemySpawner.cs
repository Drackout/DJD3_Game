using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[]   _enemies;
    [SerializeField] private int            _spawnerSize;
    [SerializeField] private IntGlobalValue _currentEnemies;
    [SerializeField] private int            _maxEnemySpawner;
    [SerializeField] private GameObject[]   _spawners;

    private Vector3 _pos;
    private float   _timer;

    void Start()
    {
        _currentEnemies.SetValue(0);
        _timer = 0f;
    }

    void FixedUpdate()
    {
        _timer += Time.deltaTime;

        // Increase enemy spawn with the time passing (find a better way to do this)
        if (_timer > 100)
            _maxEnemySpawner = 10;
        else if (_timer > 80)
            _maxEnemySpawner = 9;
        else if (_timer > 60)
            _maxEnemySpawner = 8;
        else if (_timer > 40)
            _maxEnemySpawner = 7;
        else if (_timer > 20)
            _maxEnemySpawner = 6;


        // Check max enemies
        if (_currentEnemies.GetValue() < _maxEnemySpawner)
        {
            // Select random spawner, get it's position
            GameObject chosenSpawner = _spawners[Random.Range(0, _spawners.Length)];
            _pos = chosenSpawner.transform.position;

            // Select random enemy, spawns it in the spawner area
            Vector3 spawnPosition = new Vector3(Random.Range(_pos.x+(-_spawnerSize), _pos.x+(_spawnerSize)), 0, Random.Range(_pos.z+(-_spawnerSize), _pos.z+(_spawnerSize)));
            GameObject chosenEnemy = _enemies[Random.Range(0, _enemies.Length)];

            Instantiate(chosenEnemy, spawnPosition, Quaternion.identity);

            _currentEnemies.ChangeValue(1);
        }
    }
}
