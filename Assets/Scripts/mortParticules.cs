using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class mortParticules : MonoBehaviour 
{ 
    public void Disparait()
    {
        Destroy(gameObject, 2f);
    }
}
