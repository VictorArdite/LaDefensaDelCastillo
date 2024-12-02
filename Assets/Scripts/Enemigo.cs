using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 2f; // Velocidad del enemigo
    public event Action OnEnemyDestroyed; // Evento para notificar cuando el enemigo es destruido
    private Vector2 limiteIzquierdo; // Punto donde el enemigo será destruido
    private ControlVidas controlVidas; // Referencia al script de control de vidas

    void Start()
    {
        // Calcula el límite izquierdo del mapa
        limiteIzquierdo = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // Obtiene el script ControlVidas
        controlVidas = FindObjectOfType<ControlVidas>();
        if (controlVidas == null)
        {
            Debug.LogWarning("No se encontró el script ControlVidas en la escena.");
        }
    }

    void Update()
    {
        // Mueve el enemigo hacia la izquierda
        transform.position += Vector3.left * velocidad * Time.deltaTime;

        // Si el enemigo alcanza el borde izquierdo, se destruye y se resta una vida
        if (transform.position.x <= limiteIzquierdo.x)
        {
            // Resta una vida
            controlVidas?.RestarVida();

            // Destruye el enemigo
            OnEnemyDestroyed?.Invoke(); // Notifica que el enemigo fue destruido
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flecha")) // Colisión con flecha
        {
            Destroy(collision.gameObject); // Destruye la flecha
            Destroy(gameObject);
            OnEnemyDestroyed?.Invoke(); // Notifica que el enemigo fue destruido
        }
        else
        {
            Debug.Log($"Colisión con un objeto de tag: {collision.tag}");
        }
    }
}