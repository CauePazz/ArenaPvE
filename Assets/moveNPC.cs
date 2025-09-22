using System;
using System.Collections;
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

    [Header("Patrulha")]
    public float raioPatrulha = 10f;
    private Vector3 destinoPatrulha;

    private bool playerNaVisao = false;
    private bool playerNoAlcance = false;
    private float tempoTiroAtual = 0f;

    void Start()
    {
        if (agente == null)
            agente = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartPatrulhar();
    }

    void Update()
    {
        tempoTiroAtual += Time.deltaTime;

        if (playerNaVisao && !playerNoAlcance)
        {
            agente.isStopped = false;
            agente.SetDestination(player.position);
        }
        else if (playerNoAlcance)
        {
            agente.isStopped = true;

            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);

            if (tempoTiroAtual >= tempoEntreTiros)
            {
                Atirar();
                tempoTiroAtual = 0f;
            }
        }
        else
        {
            // Patrulha
            agente.isStopped = false;

            if (!agente.pathPending && agente.remainingDistance < 0.5f)
            {
                destinoPatrulha = GetNovoDestino();
                agente.SetDestination(destinoPatrulha);
            }
}
    }

    public void Atirar()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Ignora colisão com o próprio inimigo
        Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider enemyCollider = GetComponent<Collider>();
        if (bulletCollider && enemyCollider)
        {
            Physics.IgnoreCollision(bulletCollider, enemyCollider);
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bullet.transform.forward * velocidadeBala;

        // Define quem atirou
        bullet.GetComponent<bulletScript>().dono = "Inimigo";

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
        GameManager.instance.RegistrarMorteInimigo(); // novo
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

    private void StartPatrulhar()
    {
        destinoPatrulha = GetNovoDestino();
        agente.SetDestination(destinoPatrulha);
    }

    private Vector3 GetNovoDestino()
    {
        Vector3 randomPos = transform.position + new Vector3(
            UnityEngine.Random.Range(-raioPatrulha, raioPatrulha),
            0,
            UnityEngine.Random.Range(-raioPatrulha, raioPatrulha)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, raioPatrulha, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }

    // Método chamado pelo GameManager para aumentar atributos
    public void AplicarDificuldade(float vidaBonus, float danoBonus, float velocidadeBonus)
    {
        vida += vidaBonus;
        danoTiro += danoBonus;
        agente.speed += velocidadeBonus;
    }
}
