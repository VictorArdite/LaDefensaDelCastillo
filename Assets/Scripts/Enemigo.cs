using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena
using System;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 2f; // Velocidad inicial del enemigo, se actualizar� desde la clase Movimiento
    private Transform jugador; // Referencia al transform del jugador
    public event Action OnEnemyDestroyed; // Evento para notificar cuando el enemigo es destruido

    void Start()
    {
        // Encuentra al jugador en la escena por su tag "Player"
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (jugador == null)
        {
            Debug.LogWarning("No se encontró al jugador con el tag 'Player'. Asegúrate de que el jugador tenga este tag.");
        }
    }

    void Update()
    {
        // Mover el enemigo hacia el jugador solo si existe
        if (jugador != null)
        {
            // Calcula la dirección hacia el jugador
            Vector2 direccion = (jugador.position - transform.position).normalized;

            // Mueve el enemigo en esa direcci�n con la velocidad actual
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Verifica si colisiona con el jugador
        {
            // Obtén el script de OperacionMatematica y genera la operación
            OperacionMatematica operacionMat = FindObjectOfType<OperacionMatematica>();
            if (operacionMat != null)
            {
                operacionMat.GenerarOperacion();
            }



            // Destruye al enemigo
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Flecha")) // Si colisiona con una flecha
        {
            Destroy(collision.gameObject); // Destruye la flecha para que no siga en la escena
            DestruirEnemigo();
        }
    }
}