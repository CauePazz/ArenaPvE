using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class moveNPC : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent agente;
    public Transform player;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Status")]
    public float vida = 100f;
    public float danoTiro = 10f;
    public float velocidadeBala = 10f;
    public float tempoEntreTiros = 1.5f;

    private bool playerNaVisao = false;
    private bool playerNoAlcance = false;
    private float tempoTiroAtual = 0f;

    void Start()
    {
        if (agente == null)
            agente = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        tempoTiroAtual += Time.deltaTime;

        // Perseguir player se estiver na visão
        if (playerNaVisao && !playerNoAlcance)
        {
            agente.isStopped = false;
            agente.SetDestination(player.position);
        }
        // Parar para atirar se estiver perto
        else if (playerNoAlcance)
        {
            agente.isStopped = true;

            // Olhar para o player
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0; // não rotaciona no eixo Y
            transform.rotation = Quaternion.LookRotation(lookDir);

            if (tempoTiroAtual >= tempoEntreTiros)
            {
                Atirar();
                tempoTiroAtual = 0f;
            }
        }
        else
        {
            agente.isStopped = true;
        }
    }

    public void Atirar()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bullet.transform.forward * velocidadeBala;
        Destroy(bullet, 3f);
    }

    public void LevarDano(float dano)
    {
        vida -= dano;
        Debug.Log("Vida do Inimigo: " + vida);
        if (vida <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Destroy(gameObject);
    }

    public void SetVisao(bool status)
    {
        playerNaVisao = status;
    }

    public void SetAtaque(bool status)
    {
        playerNoAlcance = status;
    }
}
