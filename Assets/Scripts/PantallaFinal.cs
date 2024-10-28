using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PantallaFinal : MonoBehaviour
{
    public void VolverAlInicio()
    {
        SceneManager.LoadScene("MenuInicial"); // el nombre coincide con la escena de inicio
    }

    public void SalirJuego()
    {
        Application.Quit(); // Esto cierra el juego
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // para editar
#endif
    }
}
