using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class moveNPC : MonoBehaviour
{
    public float visao = 15.0f;
    public float tiro = 10.0f;
    public Transform alvo;
    private NavMeshAgent agente;
    private Boolean atirar = false;

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, alvo.position) <= visao)
        {
            agente.isStopped = false;
            agente.SetDestination(alvo.position);
        }
        else
        {
            agente.isStopped = true;
        }


        if (Vector3.Distance(transform.position, alvo.position) <= tiro)
        {
            atirar = true;
        }
        else
        {
            atirar = false;
        }
    }

    void FixedUpdate()
    {
        if (atirar)
        {
            atirando();
        }
    }

    public void atirando()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        agente.isStopped = false;
        agente.SetDestination(alvo.position);
    }

    private void OnTriggerExit(Collider other)
    {
        agente.isStopped = true;
    }
}
