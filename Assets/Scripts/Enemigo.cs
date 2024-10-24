using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad del enemigo
    private Transform jugador; // Referencia al transform del jugador

    void Start()
    {
        // Encuentra el jugador en la escena por su tag
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Asegúrate de que tu jugador tenga el tag "Player"
    }

    void Update()
    {
        // Mover el enemigo hacia el jugador
        if (jugador != null)
        {
            // Calcular la dirección hacia el jugador
            Vector2 direccion = (jugador.position - transform.position).normalized; // Dirección hacia el jugador

            // Mover el enemigo hacia el jugador
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }
}