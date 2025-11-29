using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void Settings()
    {

    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
