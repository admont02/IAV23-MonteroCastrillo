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
    public void Awake()
    {
        barraGO = GameObject.FindWithTag("Barra");
        puertaGO = GameObject.FindWithTag("Puerta");

        Barra = barraGO.transform;
        Puerta = puertaGO.transform;
        agente = GetComponent<NavMeshAgent>();
        Debug.Log(Barra.position.x);
        Debug.Log(Barra.position.y);
        Debug.Log(Barra.position.z);
        int randomIndex = Random.Range(0, sprites.Length);
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
        //objetivo = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //fantasma = GameObject.FindGameObjectWithTag("Ghost");
    }
    private void BebidaDeseada()
    {

    }
    public void Start()
    {
        //agente.updateRotation = false;


    }

    public void LateUpdate()
    {
        if (agente.velocity.sqrMagnitude > Mathf.Epsilon && !enMesa)
            transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);
    }

    // Comienza a cantar, reseteando el temporizador



    // Comienza a descansar, reseteando el temporizador
    public void IrABarra()
    {
        agente.SetDestination(Barra.position);
        tiempoComienzoDescanso = 0;
        esperando = true;
    }

    // Comprueba si tiene que dejar de descansar
    public bool TerminaEsperarEnBarra()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);

        if ((1 << NavMesh.GetAreaFromName("Barra") & hit.mask) != 0)
            tiempoComienzoDescanso += Time.time;

        return tiempoComienzoDescanso >= tiempoDeDescanso;
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

    //Mira si ve al vizconde con un angulo de vision y una distancia maxima


    // Genera una posicion aleatoria a cierta distancia dentro de las areas permitidas
    private Vector3 RandomNavSphere(float distance)
    {
        Vector3 dir = UnityEngine.Random.insideUnitSphere * distance;
        dir += transform.position;
        NavMeshHit hit;
        do
        {
            dir = UnityEngine.Random.insideUnitSphere * distance;
            dir += transform.position;
            NavMesh.SamplePosition(dir, out hit, distance, NavMesh.AllAreas);
        }
        while ((1 << NavMesh.GetAreaFromName("Escenario") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Palco Este") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Palco Oeste") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Butacas") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Vestíbulo") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Bambalinas") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Sótano Este") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Sótano Oeste") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Celda") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Sótano Norte") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Música") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Pasillos Escenario") & hit.mask) == 0);

        return hit.position;
    }

    // Genera un nuevo punto de merodeo cada vez que agota su tiempo de merodeo actual




    public void setAtencion()
    {
        esperando = false;
        esperandoBebida = true;
        icono.SetActive(true);
    }
    public void MandarAMesa(Vector3 dest)
    {
        Debug.Log("MandarAMesa metodo");
        esperandoBebida = false;
        enMesa = true;
        agente.SetDestination(dest);
        tiempoComienzoConsumo = 0;
        //  transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        //transform.rotation = Quaternion.Euler(0, -90, 0);
    }
}
