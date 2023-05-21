using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesasManager : MonoBehaviour
{
    //Hay alguna mesa vacia?
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
    //Obtener mesa vacia
  
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
