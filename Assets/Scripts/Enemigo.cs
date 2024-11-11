using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class Enemigo : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad del enemigo al moverse hacia el jugador
    private Transform jugador; // Referencia al transform del jugador

    void Start()
    {
        // Encuentra al jugador en la escena por su tag "Player"
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (jugador == null)
        {
            Debug.LogWarning("No se encontr� al jugador con el tag 'Player'. Aseg�rate de que el jugador tenga este tag.");
        }
    }

    void Update()
    {
        // Mover el enemigo hacia el jugador solo si existe
        if (jugador != null)
        {
            // Calcula la direcci�n hacia el jugador
            Vector2 direccion = (jugador.position - transform.position).normalized;

            // Mueve el enemigo en esa direcci�n
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si colision� con el jugador
        if (collision.CompareTag("Player"))
        {
            // Destruye al enemigo
            Destroy(gameObject);

            // Destruye el jugador (si es necesario)
            Destroy(collision.gameObject);

            // Cargar la escena final
            SceneManager.LoadScene("PantallaFinal"); // Reemplaza "PantallaFinal" por el nombre de tu escena final
        }
    }

}
