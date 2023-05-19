using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesa : MonoBehaviour
{
    public bool libre = true;
   

    public bool IsSucia()
    {
        return transform.childCount > 0;
    }
}
