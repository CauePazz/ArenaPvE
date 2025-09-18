using UnityEngine;

public class ataqueInimigo : MonoBehaviour
{
    private moveNPC enemy;

    void Start()
    {
        enemy = GetComponentInParent<moveNPC>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetAtaque(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetAtaque(false);
        }
    }
}
