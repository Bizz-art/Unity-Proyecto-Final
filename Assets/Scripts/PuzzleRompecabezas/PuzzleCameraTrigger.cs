using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleCameraTrigger : MonoBehaviour
{
    public CinemachineCamera puzzleCamera;
    public int puzzleCameraPriority = 20;

    private bool playerInZone = false;
    private InputSystem_Actions inputActions;

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
        }
    }

    private void OnPuzzleInteract(InputAction.CallbackContext context)
    {
        if (playerInZone && puzzleCamera != null)
        {
            puzzleCamera.Priority = puzzleCameraPriority;
        }
    }
}

