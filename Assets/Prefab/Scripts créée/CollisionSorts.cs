using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSorts : MonoBehaviour
{

    public float sortDamage;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyAI>().ApplyDamage(sortDamage);
        }
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

}
