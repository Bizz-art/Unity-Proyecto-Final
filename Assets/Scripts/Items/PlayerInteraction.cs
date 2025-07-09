using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactableLayer;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.RecogerItem.performed += ctx => TryInteract();
    }

    private void OnDisable()
    {
        inputActions.Player.RecogerItem.performed -= ctx => TryInteract();
        inputActions.Player.Disable();
    }

    void TryInteract()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);
        foreach (var hit in hits)
        {
            var item = hit.GetComponent<ItemPickup>();
            if (item != null)
            {
                item.OnInspect();
                break;
            }
        }
    }
}
