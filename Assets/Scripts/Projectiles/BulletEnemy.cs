using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] float  _lifeTime;
    [SerializeField] int    _damage;

    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(_damage);
        }
        Destroy(gameObject);
    }

}
