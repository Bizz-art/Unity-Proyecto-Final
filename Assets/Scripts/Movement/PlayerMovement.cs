using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float runSpeed = 6f; // velocidad para correr
    public float rotationSpeed = 120f;
    private Animator animator;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public float groundedOffset = -0.1f;
    public float groundedRadius = 0.3f;
    public LayerMask groundLayers;

    [Header("Audio")]
    public AudioSource stepAudioSource; // AudioSource con sonido de pasos caminando
    public AudioSource runAudioSource;  // AudioSource con sonido de pasos corriendo

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
        if (PuzzleCameraTrigger.EnPuzzle)
        {
            SetMovementAnimations(Vector2.zero);
            StopStepSounds();
            return;
        }

        if (combatScript != null && combatScript.IsInCombatMode())
        {
            SetMovementAnimations(Vector2.zero);
            StopStepSounds();
            return;
        }

        CheckGround();
        ApplyGravity();
        Rotate();
        Move();
        HandleStepSounds();

        SetMovementAnimations(new Vector2(rotateInput, moveInput));
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * rotateInput * rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        bool isRunning = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        Vector3 forward = transform.forward * moveInput;
        Vector3 move = forward * currentSpeed;

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    private void SetMovementAnimations(Vector2 input)
    {
        animator.SetBool("isWalking", input.y > 0.1f);
        animator.SetBool("isWalkingBackwards", input.y < -0.1f);
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
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void HandleStepSounds()
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.1f && isGrounded;
        bool isRunning = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;

        if (isMoving)
        {
            if (isRunning)
            {
                if (!runAudioSource.isPlaying)
                {
                    stepAudioSource.Stop();
                    runAudioSource.Play();
                }
            }
            else
            {
                if (!stepAudioSource.isPlaying)
                {
                    runAudioSource.Stop();
                    stepAudioSource.Play();
                }
            }
        }
        else
        {
            StopStepSounds();
        }
    }

    private void StopStepSounds()
    {
        if (stepAudioSource.isPlaying) stepAudioSource.Stop();
        if (runAudioSource.isPlaying) runAudioSource.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzleZone"))
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

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetMoveInput()
    {
        return moveInput;
    }

    public bool IsRunning()
    {
        return Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
    }
}
