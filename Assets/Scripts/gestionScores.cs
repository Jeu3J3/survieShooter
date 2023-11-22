using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionScores : MonoBehaviour
{
    public static string[] lstNoms = new string[6] {"", "", "", "", "", ""};
    public static int[] lstScores = new int[6] {0, 0, 0, 0, 0, 0};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void sauvegarde()
    {
        // Lecture
        lstNoms = PlayerPrefsX.GetStringArray("fichierNoms");
        lstScores = PlayerPrefsX.GetIntArray("fichierScores");

        // Ecriture
        PlayerPrefsX.SetStringArray("fichierNoms", lstNoms);
        PlayerPrefsX.SetIntArray("fichierScores", lstScores);
    }
}
