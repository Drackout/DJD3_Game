using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSolo : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _camera;

    void Update()
    {
        int DistanceAway = 5;

        Vector3 PlayerPOS = _player.transform.transform.position;

        _camera.transform.position = new Vector3(PlayerPOS.x + 1.2f, PlayerPOS.y+1.2f, PlayerPOS.z - DistanceAway);
        
    }
}
