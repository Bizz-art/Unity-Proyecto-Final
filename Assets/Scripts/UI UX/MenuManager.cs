using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource audioSource; // Arrastra aquí tu AudioSource con el sonido
    public AudioClip startSound;    // Asigna el clip aquí
    public string sceneToLoad = "Game"; // O el nombre de tu escena

    private bool isStarting = false;

    public void OnNewGamePressed()
    {
        if (!isStarting)
        {
            StartCoroutine(PlaySoundAndStart());
        }
    }

    private IEnumerator PlaySoundAndStart()
    {
        isStarting = true;
        audioSource.PlayOneShot(startSound);
        yield return new WaitForSeconds(startSound.length);
        SceneManager.LoadScene(sceneToLoad);
    }
    public void OnExitPressed()
    {
        Application.Quit();
        
    }

}
