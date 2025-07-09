using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 120f; // Grados por segundo

    private CharacterController controller;
    private InputSystem_Actions inputActions;
    private float moveInput;
    private float rotateInput;
    private PlayerCombatMode combatScript;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
        combatScript = GetComponent<PlayerCombatMode>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => ReadInput(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled += ctx => ReadInput(Vector2.zero);
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => ReadInput(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled -= ctx => ReadInput(Vector2.zero);
        inputActions.Player.Disable();
    }

    private void ReadInput(Vector2 input)
    {
        moveInput = input.y;
        rotateInput = input.x;
    }

    private void Update()
    {
        if (combatScript != null && combatScript.IsInCombatMode())
            return;
        Rotate();
        Move();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * rotateInput * rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 forward = transform.forward * moveInput;
        controller.Move(forward * moveSpeed * Time.deltaTime);
    }
}
