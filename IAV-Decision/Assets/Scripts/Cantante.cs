using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cantante : MonoBehaviour
{
    // Segundos que estara cantando
    public double tiempoDeCanto;
    // Segundo en el que comezo a cantar
    private double tiempoComienzoCanto;
    // Segundos que esta descanasando
    public double tiempoDeDescanso;
    // Segundo en el que comezo a descansar
    private double tiempoComienzoDescanso;
    // Si esta capturada
    public bool capturada = false;

    [Range(0, 180)]
    // Angulo de vision en horizontal
    public double anguloVistaHorizontal;
    // Distancia maxima de vision
    public double distanciaVista;
    // Objetivo al que ver"
    public Transform objetivo;

    // Segundos que puede estar merodeando
    public double tiempoDeMerodeo;
    // Segundo en el que comezo a merodear
    public double tiempoComienzoMerodeo = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;
    // Si canta o no
    public bool cantando = false;
   


    // Componente cacheado NavMeshAgent
    private NavMeshAgent agente;

    // Objetivos de su itinerario
    public Transform Escenario;
    public Transform Bambalinas;

    // La blackboard
    public GameBlackboard bb;

    //para seguir al fantasma o al vizconde
    public GameObject fantasma;

    public void Awake()
    {
        agente = GetComponent<NavMeshAgent>();

        objetivo = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        fantasma = GameObject.FindGameObjectWithTag("Ghost");
    }

    public void Start()
    {
        agente.updateRotation = false;
    }

    public void LateUpdate()
    {
        if (agente.velocity.sqrMagnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);
    }

    // Comienza a cantar, reseteando el temporizador
    public void Cantar()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);

        bool enEscenario = (1 << NavMesh.GetAreaFromName("Escenario") & hit.mask) != 0;

        if (!enEscenario)
        {
            agente.SetDestination(Escenario.position);
        }

        tiempoComienzoCanto = 0;
        cantando = true;
        GetComponent<AudioSource>().Play();
        Debug.Log("Hola!Estoy cantando!");
    }

    // Comprueba si tiene que dejar de cantar
    public bool TerminaCantar()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);
        if ((1 << NavMesh.GetAreaFromName("Escenario") & hit.mask) != 0)
            tiempoComienzoCanto += Time.deltaTime;

        return tiempoComienzoCanto >= tiempoDeCanto;
    }

    // Comienza a descansar, reseteando el temporizador
    public void Descansar()
    {
        agente.SetDestination(Bambalinas.position);
        tiempoComienzoDescanso = 0;
        cantando = false;
    }

    // Comprueba si tiene que dejar de descansar
    public bool TerminaDescansar()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);

        if ((1 << NavMesh.GetAreaFromName("Bambalinas") & hit.mask) != 0)
            tiempoComienzoDescanso += Time.deltaTime;

        return tiempoComienzoDescanso >= tiempoDeDescanso;
    }

    // Comprueba si se encuentra en la celda
    public bool EstaEnCelda()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(agente.transform.position, out hit, 2f, NavMesh.AllAreas);
        return (1 << NavMesh.GetAreaFromName("Celda") & hit.mask) != 0;

    }

    // Comprueba si esta en un sitio desde el cual sabe llegar al escenario
    public bool ConozcoEsteSitio()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);

        return (1 << NavMesh.GetAreaFromName("Escenario") & hit.mask) != 0 ||
               (1 << NavMesh.GetAreaFromName("Bambalinas") & hit.mask) != 0 ||
               (1 << NavMesh.GetAreaFromName("Palco Oeste") & hit.mask) != 0 ||
               (1 << NavMesh.GetAreaFromName("Palco Este") & hit.mask) != 0 ||
               (1 << NavMesh.GetAreaFromName("Butacas") & hit.mask) != 0 ||
               (1 << NavMesh.GetAreaFromName("Pasillos Escenario") & hit.mask) != 0;
    }

    //Mira si ve al vizconde con un angulo de vision y una distancia maxima
    public bool Scan()
    {
        Vector3 p = transform.position;
        double angulo = Vector3.Angle(transform.forward, objetivo.position - p);
        RaycastHit hit;

        if (angulo < anguloVistaHorizontal && Vector3.Magnitude(p - objetivo.position) <= distanciaVista)
            return Physics.Raycast(p, objetivo.position - p, out hit, Mathf.Infinity) && hit.collider.gameObject.GetComponent<Player>();

        return false;
    }

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
    public void IntentaMerodear()
    {
        //Vector3 d = transform.position - agente.destination;
        //if (d.magnitude <= agente.stoppingDistance)
        //{
        tiempoComienzoMerodeo += Time.deltaTime;
        if (tiempoComienzoMerodeo >= tiempoDeMerodeo)
        {
            tiempoComienzoMerodeo = 0;
            agente.SetDestination(RandomNavSphere(distanciaDeMerodeo));
        }
        //}
    }
    public bool GetCapturada()
    {
        return capturada;
    }

    public void setCapturada(bool cap)
    {
        capturada = cap;
        cantando = false;
    }

    public GameObject sigueFantasma()
    {
        // ?
        return null;
    }

    public void sigueVizconde()
    {
        agente.SetDestination(objetivo.position);

        if (Vector3.SqrMagnitude(transform.position - objetivo.position) < 1.2f)
            agente.SetDestination(transform.position);
    }

}
