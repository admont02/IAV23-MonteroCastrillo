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
/*
  Script programado en su mayor parte/por completo
  por el grupo 15 de la asignatura IAV curso 22-23
 
  Simona Antonova Mihaylova
  Adrián Montero Castrillo
  Alejandro Segarra Chacón
 
 
 */


using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

/*
 * Devuelve Success cuando la cantante es sobre el palco
 */

public class VizcondeChocaCondition : Conditional
{
    GameObject Vizconde;
    NavMeshAgent agent;

    CapsuleCollider cc;
    bool golpeado = false;

    public override void OnAwake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        Vizconde = GameObject.FindGameObjectWithTag("Player");
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log(Vector3.SqrMagnitude(Vizconde.transform.position - transform.position));

        if(Vector3.SqrMagnitude(Vizconde.transform.position - transform.position) < 10.5f && GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>().singer.GetComponent<Cantante>().GetCapturada())
        {

            golpeado = true;
            return TaskStatus.Success;
        }
        
        golpeado = false;
        return TaskStatus.Failure;
    }
}
