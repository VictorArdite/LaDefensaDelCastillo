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

            // Mueve el enemigo en esa dirección
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Verifica si colisiona con el jugador
        {
            // Encuentra el script de control de vidas y reduce una vida
            ControlVidas controlVidas = collision.GetComponent<ControlVidas>();
            if (controlVidas != null)
            {
                controlVidas.RestarVida();
            }

            // Destruye al enemigo
            Destroy(gameObject);
        }
    }

}
