using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemis : MonoBehaviour
{
    /*--- References aux composants ---*/
    // Reference au nav mesh agent
    NavMeshAgent navigateur;
    // Reference a l'animator
    Animator animateur;
    // Reference a l'audiosource
    AudioSource sons;
    // Les particules de mort des ennemis
    [SerializeField] GameObject particules;

    /*--- Variables ---*/
    // Reference au joueur
    [SerializeField] GameObject joueur;
    // Les vies des differents ennemis
    public int vieHelle = 5;
    public int vieZombun = 2;
    public int vieZombear = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Association de la reference au nav mesh agent au GetComponent approprie
        navigateur = GetComponent<NavMeshAgent>();
        // Association de la reference a l'Animator au GetComponent approprie
        animateur = GetComponent<Animator>();
        // Association de la reference au AudioSource au GetComponent approprie
        sons = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (!DeplacementPersoScript.partieFinie)
        {
            // Si la vie des ennemis tombe a 0...
            // On invoque la fonction de mort de l'ennemi en question
            if (vieHelle == 0)
            {
                Invoke("Touche", 0f);
                vieHelle = -1;
            }

            if (vieZombear == 0)
            {
                Invoke("Touche", 0f);
                vieZombear = -1;
            }

            if (vieZombun == 0)
            {
                Invoke("Touche", 0f);
                vieZombun = -1;
            }
        }
    }

    void FixedUpdate()
    {
        if (!DeplacementPersoScript.partieFinie)
        {
            // L'animation pour faire bouger les ennemis est mise a false
            animateur.SetBool("bouge", false);

            // Les ennemis se dirrigent vers le joueur
            navigateur.SetDestination(joueur.transform.position);

            // Si la velocite d'un ennemi est superieure a 0...
            if (navigateur.velocity.magnitude > 0)
            {
                // L'animation pour faire bouger les ennemis est mise a true
                animateur.SetBool("bouge", true);
            }
        }
        else
        {
            // L'animation de mouvement est mise a false et donc celle de idle joue
            animateur.SetBool("bouge", false);
            // La position des ennemis ne s'update plus par rapport a celle du joueur
            navigateur.SetDestination(transform.position);
        }
    }

    // Fonction activee quand un ennemi est touche
    private void Touche()
    {
        // Le son e mort joue
        sons.Play();
        // Son animation de mort joue
        animateur.SetBool("mort", true);
        // L'ennemi arrete de bouger
        navigateur.speed = 0;
        // On active la fonction de mort de l'ennemi et on lui enleve le tag ennemi
        gameObject.tag = "Untagged";

        // On lance la coroutine pour detruire le corps
        StartCoroutine(DestructionEnnemi());
    }


    /*-----------------
     !! IENUMERATORS !!
     -----------------*/
    // Coroutine pour la destruction de l'ennemi une fois qu'il est mort
    private IEnumerator DestructionEnnemi()
    {
        // Apres 2 secondes le corps st detruit
        yield return new WaitForSeconds(2f);
        // On active le systeme de particules 
        transform.Find("DeathParticles").gameObject.SetActive(true);
        // On deparente le systeme de particule de l'ennemi
        transform.Find("DeathParticles").parent = null;
        // On detruit l'ennemi
        Destroy(gameObject);
        // On fait disparaitre le systeme de particules apres 2 secondes
        particules.GetComponent<mortParticules>().Disparait();
    }
}