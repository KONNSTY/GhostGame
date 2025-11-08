using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public void LoadGame()
    {
        Debug.Log("Cant find save Game");
    }

    public void HomeButton()
    {
        SceneManager.LoadScene(0);
}
}
