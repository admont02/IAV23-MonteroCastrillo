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
        //Debug.Log(temporizador);
        // Debug.Log(temporizador);
        if (temporizador >= intervaloCreacion && listaClientes.childCount<maxCli)
        {
           
            CrearCliente();
            temporizador = 0f;
        }
    }

    void CrearCliente()
    {
        // Lógica para crear un nuevo cliente
        GameObject cliente = Instantiate(clientePrefab, transform.position, transform.rotation);
        cliente.transform.SetParent(listaClientes);
        Debug.Log("Nuevo cliente");
        // Asignar propiedades aleatorias al cliente
       // cliente.GetComponent<Cliente>().AsignarPropiedadesAleatorias();
    }
    public int GetClientesBarra()
    {
        int cont = 0;
        for(int i = 0; i < listaClientes.childCount; i++)
        {
            Transform hijo = transform.GetChild(i);
            if (hijo.GetComponent<Cliente>().esperando)
                cont++;
        }
        return cont;
    }
    public bool HayHuecoEnBarra()
    {
        return GetClientesBarra() < maxCliBarra;
    }
}
