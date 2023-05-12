/*
  Script programado en su mayor parte/por completo
  por el grupo 15 de la asignatura IAV curso 22-23
 
  Simona Antonova Mihaylova
  Adrián Montero Castrillo
  Alejandro Segarra Chacón
 
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

/*
 * Accion de encerrar a la cantante
 */

public class GhostEncerrarCantante : Action
{
    NavMeshAgent agent;
    NavMeshAgent singerNav;
    GameObject singer;
    GameBlackboard bb;
    GameObject celda;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>();
        celda = bb.celda;
        singer = bb.singer;
        singerNav = singer.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(celda.transform.position);

        if (Vector3.SqrMagnitude(transform.position - celda.transform.position) < 1.5f)
        {
            Debug.Log("Encerrada");
            agent.SetDestination(transform.position);
            bb.imprisoned = true;
            singer.GetComponent<Cantante>().setCapturada(false);
            singerNav.enabled = true;
            singer.transform.SetParent(null);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
