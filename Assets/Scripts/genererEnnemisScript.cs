using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genererEnnemisScript : MonoBehaviour
{
    /*------------
     * VARIABLES *
     ------------*/
    // Reference aux ennemis
    [SerializeField] GameObject[] ennemis;
    // Temps de generation pour les Zombunny
    float tempsZombunny;

    void Start()
    {
            // Le temps de clonage des zombunny est entre 2 et 5 secondes
            tempsZombunny = Random.Range(2f, 5f);
            // On commence a invoquer les ennemis au debut du jeu
            InvokeRepeating("ClonageHellephants", 0f, 5f);
            InvokeRepeating("ClonageZombear", 0f, 2f);
            InvokeRepeating("ClonageZombunny", 0f, tempsZombunny);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*---------------------------
     * FONCTION SUPPLEMENTAIRES *
     ---------------------------*/
    // Clone les Hellephants
    void ClonageHellephants()
    {
        if (!DeplacementPersoScript.partieFinie)
        {
            GameObject cloneHellephant = Instantiate(ennemis[0]);
            cloneHellephant.SetActive(true);
        }
    }

    // Clone les Zombears
    void ClonageZombear()
    {
        if (!DeplacementPersoScript.partieFinie)
        {
            GameObject cloneZombear = Instantiate(ennemis[1]);
            cloneZombear.SetActive(true);
        }
    }

    // Clone les Zombunnys
    void ClonageZombunny()
    {
        if (!DeplacementPersoScript.partieFinie)
        {
            GameObject cloneZombunny = Instantiate(ennemis[2]);
            cloneZombunny.SetActive(true);
        }
    }
}
