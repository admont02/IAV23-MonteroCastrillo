using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesa : MonoBehaviour
{
    //bool mesa vacia
    public bool libre = true;
   
    //Esta la mesa sucia?
    public bool IsSucia()
    {
        return transform.childCount > 0;
    }
}
