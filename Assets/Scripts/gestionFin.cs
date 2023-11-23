using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Reflection;
using UnityEngine.SceneManagement;

public class gestionFin : MonoBehaviour
{
    /*-------------
     *  VARIABLES *
     -------------*/
    float points = DeplacementPersoScript.pointage; // Le pointage du script du personnage
    public TextMeshProUGUI pointageText; // Le texte affichant le pointage

    public TextMeshProUGUI instructionsFin; // Le texte des instructions de la fin
    private float compteur; // Le compteur pour faire clignoter le texte
    private float tempsMax; // Le temps maximal du compteur
    private bool compteurRepart; // La variable determinant si le compteur repart
    Color controleAlpha; // La couleur du texte (plus precisement le controle de son opacitee)

    void Start()
    {
        // Le controleAlpha est associe a la couleur du texte d'instructions de la fin
        controleAlpha = instructionsFin.GetComponent<TextMeshProUGUI>().color;
        // L'alpha du texte est mis a 0
        controleAlpha.a = 0f;
        // le temps maximal du compteur est mis a 0.5
        tempsMax = 0.5f;
        // Le compteur n'est pas reparti au depart 
        compteurRepart = false;
    }

    void Update()
    {
     /*
      * Gestion du texte affichant les points
      -------------------------------------------------------------------------------------------------*/
      // On met la valeur du pointage dans un message qui apparaitra dans la scene de fin 
      pointageText.text = "tu as " + points.ToString() + " points.";

        /*
         * Gestion du texte de fin
         -------------------------------------------------------------------------------------------------*/
        // Le compteur augmente avec le temps pour chaque update
        compteur += Time.deltaTime;
        // Si le compteur est pas reparti
        if (!compteurRepart) 
        {
            // Si la valeur du compteur est plus petite ou egale au temps maximal du compteur...
            if (compteur >= tempsMax)
            {
                // L'opacitee du texte passe a 0f
                controleAlpha.a = 0f;
                // On passe la valeur du controleAlpha au texte d'instructions de la fin
                instructionsFin.GetComponent<TextMeshProUGUI>().color = controleAlpha;
                // On invoque la fonction relancant le compteur
                Invoke("RelanceCompteur", 0.3f);
            }
        }
        else
        {
            // L'opacitee du texte passe a 0.2f
            controleAlpha.a = 0.2f;
            // On passe les valeurs du controleAlpha au texte d'instructions de la fin
            instructionsFin.GetComponent<TextMeshProUGUI>().color = controleAlpha;
            // La variable confirmant que le compteur repart, revient a false
            compteurRepart = false;
        }

        /*
         * Pour recommencer le jeu
         ---------------------------------------------------------------------------------*/
        // Si le joueur appuie sur espace...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // On appelle la fonction qui relance le jeu
            Invoke("Reload", 0f);
        }
    }
    /*-----------------------------
     *  FONCTIONS SUPPLEMENTAIRES *
     -----------------------------*/

    void RelanceCompteur()
    {
        /* 
         * Le compteur recommence a partir de 0 et
         * la variable booleene confirmant que le compteur
         * repart passe a true.
         */
        compteur = 0f;
        compteurRepart = true;
    }
    
    void Reload()
    {
        /*
         * Le jeu est relance et on remet le pointage du joueur a 0
         * et la variable booleene confirmant la fin de la partie est
         * mise a false.
         */
        points = 0f;
        DeplacementPersoScript.partieFinie = false;
        SceneManager.LoadScene("SceneDebut");
    }
}
