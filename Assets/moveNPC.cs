using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class moveNPC : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent agente;
    public Transform player;
    public GameObject bulletPrefab;

    [Header("Status")]
    public float vida = 100f;

    [Header("Ataque")]
    public float tempoEntreTiros = 1.5f;
    public Transform bulletSpawn;
    public float forcaTiro = 32f;
    public float forcaVertical = 8f;
    private bool jaAtirou;

    [Header("Ranges")]
    public float rangeVisao = 10f;
    public float rangeAtaque = 5f;
    public LayerMask layerPlayer;

    [Header("Patrulha")]
    public float raioPatrulha = 15f;
    public Transform centroPatrulha; // pode ser o próprio transform
    private bool pontoSetado = false;
    private Vector3 pontoDestino;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (centroPatrulha == null)
            centroPatrulha = transform;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        bool playerNaVisao = Physics.CheckSphere(transform.position, rangeVisao, layerPlayer);
        bool playerNoAlcance = Physics.CheckSphere(transform.position, rangeAtaque, layerPlayer);

        if (!playerNaVisao && !playerNoAlcance)
            Patrulhar();
        else if (playerNaVisao && !playerNoAlcance)
            Perseguir();
        else if (playerNoAlcance)
            Atacar();
    }

    void Patrulhar()
    {
        if (!pontoSetado)
            pontoSetado = GerarPontoAleatorio(centroPatrulha.position, raioPatrulha, out pontoDestino);

        if (pontoSetado)
            agente.SetDestination(pontoDestino);

        if (!agente.pathPending && agente.remainingDistance < 1f)
            pontoSetado = false;
    }

    void Perseguir()
    {
        agente.SetDestination(player.position);
    }

    void Atacar()
    {
        agente.SetDestination(transform.position); // para de se mover
        transform.LookAt(player);

        if (!jaAtirou)
        {
            Rigidbody rb = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * forcaTiro, ForceMode.Impulse);
            rb.AddForce(transform.up * forcaVertical, ForceMode.Impulse);

            jaAtirou = true;
            Invoke(nameof(ResetarTiro), tempoEntreTiros);
        }
    }

    void ResetarTiro()
    {
        jaAtirou = false;
    }

    bool GerarPontoAleatorio(Vector3 centro, float raio, out Vector3 resultado)
    {
        Vector3 pontoRandom = centro + Random.insideUnitSphere * raio;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pontoRandom, out hit, 1.0f, NavMesh.AllAreas))
        {
            resultado = hit.position;
            return true;
        }
        resultado = Vector3.zero;
        return false;
    }

    public void LevarDano(float dano)
    {
        vida -= dano;
        if (vida <= 0) Morrer();
    }

    void Morrer()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeVisao);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAtaque);
    }

    public void AplicarDificuldade(float vidaBonus, float danoBonus, float velocidadeBonus)
    {
        vida += vidaBonus;
        
        agente.speed += velocidadeBonus;
    }
}
