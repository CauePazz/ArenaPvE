using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float dano = 10f;
    [HideInInspector] public string dono = ""; // "Player" ou "Inimigo"

    private void OnTriggerEnter(Collider other)
    {
        // Proteção: se dono não estiver definido, evita erro
        if (string.IsNullOrEmpty(dono)) return;

        // Evita acertar quem atirou
        if (other.CompareTag(dono)) return;

        // Acertou o jogador
        if (other.CompareTag("Player") && dono != "Player")
        {
            var player = other.GetComponent<controlePlayer>();
            if (player != null)
                player.LevarDano(dano);
        }

        // Acertou o inimigo
        if (other.CompareTag("Inimigo") && dono != "Inimigo")
        {
            var inimigo = other.GetComponentInParent<moveNPC>();
            if (inimigo != null)
                inimigo.LevarDano(dano);
        }

        Destroy(gameObject);
    }
}
