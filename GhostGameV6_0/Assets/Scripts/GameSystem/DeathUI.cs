using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    const int MenuSceneIndex = 0;
    const int WorldSceneIndex = 1;

    public void LoadGame()
    {
        Time.timeScale = 1f;
        if (!SaveSystem.HasSaveFile())
        {
            Debug.Log("Cant find save Game");
            return;
        }

        PlayerPrefs.SetInt("IsNewGame", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(WorldSceneIndex);
    }

    public void HomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenuSceneIndex);
    }
}
