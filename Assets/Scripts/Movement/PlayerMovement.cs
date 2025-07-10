using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 120f;
    private Animator animator;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public float groundedOffset = -0.1f; // Corrige pequeñas inconsistencias de detección
    public float groundedRadius = 0.3f;
    public LayerMask groundLayers;

    private CharacterController controller;
    private InputSystem_Actions inputActions;
    private float moveInput;
    private float rotateInput;
    private PlayerCombatMode combatScript;

    private float verticalVelocity;
    private bool isGrounded;
    private bool playerInZone = false;
    public CameraTrigger targetCameraTrigger;

    [Header("Ground Check")]
    public Transform groundCheck;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputActions = new InputSystem_Actions();
        combatScript = GetComponent<PlayerCombatMode>();
        
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => ReadInput(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled += ctx => ReadInput(Vector2.zero);
        inputActions.Player.Interact.performed += OnInteract;

    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => ReadInput(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled -= ctx => ReadInput(Vector2.zero);
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Disable();
    }

    private void ReadInput(Vector2 input)
    {
        moveInput = input.y;
        rotateInput = input.x;
        animator.SetBool("isWalking", input.y > 0);
        animator.SetBool("isWalkingBackwards", input.y < 0);
    }

    private void Update()
    {
        if (combatScript != null && combatScript.IsInCombatMode())
            return;

        CheckGround();
        ApplyGravity();
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
        Vector3 move = forward * moveSpeed;

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

  
    private void CheckGround()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }
        else
        {
            isGrounded = controller.isGrounded;
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Mantener al personaje "pegado" al suelo
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzleZone")) // O usa "CameraZone" o el tag que uses
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PuzzleZone"))
        {
            playerInZone = false;
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (playerInZone && targetCameraTrigger != null)
        {
            targetCameraTrigger.enabled = true;
        }
    }
}

