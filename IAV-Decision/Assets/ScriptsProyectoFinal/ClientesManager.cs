using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientesManager : MonoBehaviour
{
    [SerializeField]
    GameObject clientePrefab;
    [SerializeField]
    Transform listaClientes;
    public float intervaloCreacion = 5f;
    private float temporizador = 0f;

    void Update()
    {
        temporizador += Time.deltaTime;
       // Debug.Log(temporizador);
        if (temporizador >= intervaloCreacion)
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
}
