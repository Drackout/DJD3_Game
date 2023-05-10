using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[]   _enemies;
    [SerializeField] private int            _spawnerSize;

    private Vector3 _pos;

    void Start()
    {
        _pos = transform.position;
    }

    void Update()
    {
        // Gotta work with time
        if (Input.GetKeyDown(KeyCode.H))
        {

            Vector3 spawnPosition = new Vector3(Random.Range(_pos.x+(-_spawnerSize), _pos.x+(_spawnerSize)), 0, Random.Range(_pos.z+(-_spawnerSize), _pos.z+(_spawnerSize)));
            GameObject chosenEnemy = _enemies[Random.Range(0, _enemies.Length-1)];

            Instantiate(chosenEnemy, spawnPosition, Quaternion.identity);
        }
    }
}
