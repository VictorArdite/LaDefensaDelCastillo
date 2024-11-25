using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private float _vel;
    private Vector2 minPantalla, maxPantalla;
    [SerializeField] private GameObject prefabFlecha;
    [SerializeField] private GameObject prefabEnemigo;
    [SerializeField] private float tiempoGeneracion = 2f;  // Tiempo inicial de generación
    private int cantidadEnemigos = 20;
    private int enemigosDerrotados = 0;
    private int nivelActual = 1;
    private bool enPausa = false;
    private float tiempoPausa = 10f;
    private bool generandoEnemigos = false;

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

        // Si matas 5 enemigos, se aumenta la velocidad y se hace más rápido
        if (enemigosDerrotados >= 5)  // Cada 5 enemigos derrotados
        {
            enemigosDerrotados = 0;  // Restablecemos el contador
            velocidadEnemigoBase += 0.5f;  // Aumentamos la velocidad base del enemigo
            Debug.Log("Nueva velocidad del enemigo: " + velocidadEnemigoBase); // Verificación

            // Hacer que los enemigos aparezcan más rápido
            // Reducimos el tiempo de aparición multiplicando por 0.8
            tiempoGeneracion = Mathf.Max(0.2f, tiempoGeneracion * 0.8f);  // Reducir el tiempo de aparición por un factor de 0.8
            Debug.Log("Nuevo tiempo de generación: " + tiempoGeneracion); // Verificación
        }

        // Si matas suficientes enemigos, pasa al siguiente nivel
        if (enemigosDerrotados >= 20)
        {
            enemigosDerrotados = 0;  // Reiniciar contador de enemigos
            nivelActual++;  // Aumentar el nivel
            if (nivelActual > 10) nivelActual = 10; // No permitir que el nivel sea mayor a 10

            // Cambiar la cantidad de enemigos y la velocidad base del enemigo
            cantidadEnemigos = 20 + nivelActual * 10;  // Aumentar la cantidad de enemigos por nivel
            velocidadEnemigoBase += 0.5f;  // Aumentar la velocidad del enemigo

            // Asegurarnos de detener la generación de enemigos antes de cambiar de nivel
            if (generandoEnemigos)
            {
                StopCoroutine(GenerarEnemigos());
                generandoEnemigos = false;
            }

            // Comenzar a generar enemigos para el nuevo nivel
            StartCoroutine(GenerarEnemigos());
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
        generandoEnemigos = true;

        // Generar enemigos mientras el nivel actual sea válido
        while (nivelActual <= 10)
        {
            if (!enPausa)
            {
                // Asegúrate de que la cantidad de enemigos y el tiempo de generación no se estanquen
                for (int i = 0; i < cantidadEnemigos; i++)
                {
                    InstanciarEnemigo();
                    yield return new WaitForSeconds(tiempoGeneracion);  // Tiempo entre generación
                }
            }

            // Al final de cada ciclo de generación, espera un momento antes de reiniciar
            yield return new WaitForSeconds(1f);  // Tiempo adicional entre niveles si lo deseas
        }

        generandoEnemigos = false;
    }

    private void InstanciarEnemigo()
    {
        Vector2 posicionEnemigo = new Vector2(maxPantalla.x, Random.Range(minPantalla.y, maxPantalla.y));
        GameObject enemigo = Instantiate(prefabEnemigo, posicionEnemigo, Quaternion.identity);

        Enemigo enemigoScript = enemigo.GetComponent<Enemigo>();
        enemigoScript.velocidad = velocidadEnemigoBase;  // Asignamos la velocidad correcta al enemigo
        enemigoScript.OnEnemyDestroyed += () => {
            enemigosDerrotados++; // Aumentamos el contador de enemigos derrotados
            Debug.Log("Enemigos derrotados: " + enemigosDerrotados);  // Verificación
        };
    }

    private IEnumerator CambiarDeNivel()
    {
        enPausa = true;
        yield return new WaitForSeconds(tiempoPausa);  // Pausar un poco antes de cambiar de nivel
        enPausa = false;
    }
}
