using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private float _vel;
    private Vector2 minPantalla, maxPantalla;
    [SerializeField] private GameObject prefabFlecha;
    void Start()
    {
        _vel = 13f;
        minPantalla = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxPantalla = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        minPantalla.x = minPantalla.x + 1.5f;
        maxPantalla.x = maxPantalla.x - 1.5f;
        minPantalla.y += 1.5f;
        maxPantalla.y -= 1.5f;
    }

    // Update is called once per frame
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
        novaPos = novaPos + direccioIndicada * _vel * Time.deltaTime;

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
}
