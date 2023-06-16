using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourglassSpawner : MonoBehaviour
{
    [SerializeField] private GameObject     _hourglassPref;
    [SerializeField] private GameObject[]   _spawners;
    [SerializeField] private float          _timeRespawn;

    private bool    _spawned;
    private float   _currTimeRespawn;

    void Start()
    {
        _spawned = false;
        _currTimeRespawn = _timeRespawn;
    }

    void Update()
    {
        spawnHourglass();
        runTimer();
    }

    void spawnHourglass()
    {
        if (!_spawned && _currTimeRespawn >= _timeRespawn)
        {
            GameObject chosenSpawner = _spawners[Random.Range(0, _spawners.Length)];

            Instantiate(_hourglassPref, chosenSpawner.transform.position, Quaternion.identity);

            _spawned = true;
            _currTimeRespawn = 0;
        }
    }

    void runTimer()
    {
        if (_spawned == false)
        {
            _currTimeRespawn += Time.deltaTime;
            print(_currTimeRespawn);
        }
    }

    public void changeSpawnedState()
    {
        _spawned = false;
    }

}
