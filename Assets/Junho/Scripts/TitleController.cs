using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public AudioSource audioSource;

    public void StarGameButton()
    {
        audioSource.Play();
        SceneManager.LoadScene(1);
    }

    public void ExitGameButton()
    {
        audioSource.Play();
        Application.Quit();
    }
}


