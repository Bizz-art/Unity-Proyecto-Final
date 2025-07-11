using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class NewGameManager : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource audioSource;
    public AudioClip startSound;
    public Image fadeImage;
    public string sceneToLoad = "Intro";
    public float fadeDuration = 1f;

    private bool isTransitioning = false;

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); // Activamos para que pueda hacer fade in
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
            StartCoroutine(FadeIn());
        }
    }

    public void OnNewGamePressed()
    {
        if (!isTransitioning)
        {
            StartCoroutine(PlaySoundAndLoadScene());
        }
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        isTransitioning = true;

        if (audioSource && startSound)
        {
            audioSource.PlayOneShot(startSound);
        }

        yield return new WaitForSeconds(3f);

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); // Activar justo antes del fade
            yield return StartCoroutine(FadeOut());
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 0f);
        fadeImage.gameObject.SetActive(false); // Desactivar para no estorbar
    }
}
