using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [SerializeField] float  _lifeTime;
    [SerializeField] int    _damage;

    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damage(_damage);
        }
        
        if (collision.transform.tag == "NPC")
        {
            collision.gameObject.GetComponent<NPC>().Damage(_damage);
        }
        Destroy(gameObject);
    }

}
