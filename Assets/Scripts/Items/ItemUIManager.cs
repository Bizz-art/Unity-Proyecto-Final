using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIManager : MonoBehaviour
{
    public static ItemUIManager Instance;

    [Header("UI References")]
    public GameObject itemPanel;
    public TextMeshProUGUI itemNameText;
    public Image itemSpriteImage; // NUEVO: Imagen UI para mostrar el sprite
    public Button yesButton;
    public Button noButton;

    private ItemPickup currentItem;

    private void Awake()
    {
        Instance = this;
        itemPanel.SetActive(false);
    }

    public void ShowItemPanel(ItemPickup item)
    {
        currentItem = item;
        itemPanel.SetActive(true);
        itemNameText.text = item.itemName;

        // Mostrar sprite
        if (item.itemSprite != null)
        {
            itemSpriteImage.sprite = item.itemSprite;
            itemSpriteImage.gameObject.SetActive(true);
        }
        else
        {
            itemSpriteImage.gameObject.SetActive(false);
        }

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => currentItem.OnCollect());
        noButton.onClick.AddListener(() => currentItem.OnCancel());
    }

    public void HideItemPanel()
    {
        itemPanel.SetActive(false);
        currentItem = null;

        itemSpriteImage.sprite = null;
        itemSpriteImage.gameObject.SetActive(false);
    }
}
