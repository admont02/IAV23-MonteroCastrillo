using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Cliente : MonoBehaviour
{

    // Segundos que esta descanasando
    public double tiempoDeDescanso;
    // Segundo en el que comezo a descansar
    private double tiempoComienzoDescanso;
    // Si esta capturada


    // Segundos que puede estar merodeando
    public double tiempoDeEsperaBebida;
    // Segundo en el que comezo a merodear
    public double tiempoComienzoEsperarBebida = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;
    // Si canta o no
    public bool esperando = false;
    public bool esperandoBebida = false;
    public bool enMesa = false;
    public Mesa miMesa = null;
    public double tiempoDeConsumo;
    // Segundo en el que comezo a descansar
    private double tiempoComienzoConsumo = 0;




    // Componente cacheado NavMeshAgent
    private NavMeshAgent agente;

    // Objetivos de su itinerario
    public Transform Puerta;
    public Transform Barra;
    private GameObject barraGO;
    private GameObject puertaGO;


    // La blackboard
    public GameBlackboard bb;

    //para seguir al fantasma o al vizconde
    public GameObject icono;

    //sprites 

    public Sprite[] sprites;

    public int nivelAlegria;

    public enum Bebidas
    {
        CERVEZA,
        VINO,
        WHISKEY
    }
    Bebidas bebida;
    public void Awake()
    {
        barraGO = GameObject.FindWithTag("Barra");
        puertaGO = GameObject.FindWithTag("Puerta");

        Barra = barraGO.transform;
        Puerta = puertaGO.transform;
        agente = GetComponent<NavMeshAgent>();
        nivelAlegria = 100;
        BebidaDeseada();

    }
    private void BebidaDeseada()
    {

        int randomIndex = UnityEngine.Random.Range(0, sprites.Length);
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];

        bebida = (Bebidas)Enum.Parse(typeof(Bebidas), randomIndex.ToString());

    }
    public void Start()
    {
        //agente.updateRotation = false;
        agente.stoppingDistance = .25f;

    }

    public void LateUpdate()
    {
        if (agente.velocity.sqrMagnitude > Mathf.Epsilon && !enMesa)
            transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);


    }


    public void IrABarra()
    {
        agente.SetDestination(Barra.position);
        tiempoComienzoDescanso = Time.time;
        esperando = true;
    }

    // Comprueba si tiene que dejar de descansar
    public bool TerminaEsperarEnBarra()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            if ((1 << hit.mask & NavMesh.GetAreaFromName("Barra")) != 0)
            {
                double tiempoTranscurrido = Time.time - tiempoComienzoDescanso;

                return tiempoTranscurrido >= tiempoDeDescanso;
            }
        }

        // Si no se cumple la condición, se retorna false
        return false;
    }



    public bool TerminaConsumir()
    {

        if (enMesa)
        {
            if (tiempoComienzoConsumo == 0f) // Si es la primera vez que se llama a la función
            {
                tiempoComienzoConsumo = Time.time; // Guardar el tiempo de inicio del consumo
            }

            double tiempoTranscurrido = Time.time - tiempoComienzoConsumo; // Calcular el tiempo transcurrido

            if (tiempoTranscurrido >= tiempoDeConsumo) // Si ha transcurrido el tiempo objetivo
            {
                tiempoComienzoConsumo = 0f; // Reiniciar el tiempo de inicio del consumo para futuras llamadas
                miMesa.libre = true;
                return true;
            }
        }

        return false; // Si no se cumple la condición, devuelve false
    }


    public void setAtencion()
    {
        esperando = false;
        esperandoBebida = true;
        icono.SetActive(true);

        Invoke("DesactivarIcono", 3f);
    }

    private void DesactivarIcono()
    {
        icono.SetActive(false);
    }

    public void MandarAMesa(Vector3 dest, Bebidas entregada, GameObject vaso)
    {
        Vector3 posi;
        posi.x = -0.5f;
        posi.y = 0.1f;
        posi.z = 0.7f;
        if (bebida == entregada)
        {
            nivelAlegria += 15;
            Debug.Log("LO QUE QUERIA");

        }
        else
        {
            nivelAlegria -= 50;
            Debug.Log("nooooooooooooooooooooo  "+nivelAlegria);

        }
        vaso.transform.SetParent(transform);
        vaso.transform.localPosition = posi;
        Debug.Log("MandarAMesa metodo");
        esperandoBebida = false;
        enMesa = true;
        agente.SetDestination(dest);
        tiempoComienzoConsumo = 0;
        //  transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        //transform.rotation = Quaternion.Euler(0, -90, 0);
    }
}
