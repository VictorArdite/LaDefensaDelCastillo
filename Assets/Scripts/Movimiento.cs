using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour {
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private float _vel;
    private Vector2 minPantalla, maxPantalla;
    [SerializeField] private GameObject prefabFlecha;
    [SerializeField] private GameObject prefabEnemigo;
    [SerializeField] private float tiempoGeneracion = 2f;
    private int cantidadEnemigos = 5; // Cambiado a 5
    private int enemigosDerrotados = 0;
    private int nivelActual = 1;
    private bool enPausa = false;
    private float tiempoPausa = 10f;

    // Nueva variable para la velocidad base del enemigo
    private float velocidadEnemigoBase = 2f;

    void Start()
    {
        _vel = 13f;
        minPantalla = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxPantalla = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        minPantalla.x += 1.5f;
        maxPantalla.x -= 1.5f;
        minPantalla.y += 1.5f;
        maxPantalla.y -= 1.5f;

        StartCoroutine(GenerarEnemigos());
    }

    void Update()
    {
        if (!enPausa)
        {
            MoverPers();
            DisparaFlecha();
        }

        if (enemigosDerrotados >= cantidadEnemigos) // Cambiado a usar cantidadEnemigos
        {
            enemigosDerrotados = 0;
            nivelActual++;
            if (nivelActual > 10) nivelActual = 10;

            // Aumenta la velocidad base del enemigo con cada nivel
            velocidadEnemigoBase += 0.5f;

            StartCoroutine(CambiarDeNivel());
        }
    }

    private void MoverPers()
    {
        // Lógica de movimiento del personaje
        float direccioIndicadaX = Input.GetAxisRaw("Horizontal");
        float direccioIndicadaY = Input.GetAxisRaw("Vertical");

        Vector2 direccioIndicada = new Vector2(direccioIndicadaX, direccioIndicadaY).normalized;
        Vector2 novaPos = transform.position;
        novaPos += direccioIndicada * _vel * Time.deltaTime;

        novaPos.x = Mathf.Clamp(novaPos.x, minPantalla.x, maxPantalla.x);
        novaPos.y = Mathf.Clamp(novaPos.y, minPantalla.y, maxPantalla.y);

        transform.position = novaPos;
    }

    private void DisparaFlecha()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectil = Instantiate(prefabFlecha);
            projectil.transform.position = transform.position;
            projectil.tag = "Flecha"; // Asigna el tag dinámicamente a la flecha
        }
    }

    private IEnumerator GenerarEnemigos()
    {
        while (nivelActual <= 10)
        {
            if (!enPausa)
            {
                for (int i = 0; i < cantidadEnemigos; i++)
                {
                    InstanciarEnemigo();
                    yield return new WaitForSeconds(tiempoGeneracion);
                }

                if (nivelActual > 1)
                {
                    cantidadEnemigos = 5 + (nivelActual - 1) * 5; // Incrementa en función del nivel
                    tiempoGeneracion = Mathf.Max(0.5f, tiempoGeneracion - 0.1f);
                }

                yield return StartCoroutine(CambiarDeNivel());
            }

            yield return null;
        }
    }

    private void InstanciarEnemigo()
    {
        Vector2 posicionEnemigo = new Vector2(maxPantalla.x, Random.Range(minPantalla.y, maxPantalla.y));
        GameObject enemigo = Instantiate(prefabEnemigo, posicionEnemigo, Quaternion.identity);

        Enemigo enemigoScript = enemigo.GetComponent<Enemigo>();
        enemigoScript.velocidad = velocidadEnemigoBase;
        enemigoScript.OnEnemyDestroyed += () => {
            enemigosDerrotados++;
        };
    }

    private IEnumerator CambiarDeNivel()
    {
        enPausa = true;
        yield return new WaitForSeconds(tiempoPausa);
        enPausa = false;
    }
}