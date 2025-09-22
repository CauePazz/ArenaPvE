using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float dano = 10f;
    [HideInInspector] public string dono; // "Player" ou "Inimigo"

    private void OnTriggerEnter(Collider other)
    {
        // Evita colisão com o dono da bala
        if (other.CompareTag(dono)) return;

        // Se acertar o jogador
        if (other.CompareTag("Player") && dono != "Player")
        {
            controlePlayer player = other.GetComponent<controlePlayer>();
            if (player != null)
            {
                player.LevarDano(dano);
            }
        }

        // Se acertar o inimigo
        if (other.CompareTag("Inimigo") && dono != "Inimigo")
        {
            // Usa GetComponentInParent porque o collider pode estar em um filho
            moveNPC inimigo = other.GetComponentInParent<moveNPC>();
            if (inimigo != null)
            {
                inimigo.LevarDano(dano);
            }
        }

        Destroy(gameObject);
    }
}
