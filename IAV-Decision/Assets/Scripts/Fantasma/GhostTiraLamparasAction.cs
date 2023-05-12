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

public class GhostTiraLamparasAction : Action
{
    GameBlackboard bb;
    NavMeshAgent agent;
    ControlPalanca palancaIz;
    ControlPalanca palancaDr;

    public override void OnAwake()
    {
        bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>();
        agent = GetComponent<NavMeshAgent>();

        palancaIz = bb.westLever.GetComponentInChildren<ControlPalanca>();
        palancaDr = bb.eastLever.GetComponentInChildren<ControlPalanca>();

    }

    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(palancaCercana().transform.position);
        if (palancaDr.caido && palancaIz.caido)
            return TaskStatus.Success;

        else return TaskStatus.Running;
    }
    private ControlPalanca palancaCercana()
    {
        if (!palancaDr.caido && !palancaIz.caido)
        {
            Vector3 i = palancaIz.transform.position - transform.position;
            Vector3 d = palancaDr.transform.position - transform.position;

            if (d.magnitude >= i.magnitude)
                return palancaIz;

            else return palancaDr;
        }
        else if (!palancaDr.caido)
            return palancaDr;

        return palancaIz;
    }
}
