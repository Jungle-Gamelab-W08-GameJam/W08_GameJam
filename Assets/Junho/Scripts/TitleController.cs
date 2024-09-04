using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StarGameButton()
    {
        SceneManager.LoadScene(1);

    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}


