/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

/*
 * Accion de ir al escenario, cuando llega devuelve Success
 */

public class GhostSearchStageAction : Action
{
    NavMeshAgent agent;
    GameObject stage;
    GameBlackboard gb;

    public override void OnAwake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        gb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>();
        stage = gb.stage;
    }

    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(stage.transform.position);

        if (Vector3.SqrMagnitude(stage.transform.position - transform.position) < 1.5f)
            return TaskStatus.Success;
        else return TaskStatus.Running;
    }
}