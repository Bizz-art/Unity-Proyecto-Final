using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour
{
    public Sprite[] comicPages;      // Las imágenes de tu cómic
    public Image displayImage;       // El Image UI donde se mostrará
    public Text clickText;           // El texto "Click para continuar"

    public AudioSource audioSource;  // Fuente de audio
public AudioClip pageTurnSound;  // Sonido al pasar página


    private int currentPage = 0;
    private float blinkTimer = 0f;
    private bool isTextVisible = true;
    public float blinkInterval = 0.5f; // Parpadea cada 0.5 segundos

    void Start()
    {
        if (comicPages.Length > 0)
        {
            displayImage.sprite = comicPages[0];
        }
        clickText.gameObject.SetActive(true);
    }

    void Update()
    {
        // Parpadeo del texto
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            isTextVisible = !isTextVisible;
            clickText.enabled = isTextVisible;
        }

        // Avanzar página con click/tap
        if (Input.GetMouseButtonDown(0))
        {
            AdvancePage();
        }
    }

    void AdvancePage()
    {
        currentPage++;
        if (audioSource != null && pageTurnSound != null)
{
    audioSource.PlayOneShot(pageTurnSound);
}


        if (currentPage < comicPages.Length)
        {
            displayImage.sprite = comicPages[currentPage];
        }
        else
        {
            // Se terminó el cómic, desactivar texto y cargar juego
            clickText.gameObject.SetActive(false);
            StartGame();
        }
    }

    void StartGame()
    {
        // Cambia aquí por el nombre de tu escena de juego
        SceneManager.LoadScene("Game");
    }
}
