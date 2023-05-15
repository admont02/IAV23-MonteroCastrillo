using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Cliente : MonoBehaviour
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
    public double tiempoDeEsperaBebida;
    // Segundo en el que comezo a merodear
    public double tiempoComienzoEsperarBebida = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;
    // Si canta o no
    public bool esperando = false;
    public bool esperandoBebida = false;
    public bool enMesa = false;




    // Componente cacheado NavMeshAgent
    private NavMeshAgent agente;

    // Objetivos de su itinerario
    public Transform Puerta;
    public Transform Barra;
    private GameObject barraGO;

    // La blackboard
    public GameBlackboard bb;

    //para seguir al fantasma o al vizconde
    public GameObject icono;

    //sprites 

    public Sprite[] sprites;
    public void Awake()
    {
        barraGO = GameObject.FindWithTag("Barra");
        Barra = barraGO.transform;
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
    public void Cantar()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);

        bool enEscenario = (1 << NavMesh.GetAreaFromName("Barra") & hit.mask) != 0;

        if (!enEscenario)
        {
            agente.SetDestination(Barra.position);
        }

        tiempoComienzoCanto = 0;
        esperando = true;

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
        tiempoComienzoEsperarBebida += Time.deltaTime;
        if (tiempoComienzoEsperarBebida >= tiempoDeEsperaBebida)
        {
            tiempoComienzoEsperarBebida = 0;
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
        esperando = false;
    }

    public GameObject sigueFantasma()
    {
        // ?
        return null;
    }

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
        //  transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        //transform.rotation = Quaternion.Euler(0, -90, 0);
    }
}
