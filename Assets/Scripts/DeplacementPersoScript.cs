using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeplacementPersoScript : MonoBehaviour
{

    /*#################################################
     -- variables publiques à définir dans l'inspecteur
    #################################################*/
    public GameObject cameraPerso; //la caméra qui doit suivre le perso. À définir dans l'inspecteur
    public Vector3 distanceCamera; // la distance à laquelle la caméra doit suivre le perso.
    public float vitesseDeplacementPerso; // vitesse de déplacement du personnage
    public float vitesseRotationPerso;// vitesse de rotation du personnage lorsque la souris se déplace horizontalement
    public bool curseurLock; // On vérouille ou non le curseur.
    public static int pointage = 0; // Le poinatge du joueur
    [SerializeField] int vie; // Le nombre de vie du joueur
    public static bool Blesse; // Variable qui determine si le joueur est blesse
    public static bool partieFinie = false; // Variable qui determine sila partie est finie

    private float compteurFlicker = 0f; // Le compteur pour determiner le temps de clignotement du personnage
    private float compteurFlickerMax = 0.3f; // Le temps maximal du compteur ci-haut

    Rigidbody rb; // Variable du rigidbody
    Animator anim; // Variable de l'animator
    AudioSource audiosource; // Variable de l'audiosource

    public Image[] vies; // Les vies (visuelles) du joueur
    public TextMeshProUGUI points; // Les points visibles sur l'ecran de jeu

    public AudioClip mort;
    public AudioClip ouch;


    void Start()
    {
        // Active le verrouillage du curseur seulement si l'option est cochée. Utilie seulement avec la caméra simple "rotate".
        if (curseurLock) Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>(); // Variable rb est associee au GetComponent du rigidbody
        anim = GetComponent<Animator>(); // Variable anim associee au GetComponent de l'animator
        audiosource = GetComponent<AudioSource>();

        pointage = 0; // le pointage est a 0 au debut du jeu
        vie = 3; // Le joueur a 3 vies
        Blesse = false; // Le joueur n'a pas encore ete Blesse
    }

    private void Update()
    {
        // Le pointage est incremente dans la zone de pointage du canvas
        points.text = pointage.ToString();

        /*
         * Mort du joueur
         --------------------------------*/
        // Si la vie du joueur tombe a 0...
        if (vie == 0)
        {
            // Le booleen pour la fin de la partie est a true
            partieFinie = true;
            // L'animation de mort joue
            anim.SetBool("mort", true);
            // On empeche les mouvements du perso a sa mort
            rb.constraints = RigidbodyConstraints.FreezePosition;
            // On appelle la fonction qui change la scene pour la scene de fin de partie
            Invoke("Mort", 3f);
        }

        // Si le personnage est blesse et que la partie n'est pas finie...
        if (Blesse && !partieFinie)
        {
            compteurFlicker += Time.deltaTime;

            /* 
             * Si le temps est plus petit ou egal au temps maximum, le mesh du joueur et de son arme disparaissent.
             * Sinon ils reapparaissent et on invoque la fonction pour repartir le compteur apres 0.2 secondes.
             */
            if (compteurFlicker <= compteurFlickerMax)
            {
                gameObject.transform.Find("Gun").GetComponent<SkinnedMeshRenderer>().enabled = false;
                gameObject.transform.Find("Player").GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            else
            {
                gameObject.transform.Find("Gun").GetComponent<SkinnedMeshRenderer>().enabled = true;
                gameObject.transform.Find("Player").GetComponent<SkinnedMeshRenderer>().enabled = true;
                Invoke("RepartCompteur", 0.2f);
            }
        }
    }

    /*
     * Fonction FixeUpdate pour le déplacement du perso, la gestion des animations du perso et l'ajustement de la 
     * position et de la rotation de la caméra
     */
    void FixedUpdate()
    {
        if (!partieFinie)
        {
            /* ### déplacement du perso ###
            On commence par récupérer les valeurs de l'axe vertical et de l'axe horizontal. 
            GetAxisRaw renvoie une valeur soit de -1, 0 ou 1. Aucune progression comme avec GetAxis.*/
            float axeH = Input.GetAxisRaw("Horizontal");
            float axeV = Input.GetAxisRaw("Vertical");
            /*
             **** déplacement du personnage --> partie à compléter ****
             *
            On modifie la vélocité du personnage en lui donnant un nouveau vector 3 composé de la valeur des axes vertical et
            horizontal. Ce vecteur doit être normalisé (pour éviter que le personnage se déplace plus vite en diagonale.
            On multiplie ce vecteur par la variable vitesseDeplacementPerso pour pouvoir ajuste la vitesse de déplacement.*/

            rb.velocity = new Vector3(axeH, 0, axeV).normalized * vitesseDeplacementPerso; /* Normaliser vecteur pour empecher
                                                                                         acceleration en diagonale */

            //----------------------------------------------------------------------------------------------

            /* ### rotation du personnage simple ###
             * on tourne le personnage en fonctione du déplacement horizontal de la souris. On mutliplie par la variable
             * vitesseRotationPerso pour pouvoir contrôler la vitesse de rotation*/
            //  float tourne = Input.GetAxis("Mouse X") * vitesseRotationPerso;
            //  transform.Rotate(0f, tourne, 0f);

            /* ### rotation du personnage complexe, mais plus précise pour le tir. Activer cette fonction pour qu'elle s'exécute
             * et mettre en commentaire la rotation simple.*/
            TournePersonnage();

            //----------------------------------------------------------------------------------------------

            /* 
             **** gestion des animations --> partie à compléter ****
             *
             * Activation de l'animation de marche si la magnitude de la vélocité est plus grande que 0. Si ce n'est pas le cas
             * on active l'animation de repos. GetComponent<Rigidbody>().velocity.magnitude...
             * 
            */
            anim.SetBool("marche", false); // Toujours a false pour eviter de mettre trop de conditions

            // Si la magnitude augmente, le perso marche
            if (rb.velocity.magnitude > 0f)
            {
                anim.SetBool("marche", true);
            }

            //----------------------------------------------------------------------------------------------

            /* positionnement de la caméra qui suit le joueur. On place la caméra à la position actuelle du joueur en ajoutant
             * une distance (variable distanceCamera). On fait aussi un LookAt pour s'assurer que la caméra regarde vers le joueur*/
            cameraPerso.transform.position = transform.position + distanceCamera;
            cameraPerso.transform.LookAt(transform.position);

            //----------------------------------------------------------------------------------------------
        }
    }

    /*
     * Detection des collisions
     -----------------------------------------------------------*/
    private void OnCollisionEnter(Collision infoCollisions)
    {
        // Si le joueur n'est pas blesse et si un ennemi avec le tag "ennemi" touche le joueur...
        if (!Blesse)
        {
            if (infoCollisions.gameObject.tag == "ennemi")
            {
                // La vie du joueur descend
                vie--;
                // Switch... case... Statement pour faire disparaitre les images des vies
                switch (vie)
                {
                    case 0:
                        vies[0].enabled = false;
                        // Le son de mort du joueur joue
                        audiosource.PlayOneShot(mort, 1f);
                        break;
                    case 1:
                         vies[1].enabled = false;
                         // Le son du personnage blesse joue
                         audiosource.PlayOneShot(ouch, 1f);
                        break; 
                    case 2:
                         vies[2].enabled = false;
                         // Le son du personnage blesse joue
                         audiosource.PlayOneShot(ouch, 1f);
                        break;
                }
               /* 
                * Le personnage devient blesse et invulnerable pendant 3 secondes.
                * Apres 3 secondes, on invoque la fonction qui rend le personnage a nouveau
                * vulnerable
                */
                Invoke("Invulnerable", 0f);
                Invoke("Vulnerable", 3f);
            }
        }
    }

    /*----------------------------
     * FONCTIONS SUPPLEMENTAIRES *
     ----------------------------*/
    /*
     * Fonction TournePersonnage qui permet de faire pivoter le personnage en fonction de la position de la caméra et du curseur
     * de la souris.
     */
    void TournePersonnage()
    {
        /*crée un rayon à partir de la caméra vers l’avant à la position de la souris. Le rayon est mémorisé dans la variable
         * locale camRay (variable de type Ray)*/
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); // .main shortcut, necessaire d'avoir tag mainCam sur la cam

        // variable locale infoCollision : contiendra les infos retournées par le Raycast sur l’objet touché 
        RaycastHit infoCollision;

        /* lance un rayon de 5000 unités à partir du rayon crée précédemment, vérifie seulement la collision avec le plancher en
         * spécifiant un LayerMask. Le plancher doit avoir un layerMask (exemple:“Plancher”) assigné dans l’inspecteur.
         * La commande RayCast renvoie True ou False (true si le plancher est touché par le rayon dans ce cas). Il est donc possible
         * de l'utiliser dans un if.
         * 
         * Dans l'ordre, les paramètres du RayCast sont :
         * 1- le point d'origine du rayon
         * 2- la direction dans lequel le rayon doit être tracé.
         * 3- la variable qui récoltera les informations s'il y a un contact du rayon. Ne pas oublier le mot clé "out".
         * 4- la longueur du rayon tracé
         * 5- le layerMask qui permet de tenir compte seulement des objets qui sont sur ce layer.*/

        if (Physics.Raycast(camRay.origin, camRay.direction, out infoCollision, 5000, LayerMask.GetMask("plancher")))
        {
            // si le rayon frappe le plancher...
            // le personnage regarde vers le point de contact (là ou le rayon à touché le plancher)
            transform.LookAt(infoCollision.point);

            /* Ici, on s'assure que le personnage tourne seulement sur son Axe Y en mettant ses rotations X et Z à 0. Pour changer
             * ces valeurs par programmation, il faut changer la propriété localEuleurAngles.*/
            Vector3 rotationActuelle = transform.localEulerAngles;
            rotationActuelle.x = 0f;
            rotationActuelle.z = 0f;
            transform.localEulerAngles = rotationActuelle;
        }
        //outil de déboggage pour visualiser le rayon dans l'onglet scene
        Debug.DrawRay(camRay.origin, camRay.direction * 100, Color.yellow);
    }

    // L'invulnerabilitee apres avoir recut un coup
    void Invulnerable()
    {
        Blesse = true;
    }
    // Pour repartir le compteur 
    void RepartCompteur()
    {
        compteurFlicker = 0f;
    }
    // Le personnage redevient vulnerable et reste visible
    void Vulnerable()
    {
        Blesse = false;
        gameObject.transform.Find("Gun").GetComponent<SkinnedMeshRenderer>().enabled = true;
        gameObject.transform.Find("Player").GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    // Fonction qui relance le jeu a la mort du joueur (lorsque la partie est finie)
    void Mort()
    {
        if (partieFinie)
        {
            SceneManager.LoadScene("fin");
        }
    }
}
