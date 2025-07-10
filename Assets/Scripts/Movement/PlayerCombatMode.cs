using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatMode : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private bool isInCombatMode = false;

   [Header("References")]
    public GameObject targetingUI; // Opcional: UI o retícula de combate
    private Animator animator;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        animator = GetComponent<Animator>();
    }



    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.EnterCombat.performed += OnAttackModeToggle;
    }
    private void OnDisable()
    {
        inputActions.Player.EnterCombat.performed -= OnAttackModeToggle;
        inputActions.Player.Disable();
    }

    private void OnAttackModeToggle(InputAction.CallbackContext context)
    {
        isInCombatMode = !isInCombatMode;

        if (targetingUI != null)
            targetingUI.SetActive(isInCombatMode);
        bool currentState = animator.GetBool("hasKnife");
        animator.SetBool("hasKnife", !currentState);
    }

    public bool IsInCombatMode()
    {
        return isInCombatMode;
    }
}
