using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleCameraTrigger : MonoBehaviour
{
    public CinemachineCamera puzzleCamera;
    public int puzzleCameraPriority = 20;
    private bool enPuzzle = false;
    public static bool EnPuzzle = false;
    private bool playerInZone = false;
    private InputSystem_Actions inputActions;

    public GameObject bodyMerged;
    public GameObject metaRig;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += OnPuzzleInteract;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnPuzzleInteract;
        inputActions.Player.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            // Si el jugador sale, asegurar que la cámara vuelva a la prioridad baja
            if (puzzleCamera != null)
            {
                puzzleCamera.Priority = 10;
                enPuzzle = false;
            }
        }
    }

    private void OnPuzzleInteract(InputAction.CallbackContext context)
    {
        if (playerInZone && puzzleCamera != null )
        {
            enPuzzle = !enPuzzle; // alternar estado
            puzzleCamera.Priority = enPuzzle ? puzzleCameraPriority : 10;
            PuzzleCameraTrigger.EnPuzzle = enPuzzle;
            // Oculta o muestra las partes del jugador
            bodyMerged.SetActive(!enPuzzle);
            metaRig.SetActive(!enPuzzle);
        }
        
    }
}

