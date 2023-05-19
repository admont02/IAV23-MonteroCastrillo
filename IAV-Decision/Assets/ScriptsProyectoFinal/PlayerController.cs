using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform playerTransform;
   
   public GameObject listaClientes;
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

    Cliente.Bebidas b;
    private Animator animator;
    private GameObject bebidaObjeto;

    [SerializeField]
    AudioClip limpia;
    [SerializeField]
    AudioClip vasoSon;
    private AudioSource audioS;
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerTransform = transform;
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")).normalized;

        if (moveDirection != Vector3.zero)
        {
            agent.Move(moveDirection * Time.deltaTime * agent.speed);
            playerTransform.rotation = Quaternion.LookRotation(moveDirection);
            animator.Play("andar");
        }
        else animator.Play("idle");
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteraccionConClientes();
            CogerBebidaDeEstanteria();
            LimpiarMesa();

        }

        if (Input.GetKeyDown(KeyCode.F))
            MandarClienteAMesa();


    }

    private void LimpiarMesa()
    {
        for (int i = 0; i < mM.transform.childCount; i++)
        {
            Transform mesa = mM.transform.GetChild(i);
            float distancia = Vector3.Distance(transform.position, mesa.position);

            // Comprobar si la distancia es menor que una cierta cantidad
            if (distancia <= 2.0f && mesa.GetComponent<Mesa>().IsSucia())
            {
                for (int j = 0; j < mesa.transform.childCount; j++)
                {
                    Destroy(mesa.transform.GetChild(j).gameObject);
                    Debug.Log("Mesa limpia");
                    audioS.PlayOneShot(limpia);
                }

            }
        }
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
                break;
            }

        }
    }
    private void MandarClienteAMesa()
    {
        if (!bebidaEnMano) return;

        for (int i = 0; i < listaClientes.transform.childCount; i++)
        {
            Transform hijo = listaClientes.transform.GetChild(i);
            float distancia = Vector3.Distance(transform.position, hijo.position);

            // Comprobar si la distancia es menor que una cierta cantidad
            if (distancia <= 2.0f && hijo.GetComponent<Cliente>().esperandoBebida)
            {
                Vector3 posi = new Vector3(0,0,0);
                if (mM.hayMesaVacia())
                {

                    Mesa mesa = mM.GetMesaVacia();

                    posi.x = mesa.transform.position.x + 1;
                    posi.y = mesa.transform.position.y;
                    posi.z = mesa.transform.position.z;
                    hijo.GetComponent<Cliente>().miMesa = mesa;
                    hijo.GetComponent<Cliente>().MandarAMesa(posi, b, bebidaObjeto, true);
                    bebidaEnMano = false;
                    break;
                }
                else
                {
                    hijo.GetComponent<Cliente>().MandarAMesa(posi, b, bebidaObjeto, false);
                    bebidaEnMano = false;
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
        posi.y = 1.71f;
        posi.z = 0.318f;

        // Comprobar si la distancia es menor que una cierta cantidad
        if (distCerve <= 1.5f)
        {
            Debug.Log("Estantería cerveza");
            bebidaObjeto = Instantiate(PrefabCerveza, transform.position, transform.rotation);
            bebidaObjeto.transform.SetParent(transform);
            bebidaObjeto.transform.localPosition = posi;
            b = Cliente.Bebidas.CERVEZA;
            bebidaEnMano = true;
        }
        else if (distVino <= 1.5f)
        {
            Debug.Log("Estantería Vino ");
            bebidaObjeto = Instantiate(PrefabVino, posi, transform.rotation);
            bebidaObjeto.transform.SetParent(transform);
            bebidaObjeto.transform.localPosition = posi;
            bebidaObjeto.transform.rotation = Quaternion.Euler(-90f, bebidaObjeto.transform.rotation.eulerAngles.y, bebidaObjeto.transform.rotation.eulerAngles.z);

            b = Cliente.Bebidas.VINO;
            bebidaEnMano = true;
        }
        else if (distWhiskey <= 1.5f)
        {
            Debug.Log("Estantería Whiskey");
            bebidaObjeto = Instantiate(PrefabWhiskey, posi, transform.rotation);
            bebidaObjeto.transform.SetParent(transform);
            bebidaObjeto.transform.localPosition = posi;
            b = Cliente.Bebidas.WHISKEY;
            bebidaEnMano = true;
        }
        else return;
        audioS.PlayOneShot(vasoSon);
    }
   
}
