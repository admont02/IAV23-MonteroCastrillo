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
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEditor.Experimental.GraphView;

/*
 * Accion de ir a la sala de musica, cuando llega devuelve Success
 */

public class GhostReturnAction : Action
{
    NavMeshAgent agente;
    GameObject musicRoom;

    // Objetivos de su itinerario
    GameBlackboard gb;
    GameObject gbgO;
    Transform Escenario;
    Transform Bambalinas;
    Transform Musica;

    public override void OnAwake()
    {
        // IMPLEMENTAR
        gbgO = GameObject.FindWithTag("Blackboard");
        gb = gbgO.GetComponent<GameBlackboard>();
        agente = GetComponent<NavMeshAgent>();
        Musica = gb.musicRoom.transform;
        Bambalinas = gb.backStage.transform;
        Escenario = gb.stage.transform;
    }

    public override TaskStatus OnUpdate()
    {
        // IMPLEMENTAR
        agente.SetDestination(Musica.position);

        Debug.Log("musica");

        //NavMeshHit hit;
        //NavMesh.SamplePosition(agente.transform.position, out hit, 2f, NavMesh.AllAreas);
        //if ((1 << NavMesh.GetAreaFromName("Música") & hit.mask) != 0)
        //{
        //    Debug.Log("vsiuvnd");
        //    return TaskStatus.Success;
        //}

        if (Vector3.SqrMagnitude(transform.position - Musica.position) < 1.5f)
        {
            agente.SetDestination(transform.position);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}