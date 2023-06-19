using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class Hourglass : MonoBehaviour
{    
    [SerializeField] private Timer              _timer;
    [SerializeField] private float              _timeToAdd;
    [SerializeField] private HourglassSpawner   _hourglassSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Get", transform.position);
            _timer.changeTimer(_timeToAdd);
            _hourglassSpawner.changeSpawnedState();
            Destroy(gameObject);
        }
    }
}
