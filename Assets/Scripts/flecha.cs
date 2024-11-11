using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flecha : MonoBehaviour
{
    private float _vel;
    private Vector2 maxPantalla;

    void Start()
    {
        _vel = 10f;
        maxPantalla = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Update()
    {
        // Mueve la flecha hacia la derecha
        Vector2 posInicial = transform.position;
        posInicial += new Vector2(1, 0) * _vel * Time.deltaTime;
        transform.position = posInicial;

        // Destruye la flecha si sale de la pantalla
        if (transform.position.x > maxPantalla.x)
        {
            Destroy(gameObject);
        }
    }

    // Detecta la colisión con otros objetos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si colisionó con un enemigo
        if (collision.CompareTag("Enemigo"))
        {
            // Destruye el enemigo y la flecha
            Destroy(collision.gameObject); // Destruye al enemigo
            Destroy(gameObject); // Destruye la flecha
        }
    }
}
