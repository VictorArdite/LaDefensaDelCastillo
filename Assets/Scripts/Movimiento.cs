using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private float _vel;
    private Vector2 minPantalla, maxPantalla;
    [SerializeField] private GameObject prefabFlecha; // Prefab para las flechas
    [SerializeField] private GameObject prefabEnemigo; // Prefab del enemigo
    [SerializeField] private float tiempoGeneracion = 2f; // Tiempo entre generaciones de enemigos
    [SerializeField] private int cantidadEnemigos = 5; // Cantidad de enemigos a generar

    void Start()
    {
        _vel = 13f;
        minPantalla = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxPantalla = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        minPantalla.x += 1.5f;
        maxPantalla.x -= 1.5f;
        minPantalla.y += 1.5f;
        maxPantalla.y -= 1.5f;

        StartCoroutine(GenerarEnemigos()); // Iniciar la generaci�n de enemigos
    }

    void Update()
    {
        MoverPers();
        DisparaFlechal();
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

    private void DisparaFlechal()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject projectil = Instantiate(prefabFlecha);
            projectil.transform.position = transform.position;
        }
    }

    private IEnumerator GenerarEnemigos()
    {
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            InstanciarEnemigo(); // Instanciar un enemigo
            yield return new WaitForSeconds(tiempoGeneracion); // Esperar el tiempo antes de generar el siguiente
        }
    }

    private void InstanciarEnemigo()
    {
        Vector2 posicionEnemigo = new Vector2(maxPantalla.x, Random.Range(minPantalla.y, maxPantalla.y)); // Posici�n inicial del enemigo
        Instantiate(prefabEnemigo, posicionEnemigo, Quaternion.identity); // Crear el enemigo
    }
}