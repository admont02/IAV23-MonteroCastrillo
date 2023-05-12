/*
  Script programado en su mayor parte/por completo
  por el grupo 15 de la asignatura IAV curso 22-23
 
  Simona Antonova Mihaylova
  Adrián Montero Castrillo
  Alejandro Segarra Chacón
 
 
 */

using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
public class GhostDejarCantante : Action
{
    NavMeshAgent agent;
    NavMeshAgent singerNav;
    GameObject singer;
    GameBlackboard bb;
    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>();
        singer = bb.singer;
        singerNav = singer.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        singer.GetComponent<Cantante>().setCapturada(false);
        singerNav.enabled = true;
        singer.transform.SetParent(null);
        return TaskStatus.Success;
    }
}