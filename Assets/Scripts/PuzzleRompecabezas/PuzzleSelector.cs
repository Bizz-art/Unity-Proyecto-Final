using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleSelector : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(2, 3); // columnas (Z), filas (X)
    public GameObject selectorVisual;
    public PuzzleManager puzzleManager;

    private Vector2Int selectorPosition = new Vector2Int(0, 0); // [x = columna, y = fila]
    private InputSystem_Actions inputActions;

    // Coordenadas locales relativas al padre
    private float[] xLevels = { -1f, 0f, 1f };     // Fila 0 a 2 en eje X
    private float[] zLevels = { -0.5f, 0.5f };        // Columna 0 a 1 en eje Z
    private float fixedY = 0f;                         // Altura constante en Y local

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.RecogerItem.performed += ctx => TryMoveSelectedTile();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.RecogerItem.performed -= ctx => TryMoveSelectedTile();
        inputActions.Player.Disable();
    }

    private void Start()
    {
        UpdateSelectorVisual();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        Vector2Int dir = Vector2Int.zero;

        // Corrige el movimiento en eje vertical
        if (input.y > 0.5f) dir = Vector2Int.down;        // ARRIBA → fila anterior
        else if (input.y < -0.5f) dir = Vector2Int.up; // ABAJO → fila siguiente
        else if (input.x < -0.5f) dir = Vector2Int.left;
        else if (input.x > 0.5f) dir = Vector2Int.right;

        Vector2Int newPos = selectorPosition + dir;

        if (newPos.x >= 0 && newPos.x < gridSize.x && newPos.y >= 0 && newPos.y < gridSize.y)
        {
            selectorPosition = newPos;
            UpdateSelectorVisual();
        }
    }

    private void UpdateSelectorVisual()
    {
        Vector3 localTarget = GetLocalPositionFromGrid(selectorPosition);
        selectorVisual.transform.localPosition = localTarget + Vector3.up * 0.2f;
    }

    private Vector3 GetLocalPositionFromGrid(Vector2Int gridPos)
    {
        float x = xLevels[gridPos.y]; // filas (X)
        float z = zLevels[gridPos.x]; // columnas (Z)
        float y = fixedY;

        return new Vector3(x, y, z);
    }

    private void TryMoveSelectedTile()
    {
        Vector3 localTarget = GetLocalPositionFromGrid(selectorPosition);
        Debug.Log("Intentando mover pieza en: " + localTarget);


        foreach (Transform tile in puzzleManager.tiles)
        {
            Debug.Log($"Comparando con pieza '{tile.name}' en {tile.localPosition}, distancia = {Vector3.Distance(tile.localPosition, localTarget)}");
            if (Vector2.Distance(new Vector2(tile.localPosition.x, tile.localPosition.z),
                     new Vector2(localTarget.x, localTarget.z)) < 0.2f)

            {
                Debug.Log("¡Pieza encontrada y se moverá!");
                puzzleManager.TryMoveTile(tile);
                break;
            }
        }
    }
}



