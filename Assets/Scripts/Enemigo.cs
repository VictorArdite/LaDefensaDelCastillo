using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 2f; // Velocidad inicial del enemigo, se actualizará desde la clase Movimiento
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

            // Mueve el enemigo en esa dirección con la velocidad actual
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Colisión con el jugador
        {
            Debug.Log("Colisión con el jugador detectada."); // Verifica que este mensaje aparezca

            OperacionMatematica operacionMat = FindObjectOfType<OperacionMatematica>();
            if (operacionMat != null)
            {
                Debug.Log("Llamando a GenerarOperacion...");
                operacionMat.GenerarOperacion();
            }
            else
            {
                Debug.LogError("No se encontró el script OperacionMatematica en la escena. Verifica que esté asignado correctamente.");
            }

            // Destruye al enemigo
            Destroy(gameObject);
            OnEnemyDestroyed?.Invoke(); // Llamar al evento para indicar que el enemigo fue destruido
        }
        else if (collision.CompareTag("Flecha")) // Colisión con flecha
        {
            Destroy(collision.gameObject); // Destruye la flecha para que no siga en la escena
            Destroy(gameObject);
            OnEnemyDestroyed?.Invoke(); // Llamar al evento para indicar que el enemigo fue destruido
        }
        else
        {
            Debug.Log($"Colisión con un objeto de tag: {collision.tag}");
        }
    }
}
