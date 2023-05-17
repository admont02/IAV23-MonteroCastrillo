using UnityEngine;
using UnityEngine.AI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform playerTransform;
    [SerializeField]
    GameObject listaClientes;
    [SerializeField]
    MesasManager mM;
    [SerializeField]
    GameObject PosCerveza;
    [SerializeField]
    GameObject PosVino;
    [SerializeField]
    GameObject PosWhiskey;

    //prefabs
    [SerializeField]
    GameObject PrefabCerveza;
    [SerializeField]
    GameObject PrefabVino;
    [SerializeField]
    GameObject PrefabWhiskey;

    bool bebidaEnMano = false;


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
        {
            InteraccionConClientes();
            CogerBebidaDeEstanteria();

        }

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
                Debug.Log("El hijo " + i + " est� cerca del objeto actual.");
                hijo.GetComponent<Cliente>().setAtencion();
                break;
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
                    Mesa mesa = mM.GetMesaVacia();
                    Vector3 posi;
                    posi.x = mesa.transform.position.x + 1;
                    posi.y = mesa.transform.position.y;
                    posi.z = mesa.transform.position.z;
                    hijo.GetComponent<Cliente>().miMesa = mesa;
                    hijo.GetComponent<Cliente>().MandarAMesa(posi);
                    break;
                }
            }

        }

    }


    private void CogerBebidaDeEstanteria()
    {
        if (bebidaEnMano) return;
        float distCerve = Vector3.Distance(transform.position, PosCerveza.transform.position);
        float distVino = Vector3.Distance(transform.position, PosVino.transform.position);
        float distWhiskey = Vector3.Distance(transform.position, PosWhiskey.transform.position);

        Vector3 posi;
        posi.x = -0.5f;
        posi.y = 0.1f;
        posi.z = 0.7f;

        // Comprobar si la distancia es menor que una cierta cantidad
        if (distCerve <= 1.5f)
        {
            Debug.Log("Estanter�a cerveza");
            GameObject cerveza = Instantiate(PrefabCerveza, transform.position, transform.rotation);
            cerveza.transform.SetParent(transform);
            cerveza.transform.localPosition= posi;
            bebidaEnMano = true;
        }
        else if (distVino <= 1.5f)
        {
            Debug.Log("Estanter�a Vino ");
            GameObject vino = Instantiate(PrefabVino, posi, transform.rotation);
            vino.transform.SetParent(transform);
            vino.transform.localPosition = posi;
            bebidaEnMano = true;
        }
        else if (distWhiskey <= 1.5f)
        {
            Debug.Log("Estanter�a Whiskey");
            GameObject whiskey = Instantiate(PrefabWhiskey, posi, transform.rotation);
            whiskey.transform.SetParent(transform);
            whiskey.transform.localPosition = posi;
            bebidaEnMano = true;
        }
    }
}
