using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Se encarga de controlar si el público debería huir o quedarse en el patio de butacas
 */

public class Publico : MonoBehaviour
{
    //int lucesEncendidas = 2;
    public bool sentado = true;
    bool miLuzEncendida;
    GameObject vestibulo;
    NavMeshAgent agente;
    Vector3 posInicial;

    GameObject luzAsociada;
    private void Start()
    {
        //lucesEncendidas = 2;
        agente = GetComponent<NavMeshAgent>();
        agente.stoppingDistance = .25f;
        sentado = true;
        vestibulo = GameObject.FindGameObjectWithTag("Vestibulo");
        posInicial=transform.position;
    }

    public void LateUpdate()
    {
        //para que rote hacia donde se mueve
        if (GetComponent<NavMeshAgent>().velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<NavMeshAgent>().velocity.normalized);
        }
        else if (miLuzEncendida)  //para que al llegar a su butaca miren hacia delante(el escenario)
            transform.rotation = new Quaternion(0, 0, 0, 0);

        if (!sentado && !agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
        {
            // Si hemos llegado, desactivamos el NavMeshAgent
            agente.isStopped = true;
        }

    }

    public bool getLuces()
    {
        return sentado;
    }

    public void apagaLuz()
    {
        miLuzEncendida = false;
        sentado = false;
        //destino --> vestibulo
        agente.SetDestination(vestibulo.transform.position);

        //lucesEncendidas--;
        //sentado = lucesEncendidas == 2;
    }
    //se llama cuando el fantasma o el vizconde desactivan o activan las luces
    public void enciendeLuz()
    {
        agente.isStopped=false; 
        miLuzEncendida = true;
        sentado = true;
        agente.SetDestination(posInicial);
        // lucesEncendidas++;
        //sentado = lucesEncendidas == 2;
    }

}
