using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform playerTransform;
    [SerializeField]
    GameObject listaClientes;
    [SerializeField]
    MesasManager mM;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = transform;
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")).normalized;

        if (moveDirection != Vector3.zero)
        {
            agent.Move(moveDirection * Time.deltaTime * agent.speed);
            playerTransform.rotation = Quaternion.LookRotation(moveDirection);
        }
        if (Input.GetKeyDown(KeyCode.E))
            InteraccionConClientes();

        if (Input.GetKeyDown(KeyCode.F))
            MandarClienteAMesa();
    }
    private void InteraccionConClientes()
    {
        for (int i = 0; i < listaClientes.transform.childCount; i++)
        {
            Transform hijo = listaClientes.transform.GetChild(i);
            float distancia = Vector3.Distance(transform.position, hijo.position);

            // Comprobar si la distancia es menor que una cierta cantidad
            if (distancia <= 2.0f && hijo.GetComponent<Cliente>().esperando)
            {
                Debug.Log("El hijo " + i + " está cerca del objeto actual.");
                hijo.GetComponent<Cliente>().setAtencion();
            }

        }
    }
    private void MandarClienteAMesa()
    {
        for (int i = 0; i < listaClientes.transform.childCount; i++)
        {
            Transform hijo = listaClientes.transform.GetChild(i);
            float distancia = Vector3.Distance(transform.position, hijo.position);

            // Comprobar si la distancia es menor que una cierta cantidad
            if (distancia <= 2.0f && hijo.GetComponent<Cliente>().esperandoBebida)
            {
                if (mM.hayMesaVacia())
                {
                    hijo.GetComponent<Cliente>().MandarAMesa(mM.getMesaVacia());
                }
            }

        }
       
    }
}
