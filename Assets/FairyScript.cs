using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyScript : MonoBehaviour
{
    private float distance;

    public float closeRange;

    //Animations
    Animator animations;

    //Permet à la fée de se déplacer elle même
    private UnityEngine.AI.NavMeshAgent agent;

    //Cible (joueur)
    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        animations = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Target.position, transform.position);
        if (distance > closeRange)
            Follow();
        else if (distance < closeRange)
            Stay();
    }

    public void Follow()
    {
        animations.Play("walk");
        agent.destination = Target.position;
    }

    public void Stay()
    {
        animations.Play("idle");
        agent.destination = transform.position;
    }
}
