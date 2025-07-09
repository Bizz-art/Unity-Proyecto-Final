using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SecuenciaVisualizer : MonoBehaviour
{
    public GameObject iconPrefab;           // Prefab que contiene imagen del input
    public Transform container;             // Panel con HorizontalLayoutGroup
    public Sprite jSprite;                  // Sprite para 'J' (cuadro)
    public Sprite kSprite;                  // Sprite para 'K' (círculo)
    public Sprite errorSprite;              // Sprite de X roja (opcional)

    private List<GameObject> activeIcons = new();

    public void MostrarInput(string input)
    {
        GameObject icon = Instantiate(iconPrefab, container);
        Image img = icon.GetComponent<Image>();

        if (input == "J") img.sprite = jSprite;
        else if (input == "K") img.sprite = kSprite;

        activeIcons.Add(icon);
    }

    public void MostrarError()
    {
        foreach (GameObject icon in activeIcons)
        {
            Transform x = icon.transform.Find("ErrorX");
            if (x != null) x.gameObject.SetActive(true);
        }
    }

    public void Reiniciar()
    {
        foreach (GameObject icon in activeIcons)
        {
            Destroy(icon);
        }
        activeIcons.Clear();
    }
}
