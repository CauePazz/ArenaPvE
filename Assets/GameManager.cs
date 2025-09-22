using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Spawner")]
    public GameObject inimigoPrefab;
    public int inimigosPorWave = 5;
    public int inimigosMortos = 0;
    public int inimigosTotais = 25;
    public float raioSpawn = 30f;
    public float delayEntreWaves = 2f;

    private int waveAtual = 0;
    private int inimigosVivos = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(delayEntreWaves);
        waveAtual++;

        Debug.Log("Iniciando Wave " + waveAtual);

        for (int i = 0; i < inimigosPorWave; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-raioSpawn, raioSpawn),
                0,
                Random.Range(-raioSpawn, raioSpawn)
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 5f, NavMesh.AllAreas))
            {
                GameObject inimigoObj = Instantiate(inimigoPrefab, hit.position, Quaternion.identity);
                moveNPC inimigo = inimigoObj.GetComponent<moveNPC>();

                // Aumenta dificuldade com base na wave
                if (inimigo != null)
                {
                    float vidaBonus = 10f * (waveAtual - 1);
                    float danoBonus = 2f * (waveAtual - 1);
                    float velocidadeBonus = 0.5f * (waveAtual - 1);
                    inimigo.AplicarDificuldade(vidaBonus, danoBonus, velocidadeBonus);
                }

                inimigosVivos++;
            }
        }
    }

    public void RegistrarMorteInimigo()
    {
        inimigosMortos++;
        inimigosVivos--;

        Debug.Log("Inimigos mortos: " + inimigosMortos + "/" + inimigosTotais);

        if (inimigosMortos >= inimigosTotais)
        {
            Debug.Log("Jogo encerrado! Todos os inimigos foram derrotados.");
            // Aqui você pode chamar uma tela de vitória ou mudar de cena
        }
        else if (inimigosVivos <= 0)
        {
            StartCoroutine(SpawnWave());
        }
    }
}
