using UnityEngine;
using TMPro; // Asegúrate de importar TextMeshPro
using System.Collections;

public class Movimiento : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private float _vel;
    private Vector2 minPantalla, maxPantalla;
    [SerializeField] private GameObject prefabFlecha;
    [SerializeField] private GameObject prefabEnemigo;
    private int cantidadEnemigosPorOleada = 10; // Cantidad de enemigos por oleada
    private int enemigosRestantes;
    private float tiempoEntreEnemigos = 0.5f;
    private float velocidadEnemigos = 2f; // Velocidad base de los enemigos
    private bool generandoEnemigos = false;

    [SerializeField] private float tiempoRecarga = 0.3f; // Tiempo mínimo entre disparos
    private float ultimoDisparo = 0f; // Momento del último disparo

    // Referencia a TextMeshProUGUI para mostrar el temporizador
    [SerializeField] private TextMeshProUGUI timerText; // Cambiado a TextMeshProUGUI
    private float tiempoRestanteOleada = 7f; // Tiempo entre oleadas
    private bool mostrandoCronometro = false; // Para saber si estamos mostrando el cronómetro
    private bool oleadaEnCurso = false; // Para saber si una oleada está en curso

    void Start()
    {
        _vel = 13f;
        minPantalla = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxPantalla = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        minPantalla.x += 1.5f;
        maxPantalla.x -= 1.5f;
        minPantalla.y += 1.5f;
        maxPantalla.y -= 1.5f;

        enemigosRestantes = cantidadEnemigosPorOleada;
        StartCoroutine(GenerarOleadas());
    }

    void Update()
    {
        MoverPers();
        DisparaFlecha();

        // Mostrar el temporizador si es necesario
        if (mostrandoCronometro)
        {
            tiempoRestanteOleada -= Time.deltaTime;
            timerText.text = Mathf.Ceil(tiempoRestanteOleada).ToString(); // Redondear el tiempo para mostrarlo
            if (tiempoRestanteOleada <= 0)
            {
                // Finalizar el temporizador y comenzar la siguiente oleada
                mostrandoCronometro = false;
                tiempoRestanteOleada = 7f; // Reiniciar el temporizador
                oleadaEnCurso = false; // Ya no hay oleada en curso

                // Comenzar la siguiente oleada
                StartCoroutine(GenerarOleadas());
            }
        }
    }

    private void MoverPers()
    {
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= ultimoDisparo + tiempoRecarga)
        {
            ultimoDisparo = Time.time;
            GameObject projectil = Instantiate(prefabFlecha);
            projectil.transform.position = transform.position;
            projectil.tag = "Flecha";
        }
    }

    private IEnumerator GenerarOleadas()
    {
        // Si ya estamos esperando entre oleadas, no generar enemigos.
        if (oleadaEnCurso)
        {
            yield break;
        }

        oleadaEnCurso = true;

        // Generar enemigos para la oleada actual
        generandoEnemigos = true;
        for (int i = 0; i < cantidadEnemigosPorOleada; i++)
        {
            InstanciarEnemigo();
            yield return new WaitForSeconds(tiempoEntreEnemigos);
        }
        generandoEnemigos = false;

        // Esperar a que se derroten todos los enemigos antes de pasar a la siguiente oleada
        while (enemigosRestantes > 0)
        {
            yield return null;
        }

        // Pausa de 7 segundos entre oleadas
        mostrandoCronometro = true; // Mostrar el cronómetro
        yield return new WaitForSeconds(7f);

        // Incrementar la dificultad
        cantidadEnemigosPorOleada += 5;
        tiempoEntreEnemigos = Mathf.Max(0.3f, tiempoEntreEnemigos - 0.05f);
        velocidadEnemigos += 0.5f;
    }

    private void InstanciarEnemigo()
    {
        Vector2 posicionEnemigo = new Vector2(maxPantalla.x, Random.Range(minPantalla.y, maxPantalla.y));
        GameObject enemigo = Instantiate(prefabEnemigo, posicionEnemigo, Quaternion.identity);

        Enemigo enemigoScript = enemigo.GetComponent<Enemigo>();
        enemigoScript.velocidad = velocidadEnemigos;
        enemigoScript.OnEnemyDestroyed += () =>
        {
            enemigosRestantes--;
        };
    }
}
