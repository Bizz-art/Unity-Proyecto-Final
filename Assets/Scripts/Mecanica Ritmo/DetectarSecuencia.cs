using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DetectarSecuencia : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private BPM bpmScript;

    [Header("Configuración de Secuencias")]
    public int maxInputs = 4;
    private List<string> inputSequence = new();

    private float lastCheckedBeatTime = -1f;
    private bool inputReceivedThisBeat = false;
    private bool secuenciaActiva = false;

    public SecuenciaVisualizer visualizer;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        bpmScript = FindObjectOfType<BPM>();

        if (bpmScript == null)
        {
            Debug.LogError("❌ No se encontró un objeto con el script BPM en la escena.");
        }
    }

    void Update()
    {
        {
            if (bpmScript == null) return;

            if (bpmScript.lastBeatTime != lastCheckedBeatTime)
            {
                if (!inputReceivedThisBeat && secuenciaActiva)
                {
                    inputSequence.Clear();
                    secuenciaActiva = false;
                    visualizer?.MostrarError();
                }

                lastCheckedBeatTime = bpmScript.lastBeatTime;
                inputReceivedThisBeat = false;
            }
        }
    }


    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.AttackJ.performed += OnInputJ;
        inputActions.Player.AttackK.performed += OnInputK;
    }

    private void OnDisable()
    {
        inputActions.Player.AttackJ.performed -= OnInputJ;
        inputActions.Player.AttackK.performed -= OnInputK;
        inputActions.Player.Disable();
    }

    private void OnInputJ(InputAction.CallbackContext ctx) => AddInput("J");
    private void OnInputK(InputAction.CallbackContext ctx) => AddInput("K");

    private void AddInput(string input)
    {
        if (!secuenciaActiva && inputSequence.Count == 0)
            secuenciaActiva = true;
        inputReceivedThisBeat = true;
        float timeSinceLastBeat = Time.time - bpmScript.lastBeatTime;

        // Opcional: tolerancia dinámica si necesitas aflojar un poco el tiempo
        float tolerancia = bpmScript.beatWindow + Time.deltaTime;

        if (Mathf.Abs(timeSinceLastBeat) <= tolerancia)
        {
            inputSequence.Add(input);
            visualizer?.MostrarInput(input);

            if (inputSequence.Count >= maxInputs)
            {
                CheckSequence();
                inputSequence.Clear();
            }
        }
        else
        {
            visualizer?.MostrarError();
            visualizer?.Reiniciar();
            inputSequence.Clear(); // Reinicia si se equivocan
        }
    }

    private void CheckSequence()
    {
        string combo = string.Join("-", inputSequence);
        Debug.Log($"🎵 Combo detectado: {combo}");

        if (combo == "J-J-K-K")
        {
            visualizer?.Reiniciar();
            // Aquí podrías llamar animaciones, habilidades, etc.
        }
        else
        {
            Debug.Log("❓ Comando incorrecto");
        }
    }
}
