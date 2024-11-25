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
    private int cantidadEnemigosPorOleada = 10; // Cantidad de enemigos por oleada
    private int enemigosRestantes;
    private float tiempoEntreEnemigos = 0.5f;
    private float velocidadEnemigos = 2f; // Velocidad base de los enemigos
    private bool generandoEnemigos = false;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectil = Instantiate(prefabFlecha);
            projectil.transform.position = transform.position;
            projectil.tag = "Flecha";
        }
    }

    private IEnumerator GenerarOleadas()
    {
        while (true)
        {
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
            yield return new WaitForSeconds(7f);

            // Incrementar la dificultad
            cantidadEnemigosPorOleada += 5; // Más enemigos por oleada
            tiempoEntreEnemigos = Mathf.Max(0.3f, tiempoEntreEnemigos - 0.05f); // Reducir tiempo entre enemigos
            velocidadEnemigos += 0.5f; // Incrementar velocidad de los enemigos
        }
    }

    private void InstanciarEnemigo()
    {
        Vector2 posicionEnemigo = new Vector2(maxPantalla.x, Random.Range(minPantalla.y, maxPantalla.y));
        GameObject enemigo = Instantiate(prefabEnemigo, posicionEnemigo, Quaternion.identity);

        Enemigo enemigoScript = enemigo.GetComponent<Enemigo>();
        enemigoScript.velocidad = velocidadEnemigos; // Ajustar la velocidad del enemigo
        enemigoScript.OnEnemyDestroyed += () =>
        {
            enemigosRestantes--;
        };
    }
}