using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite; // NUEVO: sprite del ítem

    public void OnInspect()
    {
        ItemUIManager.Instance.ShowItemPanel(this);
    }

    public void OnCollect()
    {
        Destroy(gameObject);
        ItemUIManager.Instance.HideItemPanel();
    }

    public void OnCancel()
    {
        ItemUIManager.Instance.HideItemPanel();
    }
}
