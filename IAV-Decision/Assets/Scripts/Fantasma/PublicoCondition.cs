/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicoCondition : Conditional
{
    GameBlackboard blackboard;

    [SerializeField] bool publicoWest;
    [SerializeField] bool publicoEast;

    public override void OnAwake()
    {
        blackboard = GameObject.FindWithTag("Blackboard").GetComponent<GameBlackboard>();
    }

    public override TaskStatus OnUpdate()
    {
        publicoWest = blackboard.westLever.GetComponentInChildren<ControlPalanca>().caido;
        publicoWest = blackboard.eastLever.GetComponentInChildren<ControlPalanca>().caido;

        if (!publicoWest || !publicoEast)
            return TaskStatus.Success;

        else return TaskStatus.Failure;
    }
}
