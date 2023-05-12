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

//Condicion de si esta la celda cerrada
public class CeldaCerradaCondition : Conditional
{
    GameBlackboard gb;

    public override void OnAwake()
    {
        gb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>();
    }
    public override TaskStatus OnUpdate()
    {
        if (gb.gate)
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }
}
