using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloon : MonoBehaviour
{
    private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;

        //Rotate the baloon to face the player then rotate to face correctly according to the img
        transform.rotation = Quaternion.LookRotation(transform.position - _player.position);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 1f);
    }
}
