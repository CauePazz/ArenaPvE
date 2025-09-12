using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlePlayer : MonoBehaviour
{
    [SerializeField] private float velocidade = 20.0f;
    [SerializeField] private float girar = 60.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float translate = (Input.GetAxis("Vertical") * velocidade) * Time.deltaTime;
        float rotate = (Input.GetAxis("Horizontal") * girar) * Time.deltaTime;

        transform.Translate(0, 0, translate);
        transform.Rotate(0, rotate, 0);
    }
}