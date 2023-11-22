using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gestionIntro : MonoBehaviour
{
    // Les instructions de l'intro
    [SerializeField] TextMeshProUGUI instructions;
    // La couleur du texte
    Color controleAlpha;
    // Les variables necesaire au clignotement du texte dans la page d'intro
    private float compteur;
    private float tempsMax;
    private bool compteurRepart;

    private void Start()
    {
        // le controleAlpha est associe a la couleur du texte d'instructions
        controleAlpha = instructions.GetComponent<TextMeshProUGUI>().color;
        // L'alpha est a 0 en partant
        controleAlpha.a = 0f;
        // le temps maximal du compteur est de 0.5f
        tempsMax = 0.5f;
        // le compteur ne repart pas encore
        compteurRepart = false;
    }

    void Update()
    {
        /*
         * Gestion du changement de scene
         -------------------------------------*/
        // Si le joueur appuie sur espace...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // On lance la scene de jeu
            SceneManager.LoadScene("SceneDebut");
        }

        /*
         * Gestion du changement d'opacitee du texte d'intro
         ------------------------------------------------------*/
        // Le comtpeur avance avec le temps
        compteur += Time.deltaTime;
        // Si le compteur ne repart pas et que le temps est plus grand que le temps maximal... 
        if (!compteurRepart)
        {
            if (compteur >= tempsMax)
            {
                // L'alpha de la couleur passe a 0f
                controleAlpha.a = 0f;
                /* on associe le GetComponenent des instructions au controleAlpha
                 * pour l'update lie a la couleur 
                 */
                instructions.GetComponent<TextMeshProUGUI>().color = controleAlpha;
                // On invoque la fonction qui repart le compteur
                Invoke("RelancerCompteur", 0.3f);
            }
        }
        // Si le compteur repart...
        else
        {
            // L'alpha reprend son opacite de base
            controleAlpha.a = 0.2f;
            // On passe ses valeurs au texte d'instructions
            instructions.GetComponent<TextMeshProUGUI>().color = controleAlpha;
            // Le booleen du compteur est mis a false
            compteurRepart = false;
        }
    }

    /*----------------------------
     * FONCTIONS SUPPLEMENTAIRES *
     ----------------------------*/
    private void RelancerCompteur()
    {
        // Le compteur est mis a 0f
        compteur = 0f;
        // Le booleen est mis a true
        compteurRepart = true;
    } 
}
