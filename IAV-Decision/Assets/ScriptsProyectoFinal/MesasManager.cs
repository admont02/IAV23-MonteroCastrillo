using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesasManager : MonoBehaviour
{
    public bool hayMesaVacia()
    {
        bool vacia = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform hijo = transform.GetChild(i);
            if (hijo.GetComponent<Mesa>().libre) vacia = true;


        }
        return vacia;

    }
    public Vector3 getMesaVacia()
    {

        Vector3 posi=new Vector3();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform hijo = transform.GetChild(i);
            if (hijo.GetComponent<Mesa>().libre)
            {
                posi = hijo.position;

            }

        }
        return posi;
    }
}
