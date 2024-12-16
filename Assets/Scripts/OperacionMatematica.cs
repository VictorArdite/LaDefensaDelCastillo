using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OperacionMatematica : MonoBehaviour
{
    public GameObject panel; // Referencia al panel
    public TextMeshProUGUI textoOperacion; // Texto para mostrar la operación
    public TMP_InputField campoRespuesta; // Campo de respuesta
    private int resultadoCorrecto;

    void Start()
    {
        // Ocultar el panel al inicio
        if (panel != null)
        {
            panel.SetActive(false); // Asegúrate de que el panel está desactivado al iniciar
            Debug.Log("Panel inicial ocultado correctamente.");
        }
        else
        {
            Debug.LogWarning("El panel no está asignado en el script OperacionMatematica. Arrástralo desde el Inspector.");
        }

        // Asegúrate de que el campo de texto escucha la tecla Enter
        if (campoRespuesta != null)
        {
            campoRespuesta.onSubmit.AddListener(delegate { VerificarRespuesta(); });
        }
        else
        {
            Debug.LogWarning("Campo de respuesta no asignado. Arrástralo desde el Inspector.");
        }
    }

    public void GenerarOperacion()
    {
        Debug.Log("Generando operación..."); // Verifica que este mensaje aparezca al tocar un enemigo

        if (panel != null)
        {
            panel.SetActive(true); // Activa el panel
            Debug.Log("Panel activado.");
            Time.timeScale = 0f; // Pausa el juego

            // Generar operación matemática
            int num1 = Random.Range(1, 10);
            int num2 = Random.Range(1, 10);
            int operacion = Random.Range(0, 3);

            switch (operacion)
            {
                case 0: // Suma
                    resultadoCorrecto = num1 + num2;
                    textoOperacion.text = $"{num1} + {num2}";
                    break;
                case 1: // Resta
                    resultadoCorrecto = num1 - num2;
                    textoOperacion.text = $"{num1} - {num2}";
                    break;
                case 2: // Multiplicación
                    resultadoCorrecto = num1 * num2;
                    textoOperacion.text = $"{num1} x {num2}";
                    break;
            }

            Debug.Log($"Resultado correcto generado: {resultadoCorrecto}");

            if (campoRespuesta != null)
            {
                campoRespuesta.text = ""; // Limpia el campo de texto
                campoRespuesta.ActivateInputField(); // Activa el campo de texto para que el usuario pueda escribir de inmediato
            }
            else
            {
                Debug.LogWarning("Campo de respuesta no asignado. Arrástralo desde el Inspector.");
            }
        }
        else
        {
            Debug.LogError("Panel no asignado en OperacionMatematica. No se puede generar operación.");
        }
    }

    public void VerificarRespuesta()
    {
        if (!panel.activeSelf)
        {
            Debug.LogWarning("El panel no está activo. Evitando verificación de respuesta.");
            return;
        }

        Debug.Log("Verificando respuesta del jugador...");

        if (int.TryParse(campoRespuesta.text.Trim(), out int respuestaUsuario))
        {
            Debug.Log($"Respuesta ingresada: {respuestaUsuario}, Respuesta correcta: {resultadoCorrecto}");

            if (respuestaUsuario == resultadoCorrecto)
            {
                Debug.Log("Respuesta correcta");
                // Lógica adicional si es necesario
            }
            else
            {
                Debug.Log("Respuesta incorrecta");
                RestarVida();
            }
        }
        else
        {
            Debug.LogWarning("El usuario ingresó un valor no válido.");
            RestarVida(); // Si la entrada no es válida, se resta una vida
        }

        // Ocultar el panel y reanudar el juego
        if (panel != null)
        {
            panel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    // Método para restar vida al jugador
    private void RestarVida()
    {
        ControlVidas controlVidas = FindObjectOfType<ControlVidas>();
        if (controlVidas != null)
        {
            controlVidas.RestarVida();
        }
        else
        {
            Debug.LogWarning("No se encontró el script ControlVidas. Asegúrate de que esté en la escena.");
        }
    }


}
