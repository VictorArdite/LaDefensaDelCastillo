using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OperacionMatematica : MonoBehaviour
{
    [SerializeField] private TMP_Text textoOperacion; // Referencia al texto donde se muestra la operación
    [SerializeField] private TMP_InputField campoRespuesta; // Campo de texto para la respuesta del jugador
    [SerializeField] private Button botonConfirmar; // Botón de confirmación

    private int resultadoCorrecto; // Almacena el resultado correcto de la operación

    private void Start()
    {
        // Asocia el botón con el método VerificarRespuesta
        botonConfirmar.onClick.AddListener(VerificarRespuesta);
    }

    public void GenerarOperacion()
    {
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        int operador = Random.Range(0, 4); // 0: suma, 1: resta, 2: multiplicación, 3: división
        string operacionTexto = "";

        switch (operador)
        {
            case 0: // Suma
                resultadoCorrecto = num1 + num2;
                operacionTexto = num1 + " + " + num2;
                break;
            
            case 1: // Resta
                resultadoCorrecto = num1 - num2;
                operacionTexto = num1 + " - " + num2;
                break;
            
            case 2: // Multiplicación
                resultadoCorrecto = num1 * num2;
                operacionTexto = num1 + " * " + num2;
                break;
            
            case 3: // División (asegura que el resultado sea entero)
                num1 = num1 * num2; // Así el primer número será divisible por el segundo
                resultadoCorrecto = num1 / num2;
                operacionTexto = num1 + " / " + num2;
                break;
        }

        // Muestra la operación en el texto y pausa el juego
        textoOperacion.text = operacionTexto + " = ?";
        Time.timeScale = 0f;
    }


    public void VerificarRespuesta()
    {
        int respuestaJugador;
        if (int.TryParse(campoRespuesta.text, out respuestaJugador))
        {
            if (respuestaJugador != resultadoCorrecto)
            {
                ControlVidas controlVidas = FindObjectOfType<ControlVidas>();
                if (controlVidas != null)
                {
                    controlVidas.RestarVida(); // Resta una vida solo si la respuesta es incorrecta
                }
            }
        }

        // Reanuda el juego
        Time.timeScale = 1f;

        // Limpia el campo de respuesta y oculta la operación
        campoRespuesta.text = "";
        textoOperacion.text = "";
    }

}
