using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelector : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(2, 3); // Tamaño de la cuadrícula (columnas x filas)
    public float cellSize = 1.1f; // Tamaño del espacio entre cubos
    public Vector2Int selectorPos = new Vector2Int(0, 0); // Posición inicial
    public Transform selectorVisual;

    private InputSystem_Actions inputActions;
    private PuzzleManager puzzleManager;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        puzzleManager = FindObjectOfType<PuzzleManager>();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.RecogerItem.performed += ctx => TryMoveTile();
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.RecogerItem.performed -= ctx => TryMoveTile();
        inputActions.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        if (input.y > 0 && selectorPos.y < gridSize.y - 1) selectorPos.y++;
        if (input.y < 0 && selectorPos.y > 0) selectorPos.y--;
        if (input.x < 0 && selectorPos.x > 0) selectorPos.x--;
        if (input.x > 0 && selectorPos.x < gridSize.x - 1) selectorPos.x++;

        UpdateSelectorPosition();
    }

    void UpdateSelectorPosition()
    {
        Vector3 newPos = new Vector3(selectorPos.x * cellSize, 0, selectorPos.y * cellSize);
        selectorVisual.position = newPos;
    }

    void TryMoveTile()
    {
        //puzzleManager.TryMoveAt(selectorPos);
    }
}

