using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionItem : MonoBehaviour
{

    PlayerInventory playerInv;

    void Start()
    {
        playerInv = gameObject.GetComponent<PlayerInventory>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInventory>().currentMana += 50;
            Destroy(gameObject);
        }
        
    }
}
