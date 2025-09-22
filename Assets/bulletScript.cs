using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float dano = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controlePlayer player = other.GetComponent<controlePlayer>();
            if (player != null)
            {
                player.LevarDano(dano);
            }
            Destroy(gameObject);
        }
    }
}
