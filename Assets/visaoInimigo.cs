using UnityEngine;

public class visaoInimigo : MonoBehaviour
{
    private moveNPC enemy;

    void Start()
    {
        enemy = GetComponentInParent<moveNPC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.SetVisao(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.SetVisao(false);
    }
}
