using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar a cena (opcional)

public class controlePlayer : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaAtual;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        vidaAtual = vidaMaxima;
    }

    void Update()
    {
        // Movimento do corpo com WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        transform.position += move * speed * Time.deltaTime;

        // Movimento da câmera com o mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotação do corpo (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Rotação da câmera (vertical, com clamping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Método público para levar dano
    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        Debug.Log("Vida do jogador: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    // Método chamado quando a vida chega a zero
    private void Morrer()
    {
        Debug.Log("Você morreu!");

        // Exemplo: reiniciar a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Alternativa: desativar jogador
        // gameObject.SetActive(false);
    }

    // (Opcional) Se quiser mostrar a vida atual no Inspector
    public float VidaAtual => vidaAtual;
}
