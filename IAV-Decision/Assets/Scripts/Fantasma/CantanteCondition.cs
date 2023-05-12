/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;


/*
 * Devuelve Success cuando la cantante es sobre el palco
 */


public class CantanteCondition : Conditional
{

    Cantante cantante;

    public override void OnAwake()
    {
        cantante = GameObject.FindGameObjectWithTag("Cantante").GetComponent<Cantante>();
    }

    public override TaskStatus OnUpdate()
    {
        if (cantante.cantando)
            return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
