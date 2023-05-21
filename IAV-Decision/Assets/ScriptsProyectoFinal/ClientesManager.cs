using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientesManager : MonoBehaviour
{
    [SerializeField]
    GameObject clientePrefab;
    [SerializeField]
    Transform listaClientes;
    public float intervaloCreacion = 450f;
    private float temporizador = 0f;
    private int maxCli=8;
    private int maxCliBarra = 3;


    void Update()
    {
        temporizador += Time.deltaTime;
      
        if (temporizador >= intervaloCreacion && listaClientes.childCount<maxCli)
        {
           
            CrearCliente();
            temporizador = 0f;
        }
    }
    //Creación de clientes
    void CrearCliente()
    {
        // Lógica para crear un nuevo cliente
        GameObject cliente = Instantiate(clientePrefab, transform.position, transform.rotation);
        cliente.transform.SetParent(listaClientes);
        Debug.Log("Nuevo cliente");
       
    }
    //Obtener clientes en la barra
    public int GetClientesBarra()
    {
        int cont = 0;
        for(int i = 0; i < listaClientes.childCount; i++)
        {
            Transform hijo = transform.GetChild(i);
            if (hijo.GetComponent<Cliente>().esperando || hijo.GetComponent<Cliente>().esperandoBebida)
                cont++;
        }
        return cont;
    }
    //HUeco en barra
    public bool HayHuecoEnBarra()
    {
        return GetClientesBarra() < maxCliBarra;
    }
}
