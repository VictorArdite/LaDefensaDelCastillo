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

        Vector2 posInicial = transform.position;
        posInicial = posInicial + new Vector2(1, 0) * _vel * Time.deltaTime;
        transform.position = posInicial;


        if (transform.position.x > maxPantalla.x)
        {
            Destroy(gameObject);
        }
    }
}