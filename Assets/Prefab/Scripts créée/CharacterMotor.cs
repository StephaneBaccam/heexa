using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMotor : MonoBehaviour
{
    
    //Scripts playerinventory pour récupérer les variables 
    PlayerInventory playerInv;

    //Variables concernant les sphères à rammaser
    public Text countSphere;
    public int count;

    //Animation
    Animation animations;

    //Vitesse déplacement
    public float vitMarcher;
    public float vitCourir;
    public float vitRotation;


    //Variables attaques
    public float attCooldown; //Délai entre chaque attaque
    private bool isAttacking; //Si le personnage est entrain d'attaquer ou non
    private float currentCooldown; //Temps restant avant de réattaquer
    public float attRange; //Distance d'attaque
    public GameObject rayhit;
    

    //Variables sort d'attaques
    public float coutSortFoudre;
    public GameObject sortFoudreGO;
    public float vitSortFoudre;
    public GameObject rayhitsorts;

    //Touches
    public string toucheHaut;
    public string toucheBas;
    public string toucheRotGauche;
    public string toucheRotDroite;

    public Vector3 hauteurSaut;
    CapsuleCollider collisionJoueur;

    //Etat du perso
    public bool isDead = false;

    public GameObject Zone;

    void Start()
    {
        animations = gameObject.GetComponent<Animation>();                          //la variable animations contiendra les différents animations
        collisionJoueur = gameObject.GetComponent<CapsuleCollider>();
        rayhit = GameObject.Find("Rayhit");
        playerInv = gameObject.GetComponent<PlayerInventory>();
        count = 0;
        SetCountText();                                                             //Fonction pour afficher le texte
    }


    void Update()
    {
        if (!isDead) //Si le perso est vivant
        {
            //Si on ne fait rien
            if (!Input.GetKey(toucheHaut) && !Input.GetKey(toucheBas))
            {
                if (!isAttacking)
                {
                    animations.Play("idle");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Lorsque l'on appui sur le bouton gauche de la souris
                {
                    Attack(); //Appel de la fonction Attack
                }
                if (Input.GetKeyDown(KeyCode.R)) //Lorsque l'on appui sur le bouton droit de la souris
                {
                    sortAttack(); //Appel de la fonction sort d'attaque
                }
            }

            //Marcher en avant
            if (Input.GetKey(toucheHaut) && !Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, vitMarcher * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Lorsque l'on appui sur le bouton gauche de la souris
                {
                    Attack(); //Appel de la fonction Attack
                }
                if (Input.GetKeyDown(KeyCode.R)) //Lorsque l'on appui sur le bouton droit de la souris
                {
                    sortAttack(); //Appel de la fonction sort d'attaque
                }
            }

            //Courir
            if (Input.GetKey(toucheHaut) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, vitCourir * Time.deltaTime);
                animations.Play("run");
            }

            //Marcher en arrière
            if (Input.GetKey(toucheBas))
            {
                transform.Translate(0, 0, -(vitMarcher / 2) * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Lorsque l'on appui sur le bouton gauche de la souris
                {
                    Attack(); //Appel de la fonction Attack
                }
                if (Input.GetKeyDown(KeyCode.R)) //Lorsque l'on appui sur le bouton droit de la souris
                {
                    sortAttack(); //Appel de la fonction sort d'attaque
                }
            }

           

            //Rotation à gauche
            if (Input.GetKey(toucheRotGauche))
            {
                transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * vitMarcher;
            }

            //Rotation à droite
            if (Input.GetKey(toucheRotDroite))
            {
                transform.position -= transform.TransformDirection(Vector3.left) * Time.deltaTime * vitMarcher;
            }

            //Sauter
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
            {
                //Préparation du saut
                Vector3 v = gameObject.GetComponent<Rigidbody>().velocity; //Stock la vélocité du rigidbody
                v.y = hauteurSaut.y;

                //Saut
                gameObject.GetComponent<Rigidbody>().velocity = hauteurSaut;
            }
        }

        if(isAttacking) //Si le personnage est en état d'attaque
        {
            currentCooldown -= Time.deltaTime; //réduit le délai avec le temps
        }

        if(currentCooldown<=0) //Si le délai est à 0
        {
            currentCooldown = attCooldown; //On remet le délai au temps de délai pour chaque attaque
            isAttacking = false; //Le personnage n'est plus en état d'attaque
        }
    }

    bool isGrounded()
    {
        return Physics.CheckCapsule(collisionJoueur.bounds.center, new Vector3(collisionJoueur.bounds.center.x, collisionJoueur.bounds.min.y - 0.1f, collisionJoueur.bounds.center.z), 0.07f, layerMask:3); //vérifie si le perso est au sol
    }

    public void Attack()  //Fonction et permet au personnage d'attaquer
    {
        if(!isAttacking)
        {
            animations.Play("attack");

            RaycastHit hit;

            if(Physics.Raycast(rayhit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attRange)) 
            {
                Debug.DrawLine(rayhit.transform.position, hit.point, Color.red); //Affiche le raycast
                if(hit.transform.tag == "Enemy")
                {
                    print(hit.transform.name + " detecté ");
                    hit.transform.GetComponent<EnemyAI>().ApplyDamage(playerInv.currentDamage);
                }

                if (hit.transform.tag == "EnemyBossLancer")
                {
                    print(hit.transform.name + " detecté ");
                    hit.transform.GetComponent<EnemyAILancer>().ApplyDamage(playerInv.currentDamage);
                }

                if (hit.transform.tag == "EnemyBossFairy")
                {
                    print(hit.transform.name + " detecté ");
                    hit.transform.GetComponent<EnemyAIFairy>().ApplyDamage(playerInv.currentDamage);
                }
            }
        }
        isAttacking = true;
    }

    public void sortAttack()  //Fonction et permet au personnage de lancer un sort d'attaque
    {
        if (!isAttacking && playerInv.currentMana>=coutSortFoudre)
        {
            animations.Play("attack");
            GameObject sort = Instantiate(sortFoudreGO, rayhitsorts.transform.position, transform.rotation); //Faire apparaître le sort
            sort.GetComponent<Rigidbody>().AddForce(transform.forward * vitSortFoudre); //Envoie du sort
            playerInv.currentMana -= coutSortFoudre; //Mana utilisée
        }
        isAttacking = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ObjetSpe"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            if(count==5)
            {
                Zone.SetActive(true);
            }
            SetCountText();
            playerInv.GainExp(1000);
        }
    }

    void SetCountText() 
    {
        countSphere.text = "Sphere récolté : " + count.ToString() + "/6";
    }
}
