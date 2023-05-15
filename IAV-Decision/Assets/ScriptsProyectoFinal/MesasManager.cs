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

        Vector3 posi = new Vector3();
        int i = 0;
        while (i < transform.childCount)
        {
            Transform hijo = transform.GetChild(i);
            if (hijo.GetComponent<Mesa>().libre)
            {
                posi.x = hijo.position.x + 1;
                posi.y = hijo.position.y;
                posi.z = hijo.position.z;
                hijo.GetComponent<Mesa>().libre = false;
                break; // Salir del bucle while cuando se encuentra una mesa libre
            }
            i++;
        }
        return posi;
    }
    public Mesa GetMesaVacia()
    {
        Mesa mesaVacia = null;
        int i = 0;
        while (i < transform.childCount)
        {
            Transform hijo = transform.GetChild(i);
            Mesa mesa = hijo.GetComponent<Mesa>();
            if (mesa != null && mesa.libre)
            {
                mesaVacia = mesa;
                mesaVacia.libre = false;
                break; // Salir del bucle while cuando se encuentra una mesa libre
            }
            i++;
        }
        return mesaVacia;
    }

}
