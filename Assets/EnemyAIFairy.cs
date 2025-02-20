﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIFairy : MonoBehaviour
{
    public GameObject winPanel;

    public PlayerInventory playerInv;

    //Exp
    public float exp;

    //Distance entre le joueur et l'ennemi
    private float Distance;

    //Distance entre l'ennemi et son spawn
    private float DistanceBase;
    private Vector3 basePosition;

    //Cible de l'ennemi
    public Transform Target;

    //Distance de poursuite
    public float chaseRange = 100;

    //Distance d'attaque
    public float attRange = 2f;

    //Délai des attaques
    public float attCooldown = 1;
    private float attTime;

    //Montant des dégâts 
    public float Damage;

    //Agent de navigation
    private UnityEngine.AI.NavMeshAgent agent;

    //Vie et état de l'ennemi
    public float ennemyHealth;
    private bool isDead = false;

    //loots
    public GameObject[] loots;

    Animator animations;

    void Start()
    {
        animations = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        attTime = Time.time;
        basePosition = transform.position;
        playerInv = gameObject.GetComponent<PlayerInventory>();
        playerInv = GameObject.Find("Joueur").GetComponent<PlayerInventory>();
    }


    void Update()
    {
        if (!isDead)
        {

            //On cherche le joueur en permanance
            Target = GameObject.Find("Joueur").transform;

            //Calcule distance joueur ennemi
            Distance = Vector3.Distance(Target.position, transform.position);

            //Calcule distance ennemi et son spawn
            DistanceBase = Vector3.Distance(basePosition, transform.position);

            //Si l'ennemi est loin de son spawn
            if (Distance > chaseRange && DistanceBase > 1)
            {
                BackBase();
            }

            //Si l'ennemi est proche mais pas à la distance d'attaque
            else if (Distance < chaseRange && Distance > attRange)
            {
                chase();
            }

            //Si l'ennemi est assez proche pour attaquer
            else if (Distance < attRange)
            {
                attack();
            }
        }
    }

    //Si le personnage est dans la zone de recherche de l'ennemi
    void chase()
    {
        animations.Play("walk");
        agent.destination = Target.position;
    }

    void attack()
    {
        //Empeche l'ennemi de traverser le joueur
        agent.destination = transform.position;

        if (Time.time > attTime)
        {
            animations.Play("atk01");
            Target.GetComponent<PlayerInventory>().ApplyDamage(Damage);
            Debug.Log("L'ennemi a fait " + Damage + " points de dégâts");
            attTime = Time.time + attCooldown;
        }
    }

    public void ApplyDamage(float Damage)
    {
        if (!isDead)
        {
            ennemyHealth = ennemyHealth - Damage;
            print(gameObject.name + "a subit" + Damage + "points de dégâts");

            if (ennemyHealth <= 0)
            {
                playerInv.GainExp(exp);
                Dead();
            }
        }
    }

    //Retour à la base 
    public void BackBase()
    {
        animations.Play("walk");
        agent.destination = basePosition;
    }

    public void Dead()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        isDead = true;
        print("Vous avez tué un" + gameObject.name + "!");
        winPanel.SetActive(true);
        //apparition du loot
        int randomNumber = Random.Range(0, loots.Length);
        GameObject finalLoot = loots[randomNumber];
        Instantiate(finalLoot, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }
}
