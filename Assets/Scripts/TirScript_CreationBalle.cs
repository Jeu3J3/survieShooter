using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirScript_CreationBalle : MonoBehaviour
{
    /*#################################################
   -- variables publiques à définir dans l'inspecteur
   #################################################*/
    public GameObject balle; // Référence au gameObject de la balle
    public GameObject particuleBalle; // Référence au gameObject à activer lorsque le personnage tir
    public float vitesseBalle; // Vitesse de la balle
    public float delaiTir; // delai de tir de 0.1f

    public AudioClip sonTir; // Son lorsque le personnage tire

    /*#################################################
   -- variables privées
   #################################################*/
    private bool peutTirer; // Est-ce que le personnage peut tirer

    AudioSource son;


    //----------------------------------------------------------------------------------------------
    void Start()
    {
        peutTirer = true; // Au départ, on veut que le personnage puisse tirer

        son = GetComponent<AudioSource>();
    }
    //----------------------------------------------------------------------------------------------


    /*
     * Fonction Update. On appele la fonction Tir() lorsque la  bouton de gauche de la souris 
     * est enfoncée et que le personnage peut tirer
     */
    void Update()
    {
        if (!DeplacementPersoScript.partieFinie)
        {
            // --> partie à compléter ****
            if (Input.GetMouseButtonDown(0) && peutTirer)
            {
                Tir();
            }
        }
    }
    //----------------------------------------------------------------------------------------------


    /*
     * Fonction Tir. Gère le tir d'une nouvelle balle.
     */
    void Tir()
    {
        /* On désactive la capacité de tirer et on appelle la fonction ActiveTir() après
         un délai de 0.1 seconde */
        peutTirer = false;
        Invoke("ActiveTir", delaiTir);

        /* --> partie à compléter...
         * 1. activation de la particuleBalle
         * 2. activation du son "Player GunShot". Que devez-vous ajouter au personnage pour qu'il puisse jouer un son?
         * 3. Création d'une copie de la balle à partir de la balle originale. La position et la rotation du clône
         * doivent être les mêmes que la balle originale.
         * 4. On active le clône. La balle originale doit rester désactivée.
         * 5. On applique une vélocité au clône. (velocité = transform.forward * vitesseBalle)
         * */

        particuleBalle.SetActive(true); // Activer la balle
        son.PlayOneShot(sonTir); // Faire jouer le son lors du tir

        GameObject NouvelleBalle = Instantiate(balle); // Copier la balle
        /* 
         * La position de la balle instantiee est la meme que la balle originale
         */
        NouvelleBalle.transform.position = balle.transform.position;
        NouvelleBalle.transform.rotation = balle.transform.rotation;
        NouvelleBalle.SetActive(true); // activation de la nouvelle balle
        NouvelleBalle.GetComponent<Rigidbody>().velocity = transform.forward * vitesseBalle;


    }
    //----------------------------------------------------------------------------------------------


    /*
     * Fonction ActiveTir(). Réactive la capacité de tirer.
     */

        void ActiveTir()
    {
        /* --> partie à compléter...
         * 1. On réactive la capacité de tirer... variable peutTirer...
         * 2. On désactive la particule particuleBalle
         * */
        peutTirer = true; // Le perso peut tirer a nouveau
        particuleBalle.SetActive(false); // L'effet de particules est desactive
    }
}
