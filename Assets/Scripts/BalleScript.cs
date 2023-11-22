using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalleScript : MonoBehaviour
{
    //Script à associer à la balle

    /*#################################################
    -- variables publiques à définir dans l'inspecteur
    #################################################*/
    public GameObject impactTir; // Référence au Prefab à instancier lorsque le tir frappe un objet. (Prefab ParticulesHit)
    public GameObject personnage; // Référence au personnage
    // Sons lorsque les differents ennemis sont touches
    public AudioClip sonBlesseHelle; 
    public AudioClip sonBlesseZombun;
    public AudioClip sonBlesseZombear;

    AudioSource audioS; // Reference a l'audisource


    private void Start()
    {
        // Association de l'audiosource des ennemis a la variable audioS
        audioS = GameObject.FindGameObjectWithTag("ennemi").GetComponent<AudioSource>();
    }
    /*
     * Fonction OnCollisionEnter. Gère ce qui se passe lorsqu'une balle touche un objet.
     */
    private void OnCollisionEnter(Collision infoCollisions)
    {
        /* --> Partie à compléter
         * 1. Création d'une copie de l'objet de particules particulesContact
         * 2. On place l'objet copié au point de contact de la collison
         * 3. On active l'objet copié
         * 4. On oriente l'objet copié vers le personnage (LookAt)
         * 5. On applique une légère correction de position pour éviter que les particules se retouvent derrère l'objet
         * 5. On détruit l'objet copié (particules de fumée) après un délai d'une seconde.
         * 6. On détruit la balle (immédiatement)
         */
        GameObject fumeeHit = Instantiate(impactTir); // Effet particules impact de tir
        /* 
          * Information sur collision entre balle et objet et vient
          * chercher point de contact
          */
        fumeeHit.transform.position = infoCollisions.contacts[0].point; // detection collision pour hit
        fumeeHit.SetActive(true); // Activation effet Hit
        fumeeHit.transform.LookAt(personnage.transform.position); // Endroit ou effet est active
        Destroy(fumeeHit, 1); // Destruction effet de particules
        Destroy(gameObject); // Destruction de la balle

        /*
         * Si le joueur touche un Hellephant...
         -----------------------------------------*/
        if (infoCollisions.gameObject.name == "Hellephant(Clone)")
        {
            //La vie de l'Hellephant descend et son son joue
            infoCollisions.gameObject.GetComponent<ennemis>().vieHelle--;
            audioS.PlayOneShot(sonBlesseHelle, 1f);

            // Si la vie de l'Hellephant tombe a 0
            if (infoCollisions.gameObject.GetComponent<ennemis>().vieHelle == 0)
            {
                // On augmente le score de 5 points
                DeplacementPersoScript.pointage += 5;
            }
        }
        

        /*
         * Si le joueur touche un Zombunny...
         -----------------------------------------*/
        if (infoCollisions.gameObject.name == "Zombunny(Clone)")
        {
            //La vie du Zombunny descend et son son joue
            infoCollisions.gameObject.GetComponent<ennemis>().vieZombun--;
            audioS.PlayOneShot(sonBlesseZombun, 1f);

            // Si la vie du Zombunny tombe a 0
            if (infoCollisions.gameObject.GetComponent<ennemis>().vieZombun == 0)
            {
                // On augmente le score de 5 points
                DeplacementPersoScript.pointage += 2;
            }
        }


        /*
         * Si le joueur touche un Zombear...
         -----------------------------------------------*/
        if (infoCollisions.gameObject.name == "ZomBear(Clone)")
        {
            //La vie du Zombear descend et son son joue
            infoCollisions.gameObject.GetComponent<ennemis>().vieZombear--;
            audioS.PlayOneShot(sonBlesseZombear, 1f);

            // Si la vie du Zombear tombe a 0
            if (infoCollisions.gameObject.GetComponent<ennemis>().vieZombear == 0)
            {
                // On augmente le score de 1 points
                DeplacementPersoScript.pointage += 1;
            }
        }

    }
}
