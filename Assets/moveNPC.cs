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

    [Header("Patrulha Aleatória")]
    public float raioPatrulha = 10f;
    public Transform centroPatrulha;

    private float tempoTiroAtual = 0f;
    private bool playerNaVisao = false;
    private bool playerNoAlcance = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (centroPatrulha == null)
            centroPatrulha = transform;

        SetNovoDestinoPatrulha();
    }

    void Update()
    {
        tempoTiroAtual += Time.deltaTime;

        // --- SE PLAYER ESTÁ NO ALCANCE PARA ATACAR ---
        if (playerNoAlcance)
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
        // --- SE PLAYER ESTÁ NA VISÃO MAS FORA DO ALCANCE ---
        else if (playerNaVisao)
        {
            agente.isStopped = false;
            agente.SetDestination(player.position);
        }
        // --- SENÃO, PATRULHA ---
        else
        {
            agente.isStopped = false;

            if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
                SetNovoDestinoPatrulha();
        }
    }

    void SetNovoDestinoPatrulha()
    {
        Vector3 ponto;
        if (PontoAleatorio(centroPatrulha.position, raioPatrulha, out ponto))
            agente.SetDestination(ponto);
    }

    bool PontoAleatorio(Vector3 centro, float raio, out Vector3 resultado)
    {
        Vector3 pontoRand = centro + Random.insideUnitSphere * raio;
        pontoRand.y = centro.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pontoRand, out hit, 2f, NavMesh.AllAreas))
        {
            resultado = hit.position;
            return true;
        }

        resultado = Vector3.zero;
        return false;
    }

    public void Atirar()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Collider bulletCol = bullet.GetComponent<Collider>();
        Collider corpo = GetComponent<Collider>();
        if (bulletCol && corpo) Physics.IgnoreCollision(bulletCol, corpo);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bullet.transform.forward * velocidadeBala;

        bullet.GetComponent<bulletScript>().dono = "Inimigo";
        Destroy(bullet, 3f);
    }

    public void LevarDano(float dano)
    {
        vida -= dano;
        if (vida <= 0) Morrer();
    }

    void Morrer()
    {
        if (GameManager.instance != null)
            GameManager.instance.RegistrarMorteInimigo();

        Destroy(gameObject);
    }

    public void SetVisao(bool status) => playerNaVisao = status;
    public void SetAtaque(bool status) => playerNoAlcance = status;

    public void AplicarDificuldade(float vidaBonus, float danoBonus, float velocidadeBonus)
    {
        vida += vidaBonus;
        danoTiro += danoBonus;
        agente.speed += velocidadeBonus;
    }
}
