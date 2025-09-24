using UnityEngine;

public class ataqueInimigo : MonoBehaviour
{
    private moveNPC enemy;

    void Start()
    {
        enemy = GetComponentInParent<moveNPC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.SetAtaque(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.SetAtaque(false);
    }
}
