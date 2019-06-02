using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWeapon : MonoBehaviour
{
    //Scripts playerinventory pour récupérer les dégâts infliger par le joueur
    PlayerInventory playerInv;

    //ID de l'arme actuelle
    private int weaponID=1;

    //Liste des armes qui peuvent se trouver dans la main du personnage
    public List<GameObject> weaponList = new List<GameObject>();

    void Start()
    {
        playerInv = gameObject.GetComponent<PlayerInventory>();
    }
    void Update()
    {
        if(transform.childCount > 0) //Vérifie en permanance si l'objet que possède se script à un enfant (càd une arme)
        {
            weaponID = gameObject.GetComponentInChildren<ItemOnObject>().item.itemID; //weaponID prend l'ID de l'arme (présent dans la BDD)
        }
        else  //Si pas d'arme équipé
        {
            weaponID = 0; //0 signifie qu'il n'y a pas d'arme équipé
            for (int i = 0; i < weaponList.Count; i++)
            {
                weaponList[i].SetActive(false); //On désactive l'arme qui était équipé
            }
        }

        //A copier/coller pour chaque arme en changeant le weaponID

        //Epée en fer
        if (weaponID == 1 && transform.childCount > 0) 
        {
            for(int i=0; i<weaponList.Count; i++)
            {
                if(i == 0)
                {
                    weaponList[i].SetActive(true);
                    
                }
            }
        }
        
        //Marteau Blanc
        if(weaponID == 2 && transform.childCount > 0)
        {
            for(int i = 0; i < weaponList.Count; i++)
            {
                if (i == 1)
                {
                    weaponList[i].SetActive(true);
                    
                }
            }
        }

        //Hache Blanche
        if (weaponID == 3 && transform.childCount > 0)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (i == 2)
                {
                    weaponList[i].SetActive(true);

                }
            }
        }

    }
}
