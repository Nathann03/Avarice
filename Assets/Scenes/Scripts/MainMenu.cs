using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void hide_panel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void show_panel(GameObject panel)
    {
        panel.SetActive(true);
    }

}
