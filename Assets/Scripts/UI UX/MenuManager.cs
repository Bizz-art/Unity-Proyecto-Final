using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame(){
        SceneManager.LoadScene("Intro");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
