/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoCondition : Conditional
{
    ControlPiano piano;

    public override void OnAwake()
    {
        piano = GameObject.FindGameObjectWithTag("Piano").GetComponent<ControlPiano>();
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log(piano.roto);

        if (piano.roto)
            return TaskStatus.Success;

        else return TaskStatus.Failure;
    }
}
