using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necesario para TextMeshPro

public class ControlVidas : MonoBehaviour
{
    public int vidas = 5; // N�mero inicial de vidas
    [SerializeField] private TMP_Text Vidas; // Referencia al texto de vidas en TextMeshPro

    private void Start()
    {
        ActualizarTextoVidas(); // Inicializa el texto de vidas en pantalla
    }

    public void RestarVida()
    {
        vidas--; // Reduce una vida
        ActualizarTextoVidas(); // Actualiza el texto en pantalla

        // Verifica si las vidas han llegado a cero
        if (vidas <= 0)
        {
            SceneManager.LoadScene("PantallaFinal"); // Cambia "PantallaFinal" por el nombre de tu escena final
        }
    }

    private void ActualizarTextoVidas()
    {
        Vidas.text = "Vidas: " + vidas; // Actualiza el texto con las vidas actuales
    }
}
