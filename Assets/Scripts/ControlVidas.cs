using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControlVidas : MonoBehaviour
{
    public List<GameObject> corazones; // Lista de imágenes de corazones (asignadas desde el Inspector)

    private void Start()
    {
        // Verifica que la lista de corazones esté configurada
        if (corazones == null || corazones.Count == 0)
        {
            Debug.LogError("No se asignaron corazones en el Inspector.");
        }
    }

    public void RestarVida()
    {
        if (corazones == null || corazones.Count == 0)
        {
            Debug.LogWarning("No quedan corazones para eliminar o la lista no está configurada.");
            return;
        }

        // Obtén el último corazón de la lista
        GameObject corazon = corazones[corazones.Count - 1];
        corazones.RemoveAt(corazones.Count - 1); // Remuévelo de la lista
        corazon.SetActive(false); // Oculta el corazón

        // Si no quedan corazones, cambia a la pantalla final
        if (corazones.Count == 0)
        {
            SceneManager.LoadScene("PantallaFinal"); // Cambia "PantallaFinal" según el nombre de tu escena
        }
    }

}
