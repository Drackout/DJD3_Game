using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{    
    [SerializeField] private Timer              _timer;
    [SerializeField] private float              _timeToAdd;
    [SerializeField] private HourglassSpawner   _hourglassSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _timer.changeTimer(_timeToAdd);
            _hourglassSpawner.changeSpawnedState();
            Destroy(gameObject);
        }
    }
}
