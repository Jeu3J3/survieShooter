using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionMusique : MonoBehaviour
{
    // Booleen static pour determiner si le dont destroy est fait ou non
    public static bool DontDestroyFait = false;

    void Start()
    {
        // Si le dont destroy n'est pas fait...
        if (!DontDestroyFait)
        {
            // Ne pas detruire le gestionnaire de musique
            DontDestroyOnLoad(gameObject);
            // le dont destroy est maintenant fait
            DontDestroyFait = true;
        }
        // Sinon...
        else
        {
            // Detruire le gameObject
            Destroy(gameObject);
        }
    }
}
